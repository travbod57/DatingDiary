using System.Device.Location;

namespace GoogleWebServiceApi.StaticMaps.Model
{
    /// <summary>
    /// Representation of a single Marker in a Google Static Map
    /// Please refer to https://developers.google.com/maps/documentation/staticmaps/#MarkerStyles
    /// </summary>
    public class GoogleStaticMapMarker
    {

        public GoogleStaticMapMarker(string markerAddress)
        {
            MarkerAddress = markerAddress;
        }


        public GoogleStaticMapMarker(GeoCoordinate markerCoordinate)
        {
            MarkerCoordinate = markerCoordinate;
        }

        /// <summary>
        /// Lat long or the marker, if address is defined, then address will be used instead
        /// </summary>
        public GeoCoordinate MarkerCoordinate { get; set; }


        /// <summary>
        /// Address of the marker
        /// </summary>
        public string MarkerAddress { get; set; }


        
    }
}