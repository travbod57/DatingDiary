using System.Runtime.Serialization;
using GoogleWebServiceApi.Places.Models;

namespace GoogleWebServiceApi.Models
{
    [DataContract]
	public class GoogleMapsGeometry
	{
        [DataMember(Name="location")]
        public GoogleMapsLocation Location { get; set; }

	}
}