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
using DatingDiary.Model;
using System.Collections.ObjectModel;
using DatingDiary.Helpers;
using Microsoft.Phone.Controls;
using System.Linq;
using DatingDiary.Views;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using DatingDiary.Other;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Windows.Threading;
using DatingDiary.Extensions;

namespace DatingDiary.ViewModels
{
    public class TestPageViewModel : ViewModelBase
    {

//private List<CustomListBoxItem<Country>> fCountries;
        //public List<CustomListBoxItem<Country>> Countries
        //{
        //    get { return fCountries; }
        //    set 
        //    { 
        //        fCountries = value;
        //        OnPropertyChanged("Countries");
        //    }
        //}

        //public ObservableCollection<CustomListBoxItem<Country>> Countries { get; set; }

        //private List<CustomListBoxItem<Country>> backgroundList = new List<CustomListBoxItem<Country>>();
        private BackgroundWorker bgw = new BackgroundWorker();
        private DispatcherTimer timer;
        private int count = 0;
        private int take = 20;

        public TestPageViewModel(PhoneApplicationPage page) : base(page)
        {
            bgw.DoWork += bgw_DoWork;
            bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
            bgw.RunWorkerAsync();

           // this.Countries = new ObservableCollection<CustomListBoxItem<Country>>();
            //this.Countries.Add(new CustomListBoxItem<Country>() { Key = -1, Value = "Choose ...", Item = null, ShowItem = Visibility.Visible });
            //this.Countries.Add(new CustomListBoxItem<Country>() { Key = 1, Value = "Albania", Item = null, ShowItem = Visibility.Visible });
            //this.Countries.Add(new CustomListBoxItem<Country>() { Key = 2, Value = "Britain", Item = null, ShowItem = Visibility.Visible });
            //this.Countries.Add(new CustomListBoxItem<Country>() { Key = 3, Value = "France", Item = null, ShowItem = Visibility.Visible });
            //this.Countries.Add(new CustomListBoxItem<Country>() { Key = 4, Value = "Germany", Item = null, ShowItem = Visibility.Visible });
            //this.Countries.Add(new CustomListBoxItem<Country>() { Key = 5, Value = "Israel", Item = null, ShowItem = Visibility.Visible });
            //this.Countries.Add(new CustomListBoxItem<Country>() { Key = 6, Value = "Portugal", Item = null, ShowItem = Visibility.Visible });
            //this.Countries.Add(new CustomListBoxItem<Country>() { Key = 7, Value = "Spain", Item = null, ShowItem = Visibility.Visible });
            //this.Countries.Add(new CustomListBoxItem<Country>() { Key = 8, Value = "Thailand", Item = null, ShowItem = Visibility.Visible });
            //this.Countries.Add(new CustomListBoxItem<Country>() { Key = 9, Value = "Venezuala", Item = null, ShowItem = Visibility.Visible });
        }

        void timer_Tick(object sender, EventArgs e)
        {
            //this.Countries.AddRange(this.backgroundList.Skip(take * count).Take(take).ToList());

            //count++;
            //if ((take * (count - 1)) + take >= this.backgroundList.Count)
            //    timer.Stop();
        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            //this.backgroundList.Add(new CustomListBoxItem<Country>() { Key = 1, Value = "Albania", Item = null, ShowItem = Visibility.Visible });
            //this.backgroundList.Add(new CustomListBoxItem<Country>() { Key = 2, Value = "Britain", Item = null, ShowItem = Visibility.Visible });
            //this.backgroundList.Add(new CustomListBoxItem<Country>() { Key = 3, Value = "France", Item = null, ShowItem = Visibility.Visible });
            //this.backgroundList.Add(new CustomListBoxItem<Country>() { Key = 4, Value = "Germany", Item = null, ShowItem = Visibility.Visible });
            //this.backgroundList.Add(new CustomListBoxItem<Country>() { Key = 5, Value = "Israel", Item = null, ShowItem = Visibility.Visible });
            //this.backgroundList.Add(new CustomListBoxItem<Country>() { Key = 6, Value = "Portugal", Item = null, ShowItem = Visibility.Visible });
            //this.backgroundList.Add(new CustomListBoxItem<Country>() { Key = 7, Value = "Spain", Item = null, ShowItem = Visibility.Visible });
            //this.backgroundList.Add(new CustomListBoxItem<Country>() { Key = 8, Value = "Thailand", Item = null, ShowItem = Visibility.Visible });
            //this.backgroundList.Add(new CustomListBoxItem<Country>() { Key = 9, Value = "Venezuala", Item = null, ShowItem = Visibility.Visible });
        }

        //private int fSelectedCountryIndex;
        //public int SelectedCountryIndex
        //{
        //    get { return fSelectedCountryIndex; }
        //    set
        //    {
        //        fSelectedCountryIndex = Countries.IndexOf((CustomListBoxItem<Country>)Countries[value]);
        //        OnPropertyChanged("SelectedCountryIndex");
        //    }
        //}



        public void RaiseChange(string name)
        {
            OnPropertyChanged(name);
        }

        public override string PageTitle
        {
            get
            {
                return "Add Person";
            }
        }
    }
}
