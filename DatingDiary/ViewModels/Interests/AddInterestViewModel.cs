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
using DatingDiary.Enums;

namespace DatingDiary.ViewModels.Interests
{
    public class AddInterestViewModel : ViewModelBase
    {
        private Interest fInterest;
        public Interest Interest
        {
            get { return fInterest; }
            set
            {
                fInterest = value;
                OnPropertyChanged("Interest");
            }
        }

        public List<CustomListBoxItem<LevelsOfInterest>> Interests { get; set; }
        public Person person { get; set; }

        public AddInterestViewModel(int personId, PhoneApplicationPage page) : base(page)
        {
            this.Interest = new Interest();
   
            person = Ctx.Persons.SingleOrDefault( x => x.Id == personId);

            this.Interests = new List<CustomListBoxItem<LevelsOfInterest>>();

            foreach (var e in EnumHelper.GetValues<LevelsOfInterest>())
                this.Interests.Add(new CustomListBoxItem<LevelsOfInterest>() { Key = 1, Value = e.ToDescription(), Item = e, ShowItem = Visibility.Visible });

            SelectedLevelOfInterestIndex = 0;
        }

        private LevelsOfInterest fLevelsOfInterest;
        public LevelsOfInterest LevelsOfInterest
        {
            get { return fLevelsOfInterest; }
            set
            {
                fLevelsOfInterest = value;
                this.Interest.Weighting = (int)fLevelsOfInterest;
                OnPropertyChanged("LevelsOfInterest");
            }
        }

        private int fSelectedLevelOfInterestIndex;
        public int SelectedLevelOfInterestIndex
        {
            get { return fSelectedLevelOfInterestIndex; }
            set
            {
                LevelsOfInterest = ((CustomListBoxItem<LevelsOfInterest>)Interests[value]).Item;
                fSelectedLevelOfInterestIndex = Interests.IndexOf((CustomListBoxItem<LevelsOfInterest>)Interests[value]);
                OnPropertyChanged("SelectedLevelOfInterestIndex");
            }
        }

        public override void SubmitChanges()
        {
            person.Interests.Add(this.Interest);
            base.SubmitChanges();
        }

        public override string PageTitle
        {
            get
            {
                return "Add Interest";
            }
        }
    }
}
