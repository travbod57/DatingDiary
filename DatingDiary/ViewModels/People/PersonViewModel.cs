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
using Microsoft.Phone.Controls;
using DatingDiary.Views.People;
using DatingDiary.Enums;
using System.Windows.Media.Imaging;

namespace DatingDiary.ViewModels.People
{
	public class PersonViewModel : ViewModelBase, IViewModel
	{
		private List<Grouping<string, Date>> fGroupedDateList;
		public List<Grouping<string, Date>> GroupedDateList
		{
			get { return fGroupedDateList; }
			set { fGroupedDateList = value; }
		}

		public Visibility ShowDatesEmptyMessage
		{
			get { return this.Person.Dates.Count == 0 ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowDatesLongListPicker
		{
			get { return this.Person.Dates.Count > 0 ? Visibility.Visible : Visibility.Collapsed; }
		}

        public Visibility ShowPersonImage
        {
            get { return this.Person.Image != null ? Visibility.Visible : Visibility.Collapsed; }
        }

		private Person fPerson;
		public Person Person
		{
			get { return fPerson; }
			set
			{
				fPerson = value;
				OnPropertyChanged("Person");
			}
		}

        public override string PageTitle
        {
            get
            {
                return this.Person.FullName;
            }
        }

        public List<TagCloudItem> Interests { get; set; }

		public PersonViewModel(int personId, PhoneApplicationPage page) : base (page)
		{
			this.Person = Ctx.Persons.SingleOrDefault(x => x.Id == personId);

            if (!String.IsNullOrEmpty(this.Person.FileName))
                this.Person.Image = Storage.ReadImageFromIsolatedStorageToWriteableBitmap(string.Format("ProfilePicture\\{0}", this.Person.FileName)); //GeneralMethods.ResizeImageToThumbnail(Storage.ReadImageFromIsolatedStorageAsStream(this.Person.FileName), 150);
            else
                this.Person.Image = Storage.ReadImageFromContent("./Images/ChoosePhoto.jpg");

            foreach (Date date in this.Person.Dates)
                ChronoGrouping.MarkDate(date);

			this.GroupedDateList =
			(from obj in this.Person.Dates
			 orderby (int)obj.ChronoGroupKey ascending, obj.DateOfMeeting ascending
			 group obj by obj.ChronoGroupKey.ToDescription() into grouped
			 select new Grouping<string, Date>(grouped)).ToList();

            Interests = (from i in Ctx.Interests select new { Description = i.Description, FontSize = i.Weighting * 15, Interest = i }).AsEnumerable() 
             .Select(t => new TagCloudItem()
             {
                 Description = t.Description,
                 FontSize = t.FontSize,
                 Margin = new System.Windows.Thickness { Top = GetRandomInt(1, 20, t.FontSize / 15), Left = 10, Right = 10 },
                 Interest = t.Interest
             }).ToList();
		}
   
		public void Delete()
		{
			Repository<Person> rep = new Repository<Person>(Ctx);

			var dates = Ctx.Dates.Where(x => x.PersonId == this.Person.Id);
            var interests = Ctx.Interests.Where(x => x.PersonId == this.Person.Id);

			foreach (Date d in dates)
			{
				var photos = Ctx.Photos.Where(p => p.DateId == d.Id);
				var notes = Ctx.Notes.Where(n => n.DateId == d.Id);

				Ctx.Photos.DeleteAllOnSubmit(photos);
				Ctx.Notes.DeleteAllOnSubmit(notes);
			}

			Ctx.Dates.DeleteAllOnSubmit(dates);
            Ctx.Interests.DeleteAllOnSubmit(interests);
			Ctx.Persons.DeleteOnSubmit(this.Person);

			SubmitChanges();
		}

        Random rnd = new Random();
        private double GetRandomInt(int min, int max, double fontsize)
        {
            int x = rnd.Next(min, max);

            if (fontsize == 3)
                return x / 2;
            if (fontsize == 2)
                return x / 1.5;
            if (fontsize == 1)
                return x * 2;
            else
                return x;
        }

        public void AddToFavourites()
        {
            if (this.Person.IsFavourite == null || !(bool)this.Person.IsFavourite)
            {
                this.Person.IsFavourite = true;
                SubmitChanges();

                MessageBox.Show(string.Format("{0} has been added to your favourites", this.Person.FullName));
            }
            else
                MessageBox.Show(string.Format("{0} is already a favourite", this.Person.FullName));
        }
	}
}
