using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization.Json;
using GoogleWebServiceApi.Models;
using GoogleWebServiceApi.Places.Models;

namespace GoogleWebServiceAPI.Places
{
    /// <summary>
    /// Represents the Google Places API
    /// </summary>
    public class GooglePlacesClient
    {
        public delegate void GooglePlacesSearchForPlaceDelegate(GooglePlaceSearchResponse response, GooglePlaceSearchRequest request);
		public delegate void GooglePlacesSearchForPlaceNotFoundDelegate(GooglePlaceSearchResponse response, GooglePlaceSearchRequest request);
    	public delegate void GooglePlacesSearchForPlaceFailedDelegate(string errorMessage, Exception e);
        private string _appId;
		public event GooglePlacesSearchForPlaceNotFoundDelegate SearchForPlacesAsyncNotFound;
        public event GooglePlacesSearchForPlaceDelegate SearchForPlacesAsyncCompleted;
    	public event GooglePlacesSearchForPlaceFailedDelegate SearchForPlacesAsyncFailed;
		private  const string GooglePlacesAPI_Url = "https://maps.googleapis.com/maps/api/place/search/json?key={0}&location={1},{2}&radius={3}&sensor={4}&keyword={5}&language={6}&types={7}";
        public GooglePlacesClient(string appId)
        {
            _appId = appId;
        }

        public void SearchForPlaceAync(GooglePlaceSearchRequest searchRequest )
        {
        	var language = searchRequest.Language.HasValue
        	               	? Enum.GetName(typeof (GoogleMapsLanguage), searchRequest.Language.Value).Replace("_", "-")
        	               	: null;
        	var types = searchRequest.Types == null?string.Empty: string.Join("|", searchRequest.Types);
			var requestUrl = string.Format(GooglePlacesAPI_Url, _appId, searchRequest.LocationLat, searchRequest.LocationLong, searchRequest.Radius, searchRequest.Sensor.ToString().ToLower(), searchRequest.Keyword, language, types);
			var request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
            request.Method = "Post";
        	
			try
			{
				request.BeginGetResponse(ResponseCallback, new List<object> {request, searchRequest});
			}
        	catch(Exception e)
        	{
				SearchForPlacesAsyncFailed(e.Message, e);
        	}
			
        }

        private void ResponseCallback(IAsyncResult result)
        {
        	var state = (List<object>) result.AsyncState;
        	var googleRequest = (GooglePlaceSearchRequest) state[1];
            if (result.IsCompleted)
            {
				try
				{
					var request = (HttpWebRequest)state[0];
					var response = (HttpWebResponse)request.EndGetResponse(result);
					var responseStream = response.GetResponseStream();
					var serializer = new DataContractJsonSerializer(typeof(GooglePlaceSearchResponse));
					var googleSearchResponse = (GooglePlaceSearchResponse)serializer.ReadObject(responseStream);
					if (googleSearchResponse.Status == GoogleMapsSearchStatus.OK)
						SearchForPlacesAsyncCompleted(googleSearchResponse, googleRequest);	
					else if (googleSearchResponse.Status == GoogleMapsSearchStatus.ZERO_RESULTS)
						SearchForPlacesAsyncNotFound(googleSearchResponse, googleRequest);
				} catch(Exception e)
				{
                    if (SearchForPlacesAsyncFailed!= null)
					    SearchForPlacesAsyncFailed(e.Message, e);
				}
            } else
            {
                if (SearchForPlacesAsyncFailed != null)
            	    SearchForPlacesAsyncFailed("For some reason request is not completed", null);

            }
        }

		


    }
}
