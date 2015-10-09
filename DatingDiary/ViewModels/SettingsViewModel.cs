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
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using DatingDiary.Model;
using DatingDiary.Helpers;
using DatingDiary.Repository;
using DatingDiary.Interfaces;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Controls;
using DatingDiary.Singletons;

namespace DatingDiary.ViewModels
{
	public class SettingsViewModel : ViewModelBase, IViewModel
	{
        public SettingsViewModel(PhoneApplicationPage page) : base (page)
		{
		}

        private bool fIsPassCodeRequired;
        public bool IsPassCodeRequired
        {
            get { return (bool)AppContext.Instance.IsolatedStorageSettings["IsPassCodeRequired"]; }
            set { fIsPassCodeRequired = value; }
        }

        public override string PageTitle
        {
            get
            {
                return "Settings";
            }
        }
        
	}
}
