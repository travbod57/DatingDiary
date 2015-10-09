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
	public class FavouritePeopleViewModel : ViewModelBase, IViewModel
	{


        private ObservableCollection<Person> fPeople;
        public ObservableCollection<Person> People
        {
            get { return fPeople; }
            set 
            { 
                fPeople = value;
                OnPropertyChanged("People");
            }
        }
        

        public FavouritePeopleViewModel(PhoneApplicationPage page) : base(page)
		{
            this.People = new ObservableCollection<Person>((from p in Ctx.Persons where p.IsFavourite == true orderby p.FirstName ascending select p).ToList());

            foreach (Person person in this.People)
                person.Image = Storage.ReadImageFromIsolatedStorageToWriteableBitmap(string.Format("ProfileThumbnail\\{0}", person.FileName));
		}

        public override string PageTitle
        {
            get
            {
                return "Favourite People";
            }
        }

	}
}
