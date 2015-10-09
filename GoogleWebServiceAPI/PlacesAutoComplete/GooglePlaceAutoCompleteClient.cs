using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization.Json;
using GoogleWebServiceApi.Models;
using GoogleWebServiceApi.PlacesAutocomplete.Models;

namespace GoogleWebServiceApi.PlacesAutocomplete
{

    /// <summary>
    /// represents Google Place Autocomplete web service
    /// plese refer to https://developers.google.com/maps/documentation/places/autocomplete
    /// </summary>
    public class GooglePlaceAutoCompleteClient
    {

    	private string _appId;
		public delegate void GooglePlaceAutoCompleteDelegate(GooglePlaceAutoCompleteResponse response, GooglePlaceAutoCompleteRequest request);
		public delegate void GooglePlaceAutoCompleteNotFoundDelegate(GooglePlaceAutoCompleteResponse response, GooglePlaceAutoCompleteRequest request);
		public delegate void GooglePlaceAutoCompleteFailedDelegate(string errorMessage, Exception e);
		public event GooglePlaceAutoCompleteNotFoundDelegate GooglePlaceAutoCompleteAsyncNotFound;
		public event GooglePlaceAutoCompleteDelegate GooglePlaceAutoCompleteAsyncCompleted;
		public event GooglePlaceAutoCompleteFailedDelegate GooglePlaceAutoCompleteAsyncFailed;
    	public const string Request_Url =
    		"http://maps.googleapis.com/maps/api/place/autocomplete/json?key={0}&input={1}&sensor={2}&location={3}&radius={4}&language={5}";

		public GooglePlaceAutoCompleteClient(string appId)
		{
			_appId = appId;
		}

		public void Autocomplete(GooglePlaceAutoCompleteRequest autocompleteRequest)
		{
			var language = autocompleteRequest.Language.HasValue
							? Enum.GetName(typeof(GoogleMapsLanguage), autocompleteRequest.Language.Value).Replace("_", "-")
							: null;
			var latlong = autocompleteRequest.LocationLat == null
							? string.Empty
							: autocompleteRequest.LocationLat + "," + autocompleteRequest.LocationLong;
            var query = Uri.EscapeDataString(autocompleteRequest.Input);
            var requestUrl = string.Format(Request_Url, _appId, query, autocompleteRequest.Sensor.ToString().ToLower(), latlong, autocompleteRequest.Radius, language);
			var request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
			request.Method = "Post";
			try
			{
				request.BeginGetResponse(ResponseCallback, new List<object> { request, autocompleteRequest });
			}
			catch (Exception e)
			{
				GooglePlaceAutoCompleteAsyncFailed(e.Message, e);
			}

		}

    	private void ResponseCallback(IAsyncResult result)
    	{
			var state = (List<object>)result.AsyncState;
			var googleRequest = (GooglePlaceAutoCompleteRequest)state[1];
			if (result.IsCompleted)
			{
				try
				{
					var request = (HttpWebRequest)state[0];
					var response = (HttpWebResponse)request.EndGetResponse(result);
					var responseStream = response.GetResponseStream();
					var serializer = new DataContractJsonSerializer(typeof(GooglePlaceAutoCompleteResponse));
					var googleSearchResponse = (GooglePlaceAutoCompleteResponse)serializer.ReadObject(responseStream);
					if (googleSearchResponse.Status == GoogleMapsSearchStatus.OK)
						GooglePlaceAutoCompleteAsyncCompleted(googleSearchResponse, googleRequest);
					else if (googleSearchResponse.Status == GoogleMapsSearchStatus.ZERO_RESULTS && GooglePlaceAutoCompleteAsyncNotFound!= null)
						GooglePlaceAutoCompleteAsyncNotFound(googleSearchResponse, googleRequest);
				}
				catch (Exception e)
				{
					GooglePlaceAutoCompleteAsyncFailed(e.Message, e);
				}
			}
			else
			{
				GooglePlaceAutoCompleteAsyncFailed("For some reason request is not completed", null);

			}
    	}
    }
}
