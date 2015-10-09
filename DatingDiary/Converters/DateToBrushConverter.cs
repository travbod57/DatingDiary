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
using DatingDiary.Model;

namespace DatingDiary.Converters
{
	public class DateToBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ChronoGroup chronoGroup = (ChronoGroup)value;

			switch (chronoGroup)
			{
				case ChronoGroup.Previously :
					return new SolidColorBrush(Colors.Green);
				default :
					return new SolidColorBrush(Colors.Red);
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
