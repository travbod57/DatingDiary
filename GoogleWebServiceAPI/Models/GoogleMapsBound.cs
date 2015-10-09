using System.Runtime.Serialization;

namespace GoogleWebServiceApi.Models
{
    [DataContract]
    public class GoogleMapsBound
    {

        [DataMember(Name = "southwest")]
        public GoogleMapsLocation SouthWest { get; set; }

        [DataMember(Name = "northeast")]
        public GoogleMapsLocation NorthEast { get; set; }

        
    }
}