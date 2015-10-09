using System.Collections.Generic;
using System.Runtime.Serialization;
using GoogleWebServiceApi.Models;

namespace GoogleWebServiceApi.Geocode.Model
{


    /// <summary>
    /// Represents the response of the Google Geocode api
    /// </summary>
	[DataContract]
	public class GoogleGeocodeResponse : GoogleWebServiceResponseBase, IGoogleMapsResponse
	{
		
        /// <summary>
        /// all results
        /// </summary>
		[DataMember(Name= "results")]
		public List<GoogleGeocodeResult> Results { get; set; }
	}
}
