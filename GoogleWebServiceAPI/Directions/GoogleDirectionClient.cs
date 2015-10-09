using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization.Json;
using GoogleWebServiceApi.Directions.Model;

namespace GoogleWebServiceApi.Directions
{


    /// <summary>
    /// Represents Google Map Direction Web Service
    /// Please refer to https://developers.google.com/maps/documentation/directions/
    /// </summary>
    public class GoogleDirectionClient
    {
        private string _appId;

        public delegate void GoogleDirectionNotFoundDelegate(GoogleDirectionResponse response, GoogleDirectionRequest request);
        public delegate void GoogleDirectionFailedDelegate(string message, Exception e);
        public delegate void GoogleDirectionFoundDelegate(GoogleDirectionResponse response, GoogleDirectionRequest request);

        /// <summary>
        /// Event raied when ResolveDirectionAsync() cannot resolve any result
        /// </summary>
        public event GoogleDirectionNotFoundDelegate DirectionNotFound;
        /// <summary>
        /// Event raised when ResolveDirectionAsync() returns a result
        /// </summary>
        public event GoogleDirectionFoundDelegate DirectionFound;
        /// <summary>
        /// Event raised when ResolveDirectionAsync() returns an error
        /// </summary>
        public event GoogleDirectionFailedDelegate DirectionFailed;


        private const string Request_Url =
            "http://maps.googleapis.com/maps/api/directions/json?appId={0}&origin={1}&destination={2}&sensor={3}";

        public GoogleDirectionClient(string appId)
        {
            _appId = appId;
        }


     

        /// <summary>
        /// Synchroniously resolve a direction
        /// </summary>
        public void ResolveDirectionAsync(GoogleDirectionRequest googleRequest)
        {

			var requestUrl = GenerateRequestUrl(googleRequest.Olatitude, googleRequest.Olongitude, googleRequest.Dlatitude, googleRequest.Dlongitude, googleRequest.Waypoints, googleRequest.Alternatives, googleRequest.TravelMode);
            var request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
            request.Method = "Post";

            try
            {
                request.BeginGetResponse(ResponseCallBackAsync, new List<object> { request, googleRequest });
            }
            catch (Exception e)
            {
                DirectionFailed(e.Message, e);
            }
        }

        private void ResponseCallBackAsync(IAsyncResult ar)
        {
            ResposeCallbackBase(ar, DirectionNotFound, DirectionFound, DirectionFailed);
        }


        private string GenerateRequestUrl(double olatitude, double olongitude, double dlatitude, double dlongitude, string waypoints, bool alternatives, GoogleDirectionTravelMode travelMode)
        {
            var urlSoFar = string.Format(Request_Url, _appId, olatitude + "," + olongitude, dlatitude + "," + dlongitude, true.ToString().ToLower());
			
            if (!string.IsNullOrEmpty(urlSoFar))
                urlSoFar = string.Concat(urlSoFar, "&waypoints=", waypoints);
            if (!string.IsNullOrEmpty(urlSoFar))
                urlSoFar = string.Concat(urlSoFar, "&alternatvies=", alternatives.ToString().ToLower());
			if (!string.IsNullOrEmpty(urlSoFar))
				urlSoFar = string.Concat(urlSoFar, "&mode=", Enum.GetName(typeof (GoogleDirectionTravelMode), travelMode));
            return urlSoFar;
        }



        private void ResposeCallbackBase(IAsyncResult result, GoogleDirectionNotFoundDelegate notfoundDelegate,
            GoogleDirectionFoundDelegate foundDelegate, GoogleDirectionFailedDelegate failedDelegate)
        {
            var state = (List<object>)result.AsyncState;
            var googleRequest = (GoogleDirectionRequest)state[1];
            if (result.IsCompleted)
            {
                try
                {
                    var request = (HttpWebRequest)state[0];
                    var response = (HttpWebResponse)request.EndGetResponse(result);
                    var responseStream = response.GetResponseStream();
                    var serializer = new DataContractJsonSerializer(typeof(GoogleDirectionResponse));
                    var googleSearchResponse = (GoogleDirectionResponse)serializer.ReadObject(responseStream);
                    if (googleSearchResponse.Status == GoogleDirectionResponseStatusCode.OK)
                    {
                        foundDelegate(googleSearchResponse, googleRequest);
                    }
                    else if (googleSearchResponse.Status == GoogleDirectionResponseStatusCode.ZERO_RESULTS)
                    {
                        notfoundDelegate(googleSearchResponse, googleRequest);
                    }
                }
                catch (Exception e)
                {
                    if (failedDelegate != null) 
                     failedDelegate(e.Message, e);
                }
            }
            else
            {
                if (failedDelegate == null) throw new Exception("For some reason request is not completed");
                failedDelegate("For some reason request is not completed", null);

            }
        }
    }
}