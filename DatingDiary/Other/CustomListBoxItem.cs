using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DatingDiary.Other
{
    public class CustomListBoxItem<T> : ICustomListBox
    {
        public int Key { get; set; }
        public string Value { get; set; }
        public T Item { get; set; }
        public WriteableBitmap Image { get; set; }
        public Visibility ShowItem { get; set; }
    }

    public interface ICustomListBox
    {
        int Key { get; set; }
        string Value { get; set; }
    }
}
