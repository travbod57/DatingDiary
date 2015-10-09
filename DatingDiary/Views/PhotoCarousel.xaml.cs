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
using DatingDiary.ViewModels;
using DatingDiary.Views;
using DatingDiary.Interfaces;
using Microsoft.Phone.Shell;

namespace DatingDiary.Views
{
    public partial class PhotoCarousel : BasePage, IPage
    {
        private PhotoCarouselViewModel photoCarouselViewModel;
        private int DateId;
        private int PhotoId;

        public PhotoCarousel()
        {
            InitializeComponent();
            this.SubClassPage = this;

            //uc.Margin = new Thickness(0, Application.Current.Host.Content.ActualHeight - uc.UserControlLayoutRoot.Height, 0, 0);
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

            if (parameters.ContainsKey("DateId"))
                DateId = Int32.Parse(parameters["DateId"]);

            if (parameters.ContainsKey("PhotoId"))
                PhotoId = Int32.Parse(parameters["PhotoId"]);

            photoCarouselViewModel = new PhotoCarouselViewModel(DateId, PhotoId, this);
            this.DataContext = photoCarouselViewModel;




            //this.PivotControl.SelectedIndex = 1;

            base.OnNavigatedTo(e);
        }

        private void ApplicationBar_StateChanged(object sender, ApplicationBarStateChangedEventArgs e)
        {
            ApplicationBar bar = (ApplicationBar)sender;

            if (e.IsMenuVisible)
                bar.Opacity = 1;
            else
                bar.Opacity = 0;

        }


    }
}