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

namespace DatingDiary.Enums
{
    public enum LevelsOfInterest
    {
        [Description("Indifferent")]
        Indifferent = 1,
        [Description("Likes")]
        Likes = 2,
        [Description("Very Much Likes")]
        VeryMuchLikes = 3,
        [Description("Passionate")]
        Passionate = 4
    }
}
