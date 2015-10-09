using System.Collections.Generic;
using System.Runtime.Serialization;
using GoogleWebServiceApi.Models;
using GoogleWebServiceApi.PlacesAutocomplete.Models;

namespace GoogleWebServiceApi.PlacesAutocomplete
{
	[DataContract]
	public class GooglePlaceAutoCompleteResponse:GoogleWebServiceResponseBase
	{

		[DataMember(Name="predictions")]
		public List<GooglePlaceAutoCompletePrediction> Predictions { get; set; }

		
	}
}
