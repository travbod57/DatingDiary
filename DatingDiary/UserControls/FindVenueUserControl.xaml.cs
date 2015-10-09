using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DatingDiary.Google;
using GoogleWebServiceApi.Places.Models;
using DatingDiary.ViewModels.Dates;
using DatingDiary.Model;
using DatingDiary.Services;
using DatingDiary.Views.Dates;


namespace DatingDiary.UserControls
{
    public partial class FindVenueUserControl : UserControl
    {
        private CEDateView _ceDateView;
        private CEDateViewModel _ceDateViewModel;
        private DialogService _dialogService;
        private ProgressIndicator _progressIndicator;
        private GooglePlacesTextSearchClient _gpc;

        public FindVenueUserControl(CEDateView ceDateView, DialogService dialogService, ProgressIndicator progressIndicator)
        {
            InitializeComponent();
            _ceDateView = ceDateView;
            _ceDateViewModel = (CEDateViewModel)_ceDateView.DataContext;
            _dialogService = dialogService;
            _progressIndicator = progressIndicator;
        }


        private void GooglePlacesSearchForPlaceDelegate(GooglePlaceSearchResponse response, GooglePlaceSearchRequest request)
        {
            Dispatcher.BeginInvoke(() =>
            {
                btnCancel.Visibility = Visibility.Collapsed;
                btnSearch.Visibility = Visibility.Visible;

                _progressIndicator.IsVisible = false;

                ResultsListSelector.ItemsSource = response.Results;

                EmptyMessage.Visibility = Visibility.Collapsed;
                ResultsListSelector.Visibility = Visibility.Visible;
            });
        }

        private void SearchForPlacesAsyncNotFound(GooglePlaceSearchResponse response, GooglePlaceSearchRequest request)
        {
            Dispatcher.BeginInvoke(() =>
            {

                btnCancel.Visibility = Visibility.Collapsed;
                btnSearch.Visibility = Visibility.Visible;

                _progressIndicator.IsVisible = false;

                EmptyMessage.Visibility = Visibility.Visible;
                ResultsListSelector.Visibility = Visibility.Collapsed;
            });
        }

        private void ResultsListSelector_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            
            GooglePlaceSearchResult result = (GooglePlaceSearchResult)((LongListSelector)this.ResultsListSelector).SelectedItem;

            if (result != null)
            {
                _ceDateView.txtVenue.IsEnabled = true;
                _ceDateViewModel.TheDate.Venue = new Venue() { Name = result.Name, Latitude = result.Geometry.Location.Latitude, Longitude = result.Geometry.Location.Longitude };
                _dialogService.Hide(false);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            btnCancel.Visibility = Visibility.Collapsed;
            btnSearch.Visibility = Visibility.Visible;
            _progressIndicator.IsVisible = false;

            _gpc.CancelWebRequest();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            ResultsListSelector.ItemsSource = null;

            if (String.IsNullOrEmpty(txtSearch.Text))
                MessageBox.Show("Please enter a search term");
            else
            {
                btnCancel.Visibility = Visibility.Visible;
                btnSearch.Visibility = Visibility.Collapsed;

                EmptyMessage.Visibility = Visibility.Collapsed;
                _progressIndicator.IsVisible = true;
                //ProgressBar.IsIndeterminate = true;
                //GooglePlacesClient gpc = new GooglePlacesClient("AIzaSyBF9QNL1u5bGOvMngxDyAV4smnXi0jwdbw");
                //gpc.SearchForPlaceAync(new GooglePlaceSearchRequest(51.417054, 0.069158, 50000, false, txtSearch.Text));

                _gpc = new GooglePlacesTextSearchClient("AIzaSyBF9QNL1u5bGOvMngxDyAV4smnXi0jwdbw");
                _gpc.SearchForPlaceAync(new GooglePlaceSearchRequest(false, txtSearch.Text));

                _gpc.SearchForPlacesAsyncCompleted += GooglePlacesSearchForPlaceDelegate;
                _gpc.SearchForPlacesAsyncNotFound += SearchForPlacesAsyncNotFound;
            }
        }
    }
}
