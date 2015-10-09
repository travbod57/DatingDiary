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
using Microsoft.Phone.Controls;
using DatingDiary.Other;
using System.Collections.Generic;
using DatingDiary.Repository;

namespace DatingDiary.ViewModels.Dates
{
	public class EditDateViewModel : ViewModelBase
	{
		public Date EditDate { get; set; }
        public List<CustomListBoxItem<Person>> People { get; set; }
        public List<Venue> Venues { get; set; }
        private Repository<Venue> VenueRep { get; set; }

		public EditDateViewModel(int DateId, PhoneApplicationPage page) : base(page)
		{
            VenueRep = new Repository<Venue>(this.Ctx);
			this.EditDate = Ctx.Dates.Where(x => x.Id == DateId).SingleOrDefault();
			this.Date = DateTime.Parse(this.EditDate.DateOfMeeting.ToShortDateString());
			this.Time = this.Date + this.EditDate.DateOfMeeting.TimeOfDay;

            this.People = new List<CustomListBoxItem<Person>>();
            this.People.Add(new CustomListBoxItem<Person>() { Key = -1, Value = "Choose ...", Item = null, ShowItem = Visibility.Collapsed });
            this.People.AddRange((from p in Ctx.Persons select new CustomListBoxItem<Person>() { Key = p.Id, Value = p.FullName, Item = p, ShowItem = Visibility.Visible }).ToList());

            this.Venues = new List<Venue>(VenueRep.GetAll());

		}


        private int fSelectedPersonIndex;
        public int SelectedPersonIndex
        {
            get
            {
                if (this.EditDate.Person == null)
                    return 0;
                else
                    return this.People.IndexOf(this.People.FirstOrDefault(x => x.Value == this.EditDate.Person.FullName));
            }
            set
            {
                this.EditDate.Person = ((CustomListBoxItem<Person>)this.People[value]).Item;
                fSelectedPersonIndex = this.People.IndexOf((CustomListBoxItem<Person>)this.People[value]);
                OnPropertyChanged("SelectedPersonIndex");
            }
        }

		private DateTime? fTime;
		public DateTime? Time
		{
			get { return fTime; }
			set
			{
				fTime = value;
				this.EditDate.DateOfMeeting = (DateTime)this.Date + ((DateTime)fTime).TimeOfDay;

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
					this.EditDate.DateOfMeeting = (DateTime)fDate + ((DateTime)this.Time).TimeOfDay;

				OnPropertyChanged("Date");
			}
		}

        public override string PageTitle
        {
            get
            {
                return "Edit Date";
            }
        }

	}
}
