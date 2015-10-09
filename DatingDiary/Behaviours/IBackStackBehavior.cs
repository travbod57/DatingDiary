using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Phone.Controls;

namespace DatingDiary.Behaviours
{
    public interface IBackStackBehavior
    {
        void RemoveEntry(PhoneApplicationPage page, List<string> uris);
    }
}
