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

namespace DatingDiary.Other
{
    public class TileItem
    {
        public string ImageUri
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Notification
        {
            get;
            set;
        }

        public bool DisplayNotification
        {
            get
            {
                return !string.IsNullOrEmpty(this.Notification);
            }
        }

        public string Message
        {
            get;
            set;
        }

        public string GroupTag
        {
            get;
            set;
        }
    }
}
