namespace GoogleWebServiceApi.Directions.Model
{
    /// <summary>
    /// The request parameters for the google direction api
    /// Please refer to https://skydrive.live.com/redir?resid=EB299F417D3831B5!3472&authkey=!ADL0bUDRwcptODw 
    /// </summary>
    public class GoogleDirectionRequest
    {
        /// <summary>
        /// Origin Latitude
        /// </summary>
        public double Olatitude { get; set; }

        /// <summary>
        /// Origin Longitude
        /// </summary>
        public double Olongitude { get; set; }

        /// <summary>
        /// Destination Latitude
        /// </summary>
        public double Dlatitude { get; set; }

        /// <summary>
        /// Destination Longitude
        /// </summary>
        public double Dlongitude { get; set; }

        /// <summary>
        /// Waypoint string TODO: more robust in the future
        /// </summary>
        public string Waypoints { get; set; }

        /// <summary>
        /// Travel Mode
        /// </summary>
        public GoogleDirectionTravelMode TravelMode { get; set; }

        /// <summary>
        /// Alternative
        /// </summary>
        public bool Alternatives { get; set; }


        /// <summary>
        /// Create a Request for Google Direction
        /// </summary>
        /// <param name="olatitude">Origin Latitude</param>
        /// <param name="olongitude">Origin Longitude</param>
        /// <param name="dlatitude">Destination Latitude</param>
        /// <param name="dlongitude">Destination Longitude</param>
        /// <param name="waypoints">way points</param>
        /// <param name="travelMode">Travel mode</param>
        /// <param name="alternatives"></param>
        public GoogleDirectionRequest(double olatitude, double olongitude, double dlatitude, double dlongitude, string waypoints, GoogleDirectionTravelMode travelMode, bool alternatives)
        {
            Olatitude = olatitude;
            Olongitude = olongitude;
            Dlatitude = dlatitude;
            Dlongitude = dlongitude;
            Waypoints = waypoints;
            TravelMode = travelMode;
            Alternatives = alternatives;
        }
    }
}