using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Device.Location;
using DatingDiary.ViewModels;
using DatingDiary.Views;
using DatingDiary.Interfaces;
using Microsoft.Phone.Shell;

namespace DatingDiary.Views
{
    public partial class MapView : BasePage, IPage
	{
		private double Latitude { get; set; }
		private double Longitude { get; set; }

		public MapView()
		{
			InitializeComponent();
            this.SubClassPage = this;
        }

        public Grid GetLayoutRoot
        {
            get
            {
                return this.LayoutRoot;
            }
        }

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			IDictionary<string, string> parameters = this.NavigationContext.QueryString;

			if (parameters.ContainsKey("Latitude"))
			{
				double tmpLatitude;
				Double.TryParse(parameters["Latitude"], out tmpLatitude);
				Latitude = tmpLatitude;
			}

			if (parameters.ContainsKey("Longitude"))
			{
				double tmpLongitude;
				Double.TryParse(parameters["Longitude"], out tmpLongitude);
				Longitude = tmpLongitude;
			}

			GeoCoordinate geocoordinate = new GeoCoordinate(Latitude, Longitude);

			this.DataContext = new MapViewModel(geocoordinate, this);
			map1.SetView(geocoordinate, 16);

			base.OnNavigatedTo(e);
		}
	}
}