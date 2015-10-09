using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DatingDiary.ViewModels;

namespace DatingDiary.Views
{
    public partial class Test : PhoneApplicationPage
    {
        public Test()
        {
            InitializeComponent();
            this.DataContext = new TestPageViewModel(this);
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