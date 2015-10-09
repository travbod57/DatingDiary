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
using System.Device.Location;
using Microsoft.Phone.Controls;

namespace DatingDiary.ViewModels
{
	public class MapViewModel : ViewModelBase
	{
		public MapViewModel(GeoCoordinate geoCoordinate, PhoneApplicationPage page) : base (page)
		{
			this.GeoCoordinate = geoCoordinate;
		}

		private GeoCoordinate fGeoCoordinate;
		public GeoCoordinate GeoCoordinate
		{
			get { return fGeoCoordinate; }
			set 
			{ 
				fGeoCoordinate = value;
				OnPropertyChanged("GeoCoordinate");
			}
		}

        public override string PageTitle
        {
            get
            {
                return "Map";
            }
        }
		
	}
}
