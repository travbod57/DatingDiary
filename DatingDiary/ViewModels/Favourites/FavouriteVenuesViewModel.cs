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
	public class FavouriteVenuesViewModel : ViewModelBase, IViewModel
	{


        private ObservableCollection<Venue> fVenues;
        public ObservableCollection<Venue> Venues
        {
            get { return fVenues; }
            set 
            {
                fVenues = value;
                OnPropertyChanged("Venues");
            }
        }


        public FavouriteVenuesViewModel(PhoneApplicationPage page) : base(page)
		{
            this.Venues = new ObservableCollection<Venue>((from v in Ctx.Venues where v.IsFavourite == true orderby v.Name ascending select v).ToList());
		}

        public override string PageTitle
        {
            get
            {
                return "Favourite Venues";
            }
        }

	}
}
