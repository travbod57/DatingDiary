using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoogleWebServiceApi.Models;
using GoogleWebServiceAPI.Places;

namespace GoogleWebServiceApi.Places.Models
{
	[DataContract]
	public class GooglePlaceSearchResponse: GoogleWebServiceResponseBase, IGoogleMapsResponse
	{
		[DataMember(Name = "html_attributions")]
		public string HtmlAttributions { get;  set; }



		[DataMember(Name="results")]
		public List<GooglePlaceSearchResult> Results { get;  set; }


	}
}