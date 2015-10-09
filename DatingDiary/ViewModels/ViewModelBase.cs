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
using System.ComponentModel;
using Microsoft.Phone.Controls;
using DatingDiary.Enums;
using DatingDiary.Interfaces;
using System.Text.RegularExpressions;
using System.Windows.Controls.Primitives;
using DatingDiary.Model;
using DatingDiary.Singletons;

namespace DatingDiary.ViewModels
{
    public abstract class ViewModelBase : IViewModel, INotifyPropertyChanged
    {
        public ViewModelBase(PhoneApplicationPage page)
        {
            this.Ctx = new DatingAppContext(AppContext.Instance.ConnectionString);
            this.Page = page;
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, args);
        }

        #endregion

        public DatingAppContext Ctx { get; set; }

        public PhoneApplicationPage Page { get; set; }

        public void DisposeContext()
        {
            this.Ctx.Dispose();
        }

        public virtual void SubmitChanges()
        {
            this.Ctx.SubmitChanges();
        }

        public abstract string PageTitle { get; }
    }
}
