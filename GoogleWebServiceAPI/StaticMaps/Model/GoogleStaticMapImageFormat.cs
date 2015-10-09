namespace GoogleWebServiceApi.StaticMaps.Model
{

	/// <summary>
	/// Image format used in Google Static Maps
	/// https://developers.google.com/maps/documentation/staticmaps/#ImageFormats
	/// </summary>
	public class GoogleStaticMapImageFormat
	{
		public static string Png { get { return "png"; } }
		public static string Png32 { get { return "png32"; } }
		public static string Jpg { get { return "jpg"; } }
		public static string JpgBaseLine { get { return "jpg-baseline"; } }

		public string Format { get; set; }
	}
}