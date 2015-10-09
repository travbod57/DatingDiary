using System;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading;
using GoogleWebServiceApi.Geocode.Model;
using GoogleWebServiceApi.Models;
using System.Collections.Generic;

namespace GoogleWebServiceAPI.GeoCode
{
    /// <summary>
    /// Represents Google Geocoding API - https://developers.google.com/maps/documentation/places/
    /// Please refer to: https://developers.google.com/maps/documentation/places/
    /// </summary>
	public class GoogleGeocodeClient
	{
		private string _appId;
	    private AutoResetEvent _autoResetEvent;
		public delegate void GooglePlacesGeocodeFoundDelegate(GoogleGeocodeResponse response, GoogleGeocodeRequest request);
		public delegate void GooglePlacesGeocodeNotFoundDelegate(GoogleGeocodeResponse response, GoogleGeocodeRequest request);
		public delegate void GooglePlacesGeocodeFailedDelegate(string errorMessage, Exception e);
        public event GooglePlacesGeocodeNotFoundDelegate GeocodeNotFound;
		public event GooglePlacesGeocodeFoundDelegate  GeocodeFound;
	    public event GooglePlacesGeocodeFailedDelegate GeocodeFailed;


        
        public event GooglePlacesGeocodeNotFoundDelegate SyncGeocodeNotFound;
        public event GooglePlacesGeocodeFoundDelegate SyncGeocodeFound;
        

	    private GoogleGeocodeResponse _syncGoogleResponse;

		private const string Request_Url =
			"http://maps.googleapis.com/maps/api/geocode/json?appId={0}&address={1}&bounds={2}&language={3}&sensor={4}";

		private const string Reverse_Geocode_Request_Url =
			"http://maps.googleapis.com/maps/api/geocode/json?appId={0}&latlng={1}&sensor={2}";
			
		public GoogleGeocodeClient(string appId)
		{
			_appId = appId;
		    _autoResetEvent = new AutoResetEvent(false);
            SyncGeocodeNotFound += SyncronizedGeocodeFoundHandler;
            SyncGeocodeFound += SyncronizedGeocodeNotFoundHandler;


		}


	    private void SyncronizedGeocodeNotFoundHandler(GoogleGeocodeResponse response, GoogleGeocodeRequest request)
	    {
            _syncGoogleResponse = response;
	        _autoResetEvent.Set();
	    }

	    private void SyncronizedGeocodeFoundHandler( GoogleGeocodeResponse googleResponse, GoogleGeocodeRequest googleRequest)
	    {
	        _syncGoogleResponse = googleResponse;
	        _autoResetEvent.Set();
        }

        /// <summary>
        /// Resolve a Geocode (synchronously)
        /// </summary>
        /// <param name="googleRequest"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        /// <remarks>Does not work as of the moment</remarks>
        public GoogleGeocodeResponse ResolveGeoCode(GoogleGeocodeRequest googleRequest, int timeout)
        {
            
            var requestUrl = GenerateRequestUrl(googleRequest);
            var request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
            request.Method = "GET";

            request.BeginGetResponse(ResponseCallbackSync, new List<object> { request, googleRequest });
            _autoResetEvent.WaitOne(timeout);
            return _syncGoogleResponse;
            
        }



        /// <summary>
        /// Resolve a Geocode (asynchronously)
        /// </summary>
        /// <param name="googleRequest"></param>
		public void ResolveGeoCodeAsync(GoogleGeocodeRequest googleRequest)
		{
            
			var requestUrl = GenerateRequestUrl(googleRequest);
		    var request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
			request.Method = "Post";

			try
			{
				request.BeginGetResponse(ResponseCallback, new List<object> { request, googleRequest });
			}
			catch (Exception e)
			{
				GeocodeFailed(e.Message, e);
			}
		}

	    private string GenerateRequestUrl(GoogleGeocodeRequest googleRequest)
	    {
	        var language = googleRequest.Language.HasValue
	                           ? Enum.GetName(typeof (GoogleMapsLanguage), googleRequest.Language.Value).Replace("_", "-")
	                           : string.Empty;
	        var bounds = googleRequest.BoundSouthWestLat != null && googleRequest.BoundNorthEastLat != null
	                         ? googleRequest.BoundSouthWestLat+ "," + googleRequest.BoundSouthWestLong+ "|" +
	                           googleRequest.BoundNorthEastLat + "," + googleRequest.BoundNorthEastLong
	                         : string.Empty;
	        var latlong = googleRequest.Latitude == null
	                          ? string.Empty
	                          : googleRequest.Latitude + "," + googleRequest.Longitude;
	        var address = string.IsNullOrEmpty(googleRequest.Address)
	                          ? string.Empty
	                          : Uri.EscapeDataString(googleRequest.Address);
	        var requestUrl =
	            !string.IsNullOrEmpty(latlong)
	                ? string.Format(Reverse_Geocode_Request_Url, _appId, latlong, googleRequest.Sensor.ToString().ToLower())
	                : string.Format(Request_Url, _appId, address, bounds, language, googleRequest.Sensor.ToString().ToLower())
	            ;
	        return requestUrl;
	    }


        private void ResposeCallbackBase(IAsyncResult  result, GooglePlacesGeocodeNotFoundDelegate notfoundDelegate,
            GooglePlacesGeocodeFoundDelegate foundDelegate, GooglePlacesGeocodeFailedDelegate failedDelegate)
        {
            var state = (List<object>)result.AsyncState;
			var googleRequest = (GoogleGeocodeRequest)state[1];
			if (result.IsCompleted)
			{
				try
				{
					var request = (HttpWebRequest)state[0];
					var response = (HttpWebResponse)request.EndGetResponse(result);
					var responseStream = response.GetResponseStream();
					var serializer = new DataContractJsonSerializer(typeof(GoogleGeocodeResponse));
					var googleSearchResponse = (GoogleGeocodeResponse)serializer.ReadObject(responseStream);
					if (googleSearchResponse.Status == GoogleMapsSearchStatus.OK)
					{
                        foundDelegate(googleSearchResponse, googleRequest);
					}
					else if (googleSearchResponse.Status == GoogleMapsSearchStatus.ZERO_RESULTS)
					{
                        notfoundDelegate(googleSearchResponse, googleRequest);
					}
				}
				catch (Exception e)
				{
				    if (failedDelegate == null) throw e;
				    failedDelegate(e.Message, e);
				}
			}
			else
			{
                if (failedDelegate == null) throw new Exception("For some reason request is not completed");
                failedDelegate("For some reason request is not completed", null);

			}
        }


        private void ResponseCallbackSync(IAsyncResult result)
        {
            ResposeCallbackBase(result, SyncGeocodeNotFound, SyncGeocodeFound, null);
        }

	    private void ResponseCallback(IAsyncResult result)
	    {
	        ResposeCallbackBase(result, GeocodeNotFound, GeocodeFound, GeocodeFailed);
	    }
	}
}