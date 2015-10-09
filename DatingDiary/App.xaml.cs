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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using DatingDiary.Model;
using System.Data.Linq;
using DatingDiary.Enums;
using DatingDiary.Singletons;
using DatingDiary.UserControls;
using DatingDiary.Extensions;
using DatingDiary.Other;

namespace DatingDiary
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }
        public IsolatedStorageSettings isolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;
        //private double RootVisualWidth { get; private set; }
        //private double RootVisualHeight { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            TestData();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }


        private void TestData()
        {
            using (DatingAppContext db = new DatingAppContext(AppContext.Instance.ConnectionString))
            {
                if (db.DatabaseExists() == false)
                {
                    // Create the local database.
                    db.CreateDatabase();

                    Note n1 = new Note { CreatedDate = DateTime.Now, Content = "This is a note" };
                    Note n2 = new Note { CreatedDate = DateTime.Now, Content = "This is another note and this is extra text to push it onto another line but will it" };

                    db.Notes.InsertOnSubmit(n1);
                    db.Notes.InsertOnSubmit(n2);

                    Venue v1 = new Venue { Name = "Zizzi Chislehurst", Latitude = 51.417493, Longitude = 0.067863, IsFavourite = true };
                    Venue v2 = new Venue { Name = "TGI Friday New York", Latitude = 40.706681, Longitude = -74.013169 };
                    Venue v3 = new Venue { Name = "Bella Italia Leicester Square", Latitude = 51.510687, Longitude = -0.129290 };
                    Venue v4 = new Venue { Name = "Hyde Park", Latitude = 51.507432, Longitude = -0.165708 };
                    Venue v5 = new Venue { Name = "Sea Life Centre Brighton", Latitude = 50.819573, Longitude = -0.135351, IsFavourite = true };

                    db.Venues.InsertOnSubmit(v1);
                    db.Venues.InsertOnSubmit(v2);
                    db.Venues.InsertOnSubmit(v3);
                    db.Venues.InsertOnSubmit(v4);
                    db.Venues.InsertOnSubmit(v5);

                    Interest i1 = new Interest() { Description = "Football", Weighting = 3 };
                    Interest i2 = new Interest() { Description = "Tennis", Weighting = 2 };
                    Interest i3 = new Interest() { Description = "Ping Pong", Weighting = 1 };
                    Interest i4 = new Interest() { Description = "Hockey", Weighting = 1 };
                    Interest i5 = new Interest() { Description = "Squash", Weighting = 3 };
                    Interest i6 = new Interest() { Description = "Judo", Weighting = 2 };

                    db.Interests.InsertOnSubmit(i1);
                    db.Interests.InsertOnSubmit(i2);
                    db.Interests.InsertOnSubmit(i3);
                    db.Interests.InsertOnSubmit(i4);
                    db.Interests.InsertOnSubmit(i5);
                    db.Interests.InsertOnSubmit(i6);

                    Date d1 = new Date { Venue = v1, DateOfMeeting = DateTime.Now.AddMinutes(25), Rating = 2.5, Notes = new EntitySet<Note>() { n1, n2 } };
                    Date d2 = new Date { Venue = v2, DateOfMeeting = DateTime.Now.AddDays(26), Rating = 5, IsFavourite = true };
                    Date d3 = new Date { Venue = v3, DateOfMeeting = DateTime.Now.AddDays(1), Rating = 3, IsFavourite = true };
                    Date d4 = new Date { Venue = v4, DateOfMeeting = DateTime.Now.AddDays(2), Rating = 4, IsFavourite = true };
                    Date d5 = new Date { Venue = v5, DateOfMeeting = DateTime.Now.AddDays(5), Rating = 1 };
                    Date d6 = new Date { Venue = v5, DateOfMeeting = DateTime.Now.AddDays(25), Rating = 1 };
                    Date d7 = new Date { Venue = v5, DateOfMeeting = DateTime.Now.AddDays(14), Rating = 5 };

                    db.Dates.InsertOnSubmit(d1);
                    db.Dates.InsertOnSubmit(d2);
                    db.Dates.InsertOnSubmit(d3);
                    db.Dates.InsertOnSubmit(d4);
                    db.Dates.InsertOnSubmit(d5);
                    db.Dates.InsertOnSubmit(d6);
                    db.Dates.InsertOnSubmit(d7);

                    //Country c1 = new Country() { Name = "United Kingdom" };
                    //db.Countries.InsertOnSubmit(c1);

                    // Prepopulate the categories.		
                    db.Persons.InsertOnSubmit(new Person { FirstName = "Alex", SecondName = "Williams", PhoneNumber = "067878768", Email = "alexwilliams57@hotmail.com", IsFavourite = true, Dates = new EntitySet<Date>() { d1, d2, d6, d7 }, Interests = new EntitySet<Interest>() { i1, i2, i3, i4, i5, i6 } });
                    db.Persons.InsertOnSubmit(new Person { FirstName = "Rachel", SecondName = "Scott", PhoneNumber = "0345234545", Email = "rachel.c.scott@hotmail.com", IsFavourite = true, Dates = new EntitySet<Date>() { d3 } });
                    db.Persons.InsertOnSubmit(new Person { FirstName = "Iain", SecondName = "Smith", PhoneNumber = "032234324", Email = "iain-smith@hotmail.com", IsFavourite = true, Dates = new EntitySet<Date>() { d4 } });
                    db.Persons.InsertOnSubmit(new Person { FirstName = "John", SecondName = "Benson", PhoneNumber = "03454353", Email = "john.benson@hotmail.com", Dates = new EntitySet<Date>() { d5 } });
                    db.Persons.InsertOnSubmit(new Person { FirstName = "Peter", SecondName = "Parker", PhoneNumber = "034324324", Email = "peterpaker@hotmail.com" });
                    db.Persons.InsertOnSubmit(new Person { FirstName = "Amit", SecondName = "Dam", PhoneNumber = "0234324", Email = "amitdam@hotmail.com" });
                    db.Persons.InsertOnSubmit(new Person { FirstName = "Helen", SecondName = "Johnson", PhoneNumber = "04365435", Email = "helenjohnson@hotmail.com" });

                    //db.Persons.InsertOnSubmit(new Person { FirstName = "Alex", SecondName = "Williams", PhoneNumber = "067878768", Email = "alexwilliams57@hotmail.com", Country = c1, IsFavourite = true, Dates = new EntitySet<Date>() { d1, d2, d6, d7 }, Interests = new EntitySet<Interest>() { i1, i2, i3, i4, i5, i6 } });
                    //db.Persons.InsertOnSubmit(new Person { FirstName = "Rachel", SecondName = "Scott", PhoneNumber = "0345234545", Email = "rachel.c.scott@hotmail.com", Country = c1, IsFavourite = true, Dates = new EntitySet<Date>() { d3 } });
                    //db.Persons.InsertOnSubmit(new Person { FirstName = "Iain", SecondName = "Smith", PhoneNumber = "032234324", Email = "iain-smith@hotmail.com", Country = c1, IsFavourite = true, Dates = new EntitySet<Date>() { d4 } });
                    //db.Persons.InsertOnSubmit(new Person { FirstName = "John", SecondName = "Benson", PhoneNumber = "03454353", Email = "john.benson@hotmail.com", Country = c1, Dates = new EntitySet<Date>() { d5 } });
                    //db.Persons.InsertOnSubmit(new Person { FirstName = "Peter", SecondName = "Parker", PhoneNumber = "034324324", Email = "peterpaker@hotmail.com", Country = c1 });
                    //db.Persons.InsertOnSubmit(new Person { FirstName = "Amit", SecondName = "Dam", PhoneNumber = "0234324", Email = "amitdam@hotmail.com", Country = c1 });
                    //db.Persons.InsertOnSubmit(new Person { FirstName = "Helen", SecondName = "Johnson", PhoneNumber = "04365435", Email = "helenjohnson@hotmail.com", Country = c1 });


                    //db.Countries.InsertOnSubmit(new Country() { Name = "Albania" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Algeria" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Andorra" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Angola" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Argentina" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Armenia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Australia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Austria" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Bangladesh" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Barbados" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Belarus" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Belgium" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Belize" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Bolivia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Bosnia and Herzegovina" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Brazil" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Bulgaria" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Cambodia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Cameroon" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Canada" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Chile" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "China" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Colombia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Costa Rica" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Croatia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Cuba" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Cyprus" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Czech Republic" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Denmark" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Ecuador" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Egypt" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Estonia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Fiji" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Finland" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "France" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Georgia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Germany" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Ghana" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Greece" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Grenada" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Hungary" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Iceland" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "India" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Indonesia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Ireland" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Israel" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Italy" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Jamaica" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Japan" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Jordan" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Kenya" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Korea, South" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Kuwait" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Latvia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Lebanon" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Liberia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Liechtenstein" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Lithuania" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Luxembourg" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Macedonia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Malaysia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Malta" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Mauritius" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Mexico" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Moldova" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Monaco" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Mongolia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Montenegro" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Morocco" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Mozambique" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Myanmar (Burma)" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Netherlands" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "New Zealand" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Nigeria" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Norway" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Oman" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Pakistan" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Panama" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Paraguay" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Peru" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Philippines" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Poland" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Portugal" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Romania" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Russia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Saint Kitts and Nevis" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Saint Lucia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Saint Vincent and the Grenadines" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Samoa" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Senegal" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Serbia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Singapore" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Slovakia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Slovenia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "South Africa" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Spain" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Sri Lanka" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Sudan" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Sweden" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Switzerland" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Thailand" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Togo" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Tonga" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Trinidad and Tobago" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Tunisia" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Turkey" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Uganda" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Ukraine" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "United Arab Emirates" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "United States" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Uruguay" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Venezuela" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Vietnam" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Yemen" });
                    //db.Countries.InsertOnSubmit(new Country() { Name = "Zambia" });


                    // Save categories to the database.
                    db.SubmitChanges();
                }
            }
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            RootFrame.Style = Current.Resources["CustomFrame"] as Style;

            AppContext.Instance.AppLoadingStatus = AppLoadingStatus.Launching;
            AppContext.Instance.IsAppLocked = (bool)isolatedStorageSettings["IsPassCodeRequired"];
            AppContext.Instance.IsolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;
            AppContext.Instance.ConnectionString = @"isostore:/DatingDiaryDB.sdf";
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            RootFrame.Style = Current.Resources["CustomFrame"] as Style;

            AppContext.Instance.AppLoadingStatus = AppLoadingStatus.Launching;
            AppContext.Instance.IsAppLocked = (bool)isolatedStorageSettings["IsPassCodeRequired"];
            AppContext.Instance.IsolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;
            AppContext.Instance.ConnectionString = @"isostore:/DatingDiaryDB.sdf";
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            //EnterPassCodeUserControl uc = new EnterPassCodeUserControl(PhoneApplicationPage);
            //uc.UserControlLayoutRoot.Height = this.SubClassPage.GetLayoutRoot.ActualHeight + (PhoneApplicationPage.ApplicationBar != null ? PhoneApplicationPage.ApplicationBar.DefaultSize : 0);
            //uc.UserControlLayoutRoot.Width = App.Current.Host.Content.ActualWidth;
            //uc.Margin = new Thickness(0, Application.Current.Host.Content.ActualHeight - uc.UserControlLayoutRoot.Height, 0, 0);
            //PassCodePopup.Child = uc;
            //PassCodePopup.IsOpen = true;
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;
            RootFrame.Navigating += new NavigatingCancelEventHandler(RootFrame_Navigating);
        
            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;

            AppContext.Instance.AppLoadingStatus = AppLoadingStatus.Launching;
            AppContext.Instance.IsAppLocked = false;
            AppContext.Instance.IsolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;
            AppContext.Instance.ConnectionString = @"isostore:/DatingDiaryDB.sdf";

            isolatedStorageSettings["IsPassCodeRequired"] = false;
        }

        private void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            // do we need to display gender choice window for first load?
            if (!AppContext.Instance.IsolatedStorageSettings.Contains("DatingGender"))
            {
                if (e.Uri.ToString() == "//Views/Home.xaml")
                {
                    RootFrame.Dispatcher.BeginInvoke(() => RootFrame.Navigate(new Uri("/Views/DatingGenderDecisionView.xaml", UriKind.Relative)));
                    e.Cancel = true;
                }
            }
            else
                AppContext.Instance.DatingGender = AppContext.Instance.IsolatedStorageSettings["DatingGender"].ToString();
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}