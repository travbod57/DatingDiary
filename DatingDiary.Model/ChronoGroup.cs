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
using System.ComponentModel;

namespace DatingDiary.Model
{
	public enum ChronoGroup
	{
		[Description("Previously")]
		Previously = 1,
		[Description("Today")]
		Today = 2,
        [Description("Monday")]
        Monday = 3,
        [Description("Tuesday")]
        Tuesday = 4,
        [Description("Wednesday")]
        Wednesday = 5,
        [Description("Thursday")]
        Thursday = 6,
        [Description("Friday")]
        Friday = 7,
        [Description("Saturday")]
        Saturday = 8,
        [Description("Sunday")]
        Sunday = 9,
		[Description("Next Week")]
		NextWeek = 10,
		[Description("Future")]
		Future = 11,

	}
}
