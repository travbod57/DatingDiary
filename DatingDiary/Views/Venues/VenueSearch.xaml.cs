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
using DatingDiary.Google;
using GoogleWebServiceApi.Places.Models;
using DatingDiary.GooglePlaces;
using System.Windows.Media.Imaging;
using System.Windows.Markup;
using System.Threading;
using System.Device.Location;
using System.Text;
using DatingDiary.ViewModels;
using System.Xml.Serialization;
using DatingDiary.Model;
using DatingDiary.Interfaces;
using DatingDiary.ViewModels.Venues;
using Microsoft.Phone.Shell;

namespace DatingDiary.Views.Venues
{
    public partial class VenueSearch : BasePage, IPage
	{

		private VenueSearchViewModel venueSearchViewModel { get; set; }
        private string SourcePage { get; set; }

		public VenueSearch()
		{
			InitializeComponent();
            this.SubClassPage = this;

			EmptyMessage.Visibility = Visibility.Collapsed;
            
        }

        public Grid GetLayoutRoot
        {
            get
            {
                return this.LayoutRoot;
            }
        }

		private void Button_Click(object sender, RoutedEventArgs e)
		{


			//GooglePlacesClient gpc = new GooglePlacesClient("AIzaSyBF9QNL1u5bGOvMngxDyAV4smnXi0jwdbw");
			//gpc.SearchForPlaceAync(new GooglePlaceSearchRequest(51.417054, 0.069158, 50000, false, txtSearch.Text));

            GooglePlacesTextSearchClient gpc = new GooglePlacesTextSearchClient("AIzaSyBF9QNL1u5bGOvMngxDyAV4smnXi0jwdbw");
            gpc.SearchForPlaceAync(new GooglePlaceSearchRequest(false, txtSearch.Text));

            gpc.SearchForPlacesAsyncCompleted += GooglePlacesSearchForPlaceDelegate;
            gpc.SearchForPlacesAsyncNotFound += SearchForPlacesAsyncNotFound;

		}

		public void GooglePlacesSearchForPlaceDelegate(GooglePlaceSearchResponse response, GooglePlaceSearchRequest request)
		{
			Dispatcher.BeginInvoke(() => {

                ResultsListSelector.ItemsSource = response.Results;

				EmptyMessage.Visibility = Visibility.Collapsed;
                ResultsListSelector.Visibility = Visibility.Visible;
			});
		}

		public void SearchForPlacesAsyncNotFound(GooglePlaceSearchResponse response, GooglePlaceSearchRequest request)
		{
			Dispatcher.BeginInvoke(() =>
			{
                ResultsListSelector.ItemsSource = response.Results;

				EmptyMessage.Visibility = Visibility.Visible;
                ResultsListSelector.Visibility = Visibility.Collapsed;
			});
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
            IDictionary<string, string> parameters = this.NavigationContext.QueryString;

            if (parameters.ContainsKey("SourcePage"))
            {
                SourcePage = parameters["SourcePage"];
            }

			if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
			{
				venueSearchViewModel = new VenueSearchViewModel(this);
				this.DataContext = venueSearchViewModel;
			}

			base.OnNavigatedTo(e);
		}

        private void CreateDate_Click(object sender, EventArgs e)
        {
            GooglePlaceSearchResult result = (GooglePlaceSearchResult)((LongListSelector)this.ResultsListSelector).SelectedItem;

            if (result != null)
            {
                Venue venue = new Venue() { Name = result.Name, Latitude = result.Geometry.Location.Latitude, Longitude = result.Geometry.Location.Longitude };

                XmlSerializer ser = new XmlSerializer(typeof(Venue));
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                System.IO.StringWriter writer = new System.IO.StringWriter(sb);
                ser.Serialize(writer, venue);

                StringBuilder destination = new StringBuilder("/Views/Dates/CEDateView.xaml");
                destination.AppendFormat("?Venue={0}", sb);

                NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
            }
        }

        private void ViewMap_Click(object sender, EventArgs e)
        {
            GooglePlaceSearchResult result = (GooglePlaceSearchResult)((LongListSelector)this.ResultsListSelector).SelectedItem;

            if (result != null)
            {
                StringBuilder destination = new StringBuilder("/Views/MapView.xaml");
                destination.AppendFormat("?Latitude={0}&Longitude={1}", result.Geometry.Location.Latitude, result.Geometry.Location.Longitude);

                NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
            }
        }

        private void Search_ActionIconTapped(object sender, EventArgs e)
        {
            GooglePlacesTextSearchClient gpc = new GooglePlacesTextSearchClient("AIzaSyBF9QNL1u5bGOvMngxDyAV4smnXi0jwdbw");
            gpc.SearchForPlaceAync(new GooglePlaceSearchRequest(false, txtSearch.Text));

            gpc.SearchForPlacesAsyncCompleted += GooglePlacesSearchForPlaceDelegate;
            gpc.SearchForPlacesAsyncNotFound += SearchForPlacesAsyncNotFound;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("click");
        }

        private void Use_Click(object sender, EventArgs e)
        {
            GooglePlaceSearchResult result = (GooglePlaceSearchResult)((LongListSelector)this.ResultsListSelector).SelectedItem;

            if (result != null)
            {
                Venue venue = new Venue() { Name = result.Name, Latitude = result.Geometry.Location.Latitude, Longitude = result.Geometry.Location.Longitude };

                XmlSerializer ser = new XmlSerializer(typeof(Venue));
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                System.IO.StringWriter writer = new System.IO.StringWriter(sb);
                ser.Serialize(writer, venue);

                StringBuilder destination = new StringBuilder("/Views/Dates/CEDateView.xaml");
                destination.AppendFormat("?Venue={0}", sb);

                NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
            }
        }
	}
}