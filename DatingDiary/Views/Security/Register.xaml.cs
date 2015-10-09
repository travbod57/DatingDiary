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
using System.Windows.Data;
using DatingDiary.ViewModels.Security;
using System.Text.RegularExpressions;

namespace DatingDiary.Views.Security
{
    public partial class Register : BasePage, IPage
    {
        private RegisterViewModel registerViewModel { get; set; }

        public Register()
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
            registerViewModel = new RegisterViewModel(this);
            this.DataContext = registerViewModel;
        }

        private void Next_Click(object sender, EventArgs e)
        {
            bool canContinue = true;

            if (String.IsNullOrEmpty(registerViewModel.Email) || String.IsNullOrEmpty(registerViewModel.RetypedEmail))
            {
                MessageBox.Show("You must enter the same email address twice");
                ClearFields();
                canContinue = false;
            }
            else if (registerViewModel.Email != registerViewModel.RetypedEmail)
            {
                MessageBox.Show("The email addresses you entered do not match");
                ClearFields();
                canContinue = false;
            }
            else
            {
                string email = registerViewModel.Email;
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);

                if (!match.Success)
                    canContinue = false;
            }

            if (canContinue)
            {
                StringBuilder destination = new StringBuilder("/Views/Security/SetPassCode.xaml");
                destination.AppendFormat("?Email={0}", registerViewModel.Email);

                NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
            }
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindingExpression be = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource();
        }

        private void ClearFields()
        {
            registerViewModel.Email = string.Empty;
            registerViewModel.RetypedEmail = string.Empty;
        }


    }
}