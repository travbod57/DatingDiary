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
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using DatingDiary.Other;
using DatingDiary.Model;
using DatingDiary.Repository;
using System.Collections.ObjectModel;
using System.Linq;

namespace DatingDiary.ViewModels.Venues
{
    public class VenueViewModel : ViewModelBase
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

        private List<Grouping<string, Date>> fGroupedPersonList;
        public List<Grouping<string, Date>> GroupedPersonList
        {
            get { return fGroupedPersonList; }
            set { fGroupedPersonList = value; }
        }

        public Venue Venue { get; set; }

        public VenueViewModel(PhoneApplicationPage page, int VenueId) : base(page)
		{

            Repository<Person> repPerson = new Repository<Person>(Ctx);
            this.People = new ObservableCollection<Person>(repPerson.GetAll());

            this.Venue = Ctx.Venues.SingleOrDefault(x => x.Id == VenueId);

            this.GroupedPersonList =
            (from pObj in this.People
             from dObj in pObj.Dates
             where dObj.VenueId == VenueId
             orderby pObj.FirstName ascending
             group dObj by dObj.Person.FullName into grouped
             select new Grouping<string, Date>(grouped)).ToList();
		}

        public override string PageTitle
        {
            get
            {
                return "Venue";
            }
        }
    }
}
