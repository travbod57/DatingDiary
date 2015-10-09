using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GoogleWebServiceApi.Places.Models;
using System.Windows.Media.Imaging;

namespace DatingDiary.GooglePlaces
{
	public class GoogleSearchResultWrapper : GooglePlaceSearchResult
	{
		//public GooglePlaceSearchResult GooglePlaceSearchResult { get; set; }
		//public string Name { get { return GooglePlaceSearchResult.Name; } }
		public BitmapImage Image { get; set; }
		//public string Address { get { return GooglePlaceSearchResult.Vicinity; } }
	}
}
