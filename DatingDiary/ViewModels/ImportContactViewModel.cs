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
using DatingDiary.Model;
using Microsoft.Phone.Controls;

namespace DatingDiary.ViewModels
{
	public class ImportContactViewModel : ViewModelBase
	{
        public ImportContactViewModel(PhoneApplicationPage page) : base(page) { }

		private Person fContact;
		public Person Contact
		{
			get { return fContact; }
			set 
			{ 
				fContact = value;
				OnPropertyChanged("Contact");
			}
		}

        public override string PageTitle
        {
            get
            {
                return "Import Contact";
            }
        }
		
	}
}
