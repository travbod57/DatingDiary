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
using System.Windows.Controls.Primitives;

namespace DatingDiary.Other
{
    public class PopupSingleton
    {
       private static Popup instance;

       private PopupSingleton() { }

       public static Popup Instance
       {
          get 
          {
             if (instance == null)
             {
                instance = new Popup();
             }
             return instance;
          }
       }




    }
}
