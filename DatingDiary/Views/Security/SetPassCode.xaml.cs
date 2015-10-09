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
using System.Windows.Navigation;
using System.Text;
using System.IO.IsolatedStorage;
using DatingDiary.Interfaces;
using DatingDiary.Views;
using Microsoft.Phone.Shell;
using DatingDiary.ViewModels.Security;
using DatingDiary.Other;
using System.Windows.Controls.Primitives;
using DatingDiary.Behaviours;
using DatingDiary.Async;

namespace DatingDiary.Views.Security
{
    public partial class SetPassCode : BasePage, IPage
    {
        private string EnteredCode;
        private string ReTypedCode;
        private int DigitCount;
        private string Email;
        private IsolatedStorageSettings isolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;
        private SetPassCodeViewModel setPassCodeViewModel;
        private ProgressIndicator Pi;
        public Popup Popup = new Popup();

        public SetPassCode()
        {
            InitializeComponent();
            this.SubClassPage = this;

            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Pi = Microsoft.Phone.Shell.SystemTray.ProgressIndicator;
        }

        public Grid GetLayoutRoot
        {
            get
            {
                return this.LayoutRoot;
            }
        }

        private void Number_Button_Click(object sender, RoutedEventArgs e)
        {
            DigitCount++;

            if (DigitCount <= 4)
            {
                EnteredCode = string.Format("{0}{1}", EnteredCode, ((Button)sender).Content);
                this.txtPassCode.Text = string.Format("{0}{1}", this.txtPassCode.Text, "*");
            }
            else
            {
                ReTypedCode = string.Format("{0}{1}", ReTypedCode, ((Button)sender).Content);
                this.txtRetype.Text = string.Format("{0}{1}", this.txtRetype.Text, "*");
            }
        }

        private void ClearFields()
        {
            DigitCount = 0;

            EnteredCode = string.Empty;
            ReTypedCode = string.Empty;

            this.txtPassCode.Text = EnteredCode;
            this.txtRetype.Text = ReTypedCode;

            isolatedStorageSettings["IsPassCodeRequired"] = false;
        }

        private void Word_Button_Click(object sender, RoutedEventArgs e)
        {
            string text = ((Button)sender).Content.ToString();

            if (text == "Clear")
                ClearFields();
            else if (text == "Enter")
            {
                if (String.IsNullOrEmpty(EnteredCode) || String.IsNullOrEmpty(ReTypedCode))
                {
                    MessageBox.Show("You must enter the same pin twice");
                    ClearFields();
                }
                else if (EnteredCode.Length != 4 && ReTypedCode.Length != 4)
                {
                    MessageBox.Show("The pin must be 4 characters long");
                    ClearFields();
                }
                else if (EnteredCode == ReTypedCode)
                {
                    Storage.WriteTextToIsolatedStorageFile("PassCode.txt", EnteredCode);
                    isolatedStorageSettings["IsPassCodeRequired"] = true;
                    isolatedStorageSettings.Save();

                    setPassCodeViewModel.RegisterCodeForRecovery(Email, Pi);

                    //if (ars.Success)
                    //{
                    //    MessageBox.Show(ars.Message, "Information", MessageBoxButton.OK);
                    //    NavigationService.Navigate(new Uri("/Views/SettingsView.xaml?PassCodeSet=True", UriKind.Relative));
                    //}
                    //else
                    //    MessageBox.Show(ars.Message, "Error", MessageBoxButton.OK);
         
                }
                else
                {
                    MessageBox.Show("The pins you entered do not match");
                    ClearFields();
                }
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            IDictionary<string, string> parameters = this.NavigationContext.QueryString;
  
            if (parameters.ContainsKey("Email"))
                Email = parameters["Email"];

            setPassCodeViewModel = new SetPassCodeViewModel(this);
            this.DataContext = setPassCodeViewModel;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
                this.Popup.IsOpen = false;

            base.OnNavigatedFrom(e);
        }


    }
}