namespace GoogleWebServiceApi.StaticMaps.Model
{
	/// <summary>
	/// The Map Type of a Google static map request
	/// https://developers.google.com/maps/documentation/staticmaps/#MapTypes
	/// </summary>
	public class GoogleStaticMapMapType
	{

		public GoogleStaticMapMapType(string type)
		{
			MapType = type;
		}

		/// <summary>
		/// specifies a standard roadmap image, as is normally shown on the Google Maps website.
		/// </summary>
		public static string RoadMap { get { return "roadmap"; } }

		/// <summary>
		/// specifies a satellite image.
		/// </summary>
		public static string Sattelite { get { return "satellite"; } }

		/// <summary>
		/// specifies a physical relief map image, showing terrain and vegetation.
		/// </summary>
		public static string Terrain { get { return "terrain"; } }

		/// <summary>
		/// specifies a hybrid of the satellite and roadmap image, showing a transparent layer of major streets and place names on the satellite image.
		/// </summary>
		public static string Hybrid { get { return "hybrid"; } }
	
		
		public string MapType { get; set; }

	}
}
