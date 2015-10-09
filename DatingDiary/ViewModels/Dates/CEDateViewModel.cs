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
using DatingDiary.Model;
using System.Collections.Generic;
using DatingDiary.Repository;
using DatingDiary.Other;
using Microsoft.Phone.Controls;
using DatingDiary.Interfaces;
using DatingDiary.Helpers;

namespace DatingDiary.ViewModels.Dates
{
    public class CEDateViewModel : ViewModelBase
    {
        public Date TheDate { get; set; }
        public Person ThePerson { get; set; }
        public Venue TheVenue { get; set; }
        public List<CustomListBoxItem<Person>> People { get; set; }
        private Repository<Date> DateRep { get; set; }
        private Repository<Venue> VenueRep { get; set; }
        private bool IsEditing { get; set; }

        public CEDateViewModel(PhoneApplicationPage page, int? dateId = null) : base(page)
        {
            DateRep = new Repository<Date>(this.Ctx);
            VenueRep = new Repository<Venue>(this.Ctx);

            this.IsEditing = dateId.HasValue;

            if (this.IsEditing)
            {
                this.TheDate = Ctx.Dates.Where(x => x.Id == dateId).SingleOrDefault();
                this.ThePerson = this.TheDate.Person;
                this.TheVenue = this.TheDate.Venue; 

                this.Date = DateTime.Parse(this.TheDate.DateOfMeeting.ToShortDateString());
                this.Time = this.Date + this.TheDate.DateOfMeeting.TimeOfDay;
            }
            else
            {
                this.TheDate = new Date();
                this.TheVenue = new Venue();     
       
                this.Date = DateTime.Parse(DateTime.Now.ToShortDateString());
                this.Time = this.Date + DateTime.Now.TimeOfDay;
            }

            this.People = new List<CustomListBoxItem<Person>>();
            this.People.Add(new CustomListBoxItem<Person>() { Key = -1, Value = "Choose ...", Item = null, ShowItem = Visibility.Collapsed });
            this.People.AddRange((from p in Ctx.Persons orderby p.FirstName ascending select new CustomListBoxItem<Person>() { Key = p.Id, Value = p.FullName, Item = p, ShowItem = Visibility.Visible }).ToList());

            // the first one is the Choose item
            foreach (CustomListBoxItem<Person> person in this.People.Skip(1))
                person.Image = Storage.ReadImageFromIsolatedStorageToWriteableBitmap(string.Format("ProfileThumbnail\\{0}", person.Item.FileName));
        }

        private DateTime? fTime;
        public DateTime? Time
        {
            get { return fTime; }
            set
            {
                fTime = value;
                this.TheDate.DateOfMeeting = (DateTime)this.Date + ((DateTime)fTime).TimeOfDay;

                OnPropertyChanged("Time");
            }
        }

        private DateTime? fDate;
        public DateTime? Date
        {
            get { return fDate; }
            set
            {
                fDate = value;

                if (this.Time != null)
                    this.TheDate.DateOfMeeting = (DateTime)fDate + ((DateTime)this.Time).TimeOfDay;

                OnPropertyChanged("Date");
            }
        }

        private int fSelectedPersonIndex;
        public int SelectedPersonIndex
        {
            get
            {
                if (this.ThePerson == null)
                    return 0;
                else
                    return this.People.IndexOf(this.People.FirstOrDefault(x => x.Value == this.ThePerson.FullName));
            }
            set
            {
                ThePerson = ((CustomListBoxItem<Person>)this.People[value]).Item;
                fSelectedPersonIndex = this.People.IndexOf((CustomListBoxItem<Person>)this.People[value]);
                OnPropertyChanged("SelectedPersonIndex");
            }
        }

        public override string PageTitle
        {
            get
            {
                return "Add Date";
            }
        }

        public override void SubmitChanges()
        {
            DateRep.Insert(this.TheDate);
            VenueRep.Insert(this.TheVenue);

            this.TheVenue.Dates.Add(this.TheDate);
            this.ThePerson.Dates.Add(this.TheDate);
            
            base.SubmitChanges();
        }
    }
}
