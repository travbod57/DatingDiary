using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text;
using DatingDiary.Model;
using DatingDiary.Behaviours;
using DatingDiary.ViewModels;
using System.Globalization;
using System.Threading;
using DatingDiary.Animations;
using DatingDiary.Interfaces;
using DatingDiary.Other;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using DatingDiary.UserControls;
using DatingDiary.Extensions;
using DatingDiary.Singletons;

namespace DatingDiary.Views
{
    public partial class Home : BasePage, IPage
    {
        private IBackStackBehavior IBackStackBehaviour { get; set; }
        private HomeViewModel homeViewModel { get; set; }
        BackgroundWorker backroungWorker;
        Popup myPopup;

        public Home()
        {
            CultureInfo cult = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentCulture = cult;

            InitializeComponent();

            myPopup = new Popup() { IsOpen = true, Child = new AnimatedSplashScreen() };
            backroungWorker = new BackgroundWorker();
            RunBackgroundWorker();

            this.SubClassPage = this;
            AnimationContext = LayoutRoot;
        }


        private void RunBackgroundWorker()
        {
            backroungWorker.DoWork += ((s, args) =>
            {
              
            });

            backroungWorker.RunWorkerCompleted += ((s, args) =>
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    
                    this.myPopup.IsOpen = false;
                    
                }
            );
            });
            backroungWorker.RunWorkerAsync();
        }

        #region IPage
        public Grid GetLayoutRoot
        {
            get
            {
                return this.LayoutRoot;
            }
        } 
        #endregion

        #region Entity Taps
        private void Date_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StringBuilder destination = new StringBuilder("/Views/Dates/DateView.xaml");

            object item = ((LongListSelector)sender).SelectedItem;

            if (item != null)
            {
                destination.AppendFormat("?DateId={0}", ((Date)((LongListSelector)sender).SelectedItem).Id);
                NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
            }
        }

        private void Person_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StringBuilder destination = new StringBuilder("/Views/People/PersonView.xaml");

            object item = ((LongListSelector)sender).SelectedItem;

            if (item != null)
            {
                destination.AppendFormat("?PersonId={0}", ((Person)((LongListSelector)sender).SelectedItem).Id);
                NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
            }
        }

        private void HubTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            switch (((HubTile)sender).Title)
            {
                case "People":
                    NavigationService.Navigate(new Uri("/Views/Favourites/FavouritePeopleView.xaml", UriKind.Relative));
                    break;
                case "Dates":
                    NavigationService.Navigate(new Uri("/Views/Favourites/FavouriteDatesView.xaml", UriKind.Relative));
                    break;
                case "Venues":
                    NavigationService.Navigate(new Uri("/Views/Favourites/FavouriteVenuesView.xaml", UriKind.Relative));
                    break;
            }

        } 
        #endregion

        #region Page Events
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    ApplicationBar = ((ApplicationBar)this.Resources["HomeView_Upcoming_AppBar"]);
                    break;

                case 1:
                    ApplicationBar = ((ApplicationBar)this.Resources["HomeView_Person_AppBar"]);
                    IApplicationBarIconButton addPersonButton = (IApplicationBarIconButton)ApplicationBar.Buttons[0];
                    addPersonButton.Text = string.Format("Add {0}", AppContext.Instance.DatingGender);
                    break;
                case 2:
                    ApplicationBar = null;
                    break;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            IBackStackBehaviour = new RemoveFromBackStackBehaviour();
            IBackStackBehaviour.RemoveEntry(this, new List<string>() { "AddPersonView" });

            homeViewModel = new HomeViewModel(this);
            this.DataContext = homeViewModel;

            LongListUpcoming.SelectedItem = null;

            base.OnNavigatedTo(e);
        }

        #endregion

        #region Menu Item Events
        private void HomeView_Person_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/People/AddPersonView.xaml", UriKind.Relative));
            //NavigationService.Navigate(new Uri("/Views/TestPage.xaml", UriKind.Relative));
        }

        private void HomeView_Upcoming_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Dates/CEDateView.xaml", UriKind.Relative));
        }

        private void HomeView_Settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SettingsView.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Security/EnterPassCode.xaml", UriKind.Relative));
        } 
        #endregion

    }
}