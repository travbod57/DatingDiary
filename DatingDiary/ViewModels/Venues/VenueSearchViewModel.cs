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
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using DatingDiary.Other;
using DatingDiary.Repository;
using DatingDiary.Model;
using DatingDiary.Helpers;
using GoogleWebServiceApi.Places.Models;
using Microsoft.Phone.Controls;
using DatingDiary.Views;
using DatingDiary.Commands;
using System.Text;
using DatingDiary.Views.Venues;

namespace DatingDiary.ViewModels.Venues
{
	public class VenueSearchViewModel : ViewModelBase
	{

        public ICommand ViewOnMapCommand { get; private set; }

		public VenueSearchViewModel(PhoneApplicationPage page) : base (page)
		{
			this.Page = page;

            ViewOnMapCommand = new RelayCommand(DoViewOnMap, CanViewOnMap);
		}

        public void DoViewOnMap(object obj)
        {
            GooglePlaceSearchResult result = (GooglePlaceSearchResult)((LongListSelector)((VenueSearch)this.Page).ResultsListSelector).SelectedItem;

            if (result != null)
            {
                StringBuilder destination = new StringBuilder("/Views/MapView.xaml");
                destination.AppendFormat("?Latitude={0}&Longitude={1}", result.Geometry.Location.Latitude, result.Geometry.Location.Longitude);

                this.Page.NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
            }
        }

        public bool CanViewOnMap(object obj)
        {
            return true;
        }

        public override string PageTitle
        {
            get
            {
                return "Find Venue";
            }
        }
	}
}
