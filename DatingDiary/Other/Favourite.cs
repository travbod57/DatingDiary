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
using System.Collections.Generic;

namespace DatingDiary.Other
{
    public class Favourite
    {
        public List<Person> People { get; set; }
        public List<Date> Dates { get; set; }
        public List<Venue> Venues { get; set; }
    }
}
