using System.Collections.Generic;
using GoogleWebServiceApi.Models;
using GoogleWebServiceAPI.Places;

namespace GoogleWebServiceApi.Places.Models
{

	public class GooglePlaceSearchRequest
	{
        public GooglePlaceSearchRequest(bool sensor, string keyword)
        {
            Sensor = sensor;
            Keyword = keyword;
        }

		public GooglePlaceSearchRequest(double originLat, double originLong, double radius, bool sensor, string keyword)
		{
			LocationLat = originLat;
			LocationLong = originLong;
			Radius = radius;
			Sensor = sensor;
			Keyword = keyword;
			Language = null;
			Name = string.Empty;
			Types = null;
		}

		public double LocationLat { get;  set; }
		public double LocationLong { get;  set; }
		public double Radius { get;  set; }
		public bool Sensor { get;  set; }
		public string Keyword { get;  set; }
		public GoogleMapsLanguage? Language { get;  set; }
		public string Name { get;  set; }
		public IList<GooglePlaceSearchType> Types { get;  set; }


	}
}