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
using System.IO.IsolatedStorage;
using System.Windows.Markup;
using System.Threading;
using DatingDiary.ViewModels;
using DatingDiary.Interfaces;
using Microsoft.Phone.Shell;
using DatingDiary.Singletons;

namespace DatingDiary.Views
{
    public partial class DatingGenderDecisionView : BasePage, IPage
	{
		public DatingGenderDecisionView()
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

		private void Boys_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;

			var settings = IsolatedStorageSettings.ApplicationSettings;
			settings["DatingGender"] = button.Content.ToString();
			settings.Save();
            AppContext.Instance.DatingGender = button.Content.ToString();

			NavigationService.Navigate(new Uri("/Views/Home.xaml", UriKind.Relative));
		}

		private void Girls_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;

			var settings = IsolatedStorageSettings.ApplicationSettings;
			settings["DatingGender"] = button.Content.ToString();
			settings.Save();
            AppContext.Instance.DatingGender = button.Content.ToString();

			NavigationService.Navigate(new Uri("/Views/Home.xaml", UriKind.Relative));
		}

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.DataContext = new DatingGenderDecisionViewModel(this);
            base.OnNavigatedTo(e);
        }
	}
}