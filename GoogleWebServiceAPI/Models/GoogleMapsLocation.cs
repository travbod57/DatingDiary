using System.Device.Location;
using System.Runtime.Serialization;

namespace GoogleWebServiceApi.Models
{
    [DataContract]
    public class GoogleMapsLocation
    {


        [DataMember(Name="lat")]
        public double Latitude { get; set; }

        [DataMember(Name="lng")]
        public double Longitude { get; set; }

     
        public GeoCoordinate GetGeoCoordinate()
        {
            return new GeoCoordinate(Latitude, Longitude);
        }

    }
}
