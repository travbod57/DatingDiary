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

namespace DatingDiary.Other
{
    public class TagCloudItem
    {
        public string Description { get; set; }
        public double FontSize { get; set; }
        public Thickness Margin { get; set; }
        public Interest Interest { get; set; }
    }
}
