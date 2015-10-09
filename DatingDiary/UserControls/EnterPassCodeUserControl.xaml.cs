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
using DatingDiary.Helpers;
using Microsoft.Phone.Controls;
using DatingDiary.Interfaces;
using DatingDiary.Views;
using DatingDiary.Singletons;

namespace DatingDiary.UserControls
{
    public partial class EnterPassCodeUserControl : UserControl
    {
        private string EnteredCode;
        private string CorrectCode;
        private PhoneApplicationPage Page;


        public EnterPassCodeUserControl(PhoneApplicationPage page)
        {
            InitializeComponent();
            CorrectCode = Storage.ReadTextFromIsolatedStorageFile("PassCode.txt");
            Page = page;
        }

        private void Number_Button_Click(object sender, RoutedEventArgs e)
        {
            EnteredCode = string.Format("{0}{1}", EnteredCode, ((Button)sender).Content);
            this.txtPassCode.Text = string.Format("{0}{1}", this.txtPassCode.Text, "*");
        }

        private void Word_Button_Click(object sender, RoutedEventArgs e)
        {
            string text = ((Button)sender).Content.ToString();

            if (text == "Clear")
                Clear();
            else if (text == "Enter")
            {
                if (String.IsNullOrEmpty(EnteredCode))
                    MessageBox.Show("You must enter a pin");
                else if ((EnteredCode != CorrectCode))
                {
                    MessageBox.Show("The pin you entered is not valid");
                    Clear();
                }
                else if (EnteredCode == CorrectCode)
                {
                    AppContext.Instance.IsAppLocked = false;
                    ((BasePage)this.Page).ClosePopup();
                }
            }
        }

        private void Clear()
        {
            EnteredCode = string.Empty;
            this.txtPassCode.Text = EnteredCode;
        }
    }
}
