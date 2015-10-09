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
using DatingDiary.Enums;

namespace DatingDiary.Converters
{
    public class EnumToBoolConverter : IValueConverter
	{

        // Convert enum [value] to boolean, true if matches [param]
        public object Convert(object value, Type targetType, object param, CultureInfo culture)
        {
            LevelsOfInterest radioButton = (LevelsOfInterest)Enum.Parse(typeof(LevelsOfInterest), param.ToString(), true);
            LevelsOfInterest boundValue = (LevelsOfInterest)Enum.Parse(typeof(LevelsOfInterest), value.ToString(), true);

            return radioButton == boundValue ? true : false;
        }

        // Convert boolean to enum, returning [param] if true
        public object ConvertBack(object value, Type targetType, object param, CultureInfo culture)
        {
            return (bool)value ? param : null;
        }
    }
	
}
