using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using GoogleWebServiceApi.Places.Models;
using GoogleWebServiceApi.Models;
using System.Globalization;
using System.Runtime.Serialization.Json;

namespace DatingDiary.Google
{
		/// <summary>
		/// Represents the Google Places API
		/// </summary>
		public class GooglePlacesTextSearchClient
		{
			public delegate void GooglePlacesSearchForPlaceDelegate(GooglePlaceSearchResponse response, GooglePlaceSearchRequest request);
			public delegate void GooglePlacesSearchForPlaceNotFoundDelegate(GooglePlaceSearchResponse response, GooglePlaceSearchRequest request);
			public delegate void GooglePlacesSearchForPlaceFailedDelegate(string errorMessage, Exception e);
			private string _appId;
            private WebRequest _webRequest;
			public event GooglePlacesSearchForPlaceNotFoundDelegate SearchForPlacesAsyncNotFound;
			public event GooglePlacesSearchForPlaceDelegate SearchForPlacesAsyncCompleted;
			public event GooglePlacesSearchForPlaceFailedDelegate SearchForPlacesAsyncFailed;

			// maximum 50,000 m radius
			private const string GooglePlacesAPI_Url = "https://maps.googleapis.com/maps/api/place/textsearch/json?key={0}&sensor={1}&query={2}&rankby=prominence";
			public GooglePlacesTextSearchClient(string appId)
			{
				_appId = appId;
			}

			public void SearchForPlaceAync(GooglePlaceSearchRequest searchRequest)
			{
				var language = searchRequest.Language.HasValue ? Enum.GetName(typeof(GoogleMapsLanguage), searchRequest.Language.Value).Replace("_", "-") : null;
				var types = searchRequest.Types == null ? string.Empty : string.Join("|", searchRequest.Types);
				var requestUrl = string.Format(GooglePlacesAPI_Url, _appId, searchRequest.Sensor.ToString().ToLower(), searchRequest.Keyword);
                _webRequest = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
                _webRequest.Method = "Post";

				try
				{
                    _webRequest.BeginGetResponse(ResponseCallback, new List<object> { _webRequest, searchRequest });
				}
				catch (Exception e)
				{
					SearchForPlacesAsyncFailed(e.Message, e);
				}

			}

			private void ResponseCallback(IAsyncResult result)
			{
				var state = (List<object>)result.AsyncState;
				var googleRequest = (GooglePlaceSearchRequest)state[1];
                
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
					}
					catch (Exception e)
					{
						if (SearchForPlacesAsyncFailed != null)
							SearchForPlacesAsyncFailed(e.Message, e);
					}
				}
				else
				{
					if (SearchForPlacesAsyncFailed != null)
						SearchForPlacesAsyncFailed("For some reason request is not completed", null);

				}
			}

            public void CancelWebRequest()
            {
                _webRequest.Abort();
            }
		}
}
