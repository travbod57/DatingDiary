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
using DatingDiary.Repository;
using DatingDiary.Model;
using DatingDiary.Helpers;
using DatingDiary.Interfaces;
using Microsoft.Phone.Controls;
using System.IO;
using DatingDiary.Singletons;

namespace DatingDiary.ViewModels
{
    public class HomeViewModel : ViewModelBase
	{
		public string DatingGender
		{
			get
			{
                return AppContext.Instance.DatingGender;
			}
		}

		public Visibility ShowDatesLongListPicker
		{
			get { return this.Dates.Count > 0 ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowDatesEmptyMessage
		{
			get { return this.Dates.Count == 0 ? Visibility.Visible : Visibility.Collapsed; }
		}

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

        private List<Grouping<char, Person>> fGroupedPersonList;
        public List<Grouping<char, Person>> GroupedPersonList
        {
            get { return fGroupedPersonList; }
            set { fGroupedPersonList = value; }
        }

		private List<Grouping<string, Date>> fGroupedDateList;
		public List<Grouping<string, Date>> GroupedDateList
		{
			get { return fGroupedDateList; }
			set { fGroupedDateList = value; }
		}

        //private ObservableCollection<RankedVenue> fVenues;
        //public ObservableCollection<RankedVenue> Venues
        //{
        //    get { return fVenues; }
        //    set
        //    {
        //        fVenues = value;
        //        OnPropertyChanged("Venues");
        //    }
        //}

        public ObservableCollection<Venue> Venues { get; set; }
        //public Favourite Favourites { get; set; }
        //public List<EntityBase> MoreFavourites { get; set; }
        public List<TileItem> TileItems { get; set; }

        //public List<Grouping<string, EntityBase>> GroupedFavouritesList { get; set; }

        public override string PageTitle
        {
            get { return "At a Glance"; }
        }
        

		public HomeViewModel(PhoneApplicationPage page) : base(page)
		{
			Repository<Person> repPerson = new Repository<Person>(Ctx);
			Repository<Date> repDate = new Repository<Date>(Ctx);
			Repository<Venue> repVenue = new Repository<Venue>(Ctx);

			this.People = new ObservableCollection<Person>(repPerson.GetAll());
			this.Dates = new ObservableCollection<Date>(repDate.GetAll());
			this.Venues = new ObservableCollection<Venue>(repVenue.GetAll());

            this.TileItems = new List<TileItem>() { 
                new TileItem() { Title = "People", Message = ResolveTileInfo(this.People.Count( x => x.IsFavourite == true)), ImageUri = "/Images/PeopleCollage.jpg" }, 
                new TileItem() { Title = "Dates", Message = ResolveTileInfo(this.Dates.Count( x => x.IsFavourite == true)), ImageUri = "/Images/WineGlassTmp.jpg" }, 
                new TileItem() { Title = "Venues", Message = ResolveTileInfo(this.Venues.Count( x => x.IsFavourite == true)), ImageUri = "/Images/VenuesTmp.jpg" }};


            foreach (Date date in this.Dates.Where(x => x.DateOfMeeting.Date >= DateTime.Now.Date).Take(10))
                ChronoGrouping.MarkDate(date);

            foreach (Person person in this.People)
            {
                person.Image = Storage.ReadImageFromIsolatedStorageToWriteableBitmap(string.Format("ProfileThumbnail\\{0}", person.FileName));

                //if (!String.IsNullOrEmpty(person.FileName))
                //    Storage.AsyncImageLoad(person.FileName, person);
                //Stream stream = Storage.ReadImageFromIsolatedStorageAsStream(person.FileName);
                //person.Image = stream == null ? Storage.ReadImageFromContent("./Images/LittleDude.jpg") : GeneralMethods.ResizeImageToThumbnail(stream, 150);
            }
                //person.Image = GeneralMethods.ResizeImageToThumbnail(Storage.ReadImageFromIsolatedStorageAsStream(person.FileName), 50);


             
            this.GroupedPersonList =
            (from obj in this.People
             orderby obj.FirstName ascending
             group obj by obj.FirstName.ToUpper().ToList().First() into grouped
             select new Grouping<char, Person>(grouped)).ToList();

      
			this.GroupedDateList =
			(from obj in this.Dates.Where(x => x.DateOfMeeting.Date >= DateTime.Now.Date)
			 orderby (int)obj.ChronoGroupKey ascending, obj.DateOfMeeting ascending
			 group obj by obj.ChronoGroupKey.ToDescription() into grouped
			 select new Grouping<string, Date>(grouped)).ToList();

		}

        private string ResolveTileInfo(int num)
        {
            return num == 1 ? string.Format("{0} favourite", num) : string.Format("{0} favourites", num);
        }
	}
}
