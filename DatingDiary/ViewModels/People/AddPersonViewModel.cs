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
using Microsoft.Phone.Controls;
using System.Threading;
using DatingDiary.Views.People;
using System.ComponentModel;
using DatingDiary.Validation;
using System.Windows.Threading;
using DatingDiary.Extensions;

namespace DatingDiary.ViewModels.People
{
    public class AddPersonViewModel : ViewModelBase
    {
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

        private DateTime? fDateOfBirth;
        public DateTime? DateOfBirth
        {
            get { return fDateOfBirth; }
            set
            {
                fDateOfBirth = value;
                OnPropertyChanged("DateOfBirth");

                this.Person.DateOfBirth = fDateOfBirth;

                if (this.Person.DateOfBirth.HasValue)
                {
                    this.Person.Age = GeneralMethods.CalculateAge((DateTime)this.Person.DateOfBirth);
                    OnPropertyChanged("Age");
                }
            }
        }

        private int fSelectedCountryIndex;
        public int SelectedCountryIndex
        {
            get
            {
                if (this.Person.Country == null)
                    return 0;
                else
                    return Countries.IndexOf(Countries.FirstOrDefault(x => x.Value == this.Person.Country.Name));
            }
            set
            {
                Person.Country = ((CustomListBoxItem<Country>)Countries[value]).Item;
                fSelectedCountryIndex = Countries.IndexOf((CustomListBoxItem<Country>)Countries[value]);
                OnPropertyChanged("SelectedCountryIndex");
            }
        }

        public ObservableCollection<CustomListBoxItem<Country>> Countries { get; set; }

        public override string PageTitle
        {
            get
            {
                return "Add Person";
            }
        }

        private List<CustomListBoxItem<Country>> backgroundList = new List<CustomListBoxItem<Country>>();
        private BackgroundWorker bgw;
        private DispatcherTimer timer;
        private int count = 0;
        private int take = 20;

        public AddPersonViewModel(PhoneApplicationPage page) : base(page)
        {
            bgw = new BackgroundWorker();
            bgw.DoWork += bgw_DoWork;
            bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
            bgw.RunWorkerAsync();

            this.Person = new Person();

            this.Countries = new ObservableCollection<CustomListBoxItem<Country>>();
            this.Countries.Add(new CustomListBoxItem<Country>() { Key = -1, Value = "Choose ...", Item = null, ShowItem = Visibility.Collapsed });
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.Countries.AddRange(this.backgroundList.Skip(take * count).Take(take).ToList());

            count++;
            if ((take * (count - 1)) + take >= this.backgroundList.Count)
            {
                timer.Stop();
                OnPropertyChanged("SelectedCountryIndex");
            }
        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            this.backgroundList.AddRange((from c in Ctx.Countries select new CustomListBoxItem<Country>() { Key = c.Id, Value = c.Name, Item = c, ShowItem = Visibility.Visible }).ToList());
        }

        public override void SubmitChanges()
        {
            base.SubmitChanges();
        }
    }
}