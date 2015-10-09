using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using DatingDiary.Async;

namespace DatingDiary.ViewModels.Security
{
    public class RegisterViewModel : ViewModelBase
    {

        private string fEmail;
        public string Email
        {
            get { return fEmail; }
            set
            {
                fEmail = value;
                OnPropertyChanged("Email");
            }
        }

        private string fRetypedEmail;
        public string RetypedEmail
        {
            get { return fRetypedEmail; }
            set
            {
                fRetypedEmail = value;
                OnPropertyChanged("RetypedEmail");
            }
        }

        public RegisterViewModel(PhoneApplicationPage page) : base(page)
        {

        }



        public override string PageTitle
        {
            get
            {
                return string.Empty;
            }
        }
    }
}
