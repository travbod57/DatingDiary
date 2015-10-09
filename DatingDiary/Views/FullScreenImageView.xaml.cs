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
using DatingDiary.Helpers;
using System.Windows.Media.Imaging;
using System.Windows.Markup;
using System.Threading;
using DatingDiary.Interfaces;
using Microsoft.Phone.Shell;
using DatingDiary.ViewModels;

namespace DatingDiary.Views
{
    public partial class FullScreenImageView : BasePage, IPage
	{
		public int PhotoId { get; set; }

		public FullScreenImageView()
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

			if (parameters.ContainsKey("PhotoId"))
			{
				PhotoId = Int32.Parse(parameters["PhotoId"]);
			}

			this.DataContext = new FullScreenImageViewModel(PhotoId, this);

			base.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
		{
			((FullScreenImageViewModel)this.DataContext).DisposeContext();
			base.OnNavigatedFrom(e);
		}

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            //MessageBox.Show(string.Format("width: {0}, heigh: {1}", img.Width, img.Height));
            //MessageBox.Show(string.Format("Margin: {0}", img.Margin.Left));
        }

        private void ApplicationBar_StateChanged_1(object sender, ApplicationBarStateChangedEventArgs e)
        {
            ApplicationBar bar = (ApplicationBar)sender;

            if (e.IsMenuVisible)
                bar.Opacity = 1;
            else
                bar.Opacity = 0;
        }
	}
}