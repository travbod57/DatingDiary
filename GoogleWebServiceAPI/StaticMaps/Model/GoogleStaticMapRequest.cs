using System;
using System.Device.Location;
using GoogleWebServiceApi.Models;

namespace GoogleWebServiceApi.StaticMaps.Model
{
	public class GoogleStaticMapRequest
	{

		public const string Request_Url =
		"http://maps.googleapis.com/maps/api/staticmap?key={0}&center={1}&size={2}&zoom={3}&sensor={4}";
		/// <summary>
		/// scale (optional) affects the number of pixels that are returned. scale=2 returns twice as many pixels as scale=1 while retaining the same coverage area and level of detail (i.e. the contents of the map don't change). This is useful when developing for high-resolution displays, or when generating a map for printing. The default value is 1. Accepted values are 2 
		/// </summary>
		public int? Scale { get; set; }

		/// <summary>
		/// language (optional) defines the language to use for display of labels on map tiles. Note that this parameter is only supported for some country tiles; if the specific language requested is not supported for the tile set, then the default language for that tileset will be used.
		/// </summary>
		public GoogleMapsLanguage Language { get; set; }
		/// <summary>
		/// Center of the map in Lat Long, cannot co-exist with Address
		/// </summary>
		public GeoCoordinate Location { get; set; }

		/// <summary>
		/// Address of the center of the map, cannot co-exist with Location
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// (required if markers not present) defines the zoom level of the map, which determines the magnification level of the map. This parameter takes a numerical value corresponding to the zoom level of the region desired.
		/// </summary>
		public int ZoomLevel { get; set; }

		/// <summary>
		/// Width of the image in pixels
		/// 
		public int SizeX { get; set; }

		/// <summary>
		/// Height of the image in Pixels
		/// </summary>
		public int SizeY { get; set; }

		/// <summary>
		/// Whether or not the device is using a GPS device for this request or not
		/// </summary>
		public bool Sensor { get; set; }

		public string AppId { get; set; }

		/// <summary>
		/// Image Format of the returned image
		/// </summary>
		public GoogleStaticMapImageFormat ImageFormat { get; set; }

		/// <summary>
		/// defines the type of map to construct. There are several possible maptype values, including roadmap, satellite, hybrid, and terrain
		/// Please refer to https://developers.google.com/maps/documentation/staticmaps/#MapTypes
		/// </summary>
		public GoogleStaticMapMapType MapType { get; set; }

		/// <summary>
		/// Markers to be added in the staic map
		/// </summary>
        public GoogleStaticMapMarkerStyleParameter Markers { get; set; }

		/// <summary>
		/// Path parameter, allows drawing a path on the static map
		/// Please refer to https://developers.google.com/maps/documentation/staticmaps/#Paths
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// Additional Styling of the returned static map
		/// Please refer to https://developers.google.com/maps/documentation/staticmaps/#StyledMaps
		/// </summary>
		public string Style { get; set; }

		

		public GoogleStaticMapRequest(string address, int zoomLevel, int sizeX, int sizeY, bool sensor, string appId)
		{
		    
			Address = Uri.EscapeDataString(address);
			ZoomLevel = zoomLevel;
			SizeX = sizeX;
			SizeY = sizeY;
			Sensor = sensor;
			AppId = appId;
		}

		public GoogleStaticMapRequest(GeoCoordinate location, int zoomLevel, int sizeX, int sizeY, bool sensor, string appId)
		{
			Location = location;
			ZoomLevel = zoomLevel;
			SizeX = sizeX;
			SizeY = sizeY;
			Sensor = sensor;
			AppId = appId;
		}


        /// <summary>
        /// generate the Request Url
        /// </summary>
        /// <returns></returns>
		public string GenerateRequestUrl()
		{
			var center = Address ?? (Location.Latitude + "," + Location.Longitude);
			var url = string.Format(Request_Url, AppId, center, SizeX + "x" + SizeY, ZoomLevel,Sensor.ToString().ToLower());
			if (Scale.HasValue) url = string.Concat(url, "&scale=", Scale);
			
			if (ImageFormat != null) url = string.Concat(url, "&format=", ImageFormat.Format);

			if (MapType != null) url = string.Concat(url, "&format=", MapType.MapType);

            if (Markers != null) url = string.Concat(url, Markers.BuildMarkerParameter());

			if (!string.IsNullOrEmpty(Path)) url = string.Concat(url, "&path=", Path);

			return url;
		}

	}
}
