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
using System.Windows.Data;
using System.Globalization;
using DatingDiary.Other;
using DatingDiary.Enums;
using DatingDiary.Model;

namespace DatingDiary.Converters
{
    public class InterestToWordingConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            TagCloudItem selectedTagCloudItem = (TagCloudItem)value;

            if (selectedTagCloudItem != null)
                return string.Format("{0} about {1}", ((LevelsOfInterest)selectedTagCloudItem.Interest.Weighting).ToDescription(), selectedTagCloudItem.Interest.Description);
            else
                return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
