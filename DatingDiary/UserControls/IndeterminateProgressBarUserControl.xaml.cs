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
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;

namespace DatingDiary.UserControls
{
    public partial class IndeterminateProgressBarUserControl : UserControl
    {
        public Popup Popup { get; set; }

        public IndeterminateProgressBarUserControl(Popup popup)
        {
            InitializeComponent();
            this.Popup = popup;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Popup.IsOpen = false;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/SettingsView.xaml", UriKind.Relative));
        }
    }
}
