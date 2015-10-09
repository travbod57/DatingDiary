using System.Runtime.Serialization;

namespace GoogleWebServiceApi.PlacesAutocomplete.Models
{
	[DataContract]
	public class GooglePlaceAutoCompletePrediction
	{

		[DataMember(Name="description")]
		public string Description { get; set; }



	}
}
