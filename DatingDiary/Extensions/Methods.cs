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

namespace DatingDiary.Extensions
{
	public static class Methods
	{
		public static string ToSingular(this string str)
		{
			return str.Substring(0, str.Length - 1);
		}
	}
}
