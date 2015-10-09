using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatingDiary.Model
{
    public class ChronoGrouping
    {
        private static int DaysUntilWeekend { get; set; }

        static ChronoGrouping()
        {
            DateTime now = DateTime.Now;
            int count = 0;

            if (now.DayOfWeek == DayOfWeek.Sunday)
                DaysUntilWeekend = 0;
            else
            {
                while (now.AddDays(count).DayOfWeek != DayOfWeek.Sunday)
                {
                    now.AddDays(1);
                    count++;
                }

                DaysUntilWeekend = count;
            }
        }

        public static void MarkDate(Date date)
        {
            // shows the date for the entirity of the day even if the hours pass
            // to make it the date go the moment it passes remove after the double ambersand
            if (date.DateOfMeeting < DateTime.Now && date.DateOfMeeting.Date != DateTime.Now.Date)
                date.ChronoGroupKey = ChronoGroup.Previously;
            else if (date.DateOfMeeting.Date == DateTime.Now.Date)
                date.ChronoGroupKey = ChronoGroup.Today;
            else if (date.DateOfMeeting <= DateTime.Now.AddDays(DaysUntilWeekend))
            {
                switch (date.DateOfMeeting.DayOfWeek)
                {
                    case DayOfWeek.Monday: date.ChronoGroupKey = ChronoGroup.Monday; break;
                    case DayOfWeek.Tuesday: date.ChronoGroupKey = ChronoGroup.Tuesday; break;
                    case DayOfWeek.Wednesday: date.ChronoGroupKey = ChronoGroup.Wednesday; break;
                    case DayOfWeek.Thursday: date.ChronoGroupKey = ChronoGroup.Thursday; break;
                    case DayOfWeek.Friday: date.ChronoGroupKey = ChronoGroup.Friday; break;
                    case DayOfWeek.Saturday: date.ChronoGroupKey = ChronoGroup.Saturday; break;
                    case DayOfWeek.Sunday: date.ChronoGroupKey = ChronoGroup.Sunday; break;
                }
            }
            else if (date.DateOfMeeting <= DateTime.Now.AddDays(DaysUntilWeekend + 7))
                date.ChronoGroupKey = ChronoGroup.NextWeek;
            else
                date.ChronoGroupKey = ChronoGroup.Future;
        }
    }
}
