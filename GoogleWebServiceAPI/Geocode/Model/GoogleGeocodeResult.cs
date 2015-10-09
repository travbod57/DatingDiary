using System.Runtime.Serialization;
using GoogleWebServiceApi.Models;

namespace GoogleWebServiceApi.Geocode.Model
{
    /// <summary>
    /// Represents a single result of Google Gecoding API
    /// </summary>
	[DataContract]
	public class GoogleGeocodeResult
	{
        /// <summary>
        /// THe formated address
        /// </summary>
		[DataMember(Name="formatted_address")]
		public string FormattedAddress { get; set; }


        /// <summary>
        /// The Geometry
        /// </summary>
		[DataMember(Name="geometry")]
		public GoogleMapsGeometry Geometry { get; set; }

        /// <summary>
        /// The Address components of the result address
        /// </summary>
        [DataMember(Name="address_components")]
        public GoogleMapAddressComponent[] AddressComponents { get; set; }
	}
}