using GoogleWebServiceApi.Models;

namespace GoogleWebServiceApi.Geocode.Model
{
	/// <summary>
	/// Represents a Google Geo code request parameter
	/// </summary>
	public class GoogleGeocodeRequest
	{

        /// <summary>
        /// Create a geocode request
        /// </summary>
        /// <param name="address"></param>
        /// <param name="sensor"></param>
		public GoogleGeocodeRequest(string address, bool sensor)
		{
			Address = address;
			Sensor = sensor;
		}


        /// <summary>
        /// Create a reverse geocode request
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="sensor"></param>
		public GoogleGeocodeRequest(double latitude, double longitude, bool sensor)
		{
		    Latitude = latitude;
		    Longitude = longitude;
			Sensor = sensor;
		}

        /// <summary>
        /// The address to geocode
        /// </summary>
		public string Address { get; set;}

        /// <summary>
        /// the Latitude to reverse geocode
        /// </summary>
		public double? Latitude { get; set; }

        /// <summary>
        /// the longitude to reverse geocode
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// The NE latitude bound of the Viewport biasing 
        /// Please refer to https://developers.google.com/maps/documentation/geocoding/#Viewports
        /// </summary>
		public double? BoundNorthEastLat { get; set; }

        /// <summary>
        /// The NE longitude bound of the viewport biasing
        /// Please refer to https://developers.google.com/maps/documentation/geocoding/#Viewports
        /// </summary>
        public double? BoundNorthEastLong { get; set; }

        /// <summary>
        /// The SW latitude bound of the viewport biasing
        /// https://developers.google.com/maps/documentation/geocoding/#Viewports
        /// </summary>
		public double? BoundSouthWestLat { get; set; }

        /// <summary>
        /// the SQ longitude bound of the viewport biasing
        /// https://developers.google.com/maps/documentation/geocoding/#Viewports
        /// </summary>
        public double? BoundSouthWestLong { get; set; }

        /// <summary>
        /// Language parameter
        /// </summary>
		public GoogleMapsLanguage? Language { get; set; }

		public bool Sensor{ get; set; }
		
		
	}
}

