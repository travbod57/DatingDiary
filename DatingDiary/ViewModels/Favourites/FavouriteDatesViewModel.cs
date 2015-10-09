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
using DatingDiary.Helpers;
using System.Windows.Media.Imaging;
using DatingDiary.Model;
using DatingDiary.Repository;
using Microsoft.Phone.Controls;
using DatingDiary.Views;
using DatingDiary.Interfaces;
using System.Collections.Generic;

namespace DatingDiary.ViewModels.Favourites
{
	public class FavouriteDatesViewModel : ViewModelBase, IViewModel
	{


        private ObservableCollection<Date> fDates;
        public ObservableCollection<Date> Dates
        {
            get { return fDates; }
            set 
            {
                fDates = value;
                OnPropertyChanged("Dates");
            }
        }


        public FavouriteDatesViewModel(PhoneApplicationPage page) : base(page)
		{
            this.Dates = new ObservableCollection<Date>((from d in Ctx.Dates where d.IsFavourite == true orderby d.DateOfMeeting ascending select d).ToList());
		}

        public override string PageTitle
        {
            get
            {
                return "Favourite Dates";
            }
        }

	}
}
