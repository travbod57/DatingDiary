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
using System.Linq;
using System.Collections.Generic;

namespace DatingDiary.Behaviours
{
    public class RemoveFromBackStackBehaviour : IBackStackBehavior
    {
        public void RemoveEntry(PhoneApplicationPage page, List<string> uris)
        {

            var lastPage = page.NavigationService.BackStack.FirstOrDefault();

            if (lastPage != null)
            {
  

                for (int i = page.NavigationService.BackStack.Count() - 1; i >= 0; i--)
                {
                    bool remove = uris.Any(x => { return page.NavigationService.BackStack.ElementAt(i).Source.ToString().Contains(x); });

                    if (remove)
                        page.NavigationService.RemoveBackEntry();                    
                }


            }

   
        }
    }
}
