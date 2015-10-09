using DatingDiary.Enums;
using DatingDiary.Model;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using DatingDiary.ViewModels;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using DatingDiary.Services;

namespace DatingDiary.Singletons
{
    public class AppContext
    {
       private static AppContextData instance;

       private AppContext() { }

       public static AppContextData Instance
       {
          get 
          {
             if (instance == null)
             {
                 instance = new AppContextData();
             }
             return instance;
          }
        }
    }

    public class AppContextData
    {
        public AppLoadingStatus AppLoadingStatus { get; set; }
        public IsolatedStorageSettings IsolatedStorageSettings { get; set; }
        public bool IsAppLocked { get; set; }
        public Uri NextURI { get; set; }
        public string ConnectionString { get; set; }
        public EntityBase PassedData { get; set; }
        public string DatingGender { get; set; }
    }
}
