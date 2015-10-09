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
    public class CEPersonViewModel : ViewModelBase
    {
        private bool IsEditing { get; set; }

        public CEPersonViewModel(PhoneApplicationPage page, int? personId = null) : base(page)
        {
            this.IsEditing = personId.HasValue;

            if (IsEditing)
            {
                this.ThePerson = Ctx.Persons.Where(x => x.Id == personId).SingleOrDefault();
                this.DateOfBirth = this.ThePerson.DateOfBirth;
            }
            else
            {
                this.ThePerson = new Person();
            }
        }

        private Person fThePerson;
        public Person ThePerson
        {
            get { return fThePerson; }
            set
            {
                fThePerson = value;
                OnPropertyChanged("ThePerson");
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

                this.ThePerson.DateOfBirth = fDateOfBirth;

                if (this.ThePerson.DateOfBirth.HasValue)
                {
                    this.ThePerson.Age = GeneralMethods.CalculateAge((DateTime)this.ThePerson.DateOfBirth);
                    OnPropertyChanged("Age");
                }
            }
        }

        public override string PageTitle
        {
            get
            {
                return !this.IsEditing ? "Add Person" : "Edit Person";
            }
        }

        public override void SubmitChanges()
        {
            base.SubmitChanges();
        }
    }
}