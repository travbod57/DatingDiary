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
    public class AddDateViewModel : ViewModelBase
    {
        public Date NewDate { get; set; }
        public Person Person { get; set; }
        public Venue NewVenue { get; set; }
        public List<CustomListBoxItem<Person>> People { get; set; }
        private Repository<Date> DateRep { get; set; }
        private Repository<Venue> VenueRep { get; set; }

        public AddDateViewModel(PhoneApplicationPage page) : base(page)
        {
            this.NewDate = new Date();
            this.NewVenue = new Venue();

            DateRep = new Repository<Date>(this.Ctx);
            VenueRep = new Repository<Venue>(this.Ctx);

            this.Date = DateTime.Parse(DateTime.Now.ToShortDateString());
            this.Time = this.Date + DateTime.Now.TimeOfDay;

            this.People = new List<CustomListBoxItem<Person>>();
            this.People.Add(new CustomListBoxItem<Person>() { Key = -1, Value = "Choose ...", Item = null, ShowItem = Visibility.Collapsed });
            this.People.AddRange((from p in Ctx.Persons orderby p.FirstName ascending select new CustomListBoxItem<Person>() { Key = p.Id, Value = p.FullName, Item = p, ShowItem = Visibility.Visible }).ToList());

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
                this.NewDate.DateOfMeeting = (DateTime)this.Date + ((DateTime)fTime).TimeOfDay;

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
                    this.NewDate.DateOfMeeting = (DateTime)fDate + ((DateTime)this.Time).TimeOfDay;

                OnPropertyChanged("Date");
            }
        }

        private int fSelectedPersonIndex;
        public int SelectedPersonIndex
        {
            get
            {
                if (this.Person == null)
                    return 0;
                else
                    return this.People.IndexOf(this.People.FirstOrDefault(x => x.Value == this.Person.FullName));
            }
            set
            {
                Person = ((CustomListBoxItem<Person>)this.People[value]).Item;
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
            DateRep.Insert(this.NewDate);
            VenueRep.Insert(this.NewVenue);

            this.NewVenue.Dates.Add(this.NewDate);
            this.Person.Dates.Add(this.NewDate);
            
            base.SubmitChanges();
        }
    }
}
