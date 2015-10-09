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
using DatingDiary.Interfaces;
using DatingDiary.ViewModels.Venues;
using System.Text;
using DatingDiary.Model;
using Microsoft.Phone.Shell;

namespace DatingDiary.Views.Venues
{
    public partial class VenueView : BasePage, IPage
    {
        private VenueViewModel venueViewModel { get; set; }
        private int VenueId { get; set; }

        public VenueView()
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

            if (parameters.ContainsKey("VenueId"))
                VenueId = Int32.Parse(parameters["VenueId"]);

            venueViewModel = new VenueViewModel(this, VenueId);
            this.DataContext = venueViewModel;

            base.OnNavigatedTo(e);
        }

        private void PeopleListPicker_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StringBuilder destination = new StringBuilder("/Views/Dates/DateView.xaml");
            destination.AppendFormat("?DateId={0}", ((Date)((LongListSelector)sender).SelectedItem).Id);

            NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
        }
    }
}