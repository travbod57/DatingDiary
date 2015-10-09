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
using DatingDiary.Other;
using DatingDiary.Model;
using DatingDiary.Helpers;
using DatingDiary.Repository;
using DatingDiary.Interfaces;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Controls;
using DatingDiary.ViewModels;

namespace DatingDiary.ViewModels
{
	public class DatingGenderDecisionViewModel : ViewModelBase, IViewModel
	{
        public DatingGenderDecisionViewModel(PhoneApplicationPage page) : base(page)
		{
		}

        public override string PageTitle
        {
            get
            {
                return "Boys or Girls?";
            }
        }        
	}
}
