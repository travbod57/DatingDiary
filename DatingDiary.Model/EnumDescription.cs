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
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;

namespace DatingDiary.Model
{
	public static class EnumDescription
	{
		/// <summary>
		/// Retrieve the description on the enum, e.g.
		/// [Description("Bright Pink")]
		/// BrightPink = 2,
		/// Then when you pass in the enum, it will retrieve the description
		/// </summary>
		/// <param name="en">The Enumeration</param>
		/// <returns>A string representing the friendly name</returns>
		public static string ToDescription(this Enum en)
		{
			Type type = en.GetType();

			MemberInfo[] memInfo = type.GetMember(en.ToString());

			if (memInfo != null && memInfo.Length > 0)
			{
				object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

				if (attrs != null && attrs.Length > 0)
				{
					return ((DescriptionAttribute)attrs[0]).Description;
				}
			}

			return en.ToString();
		}

	}
}
