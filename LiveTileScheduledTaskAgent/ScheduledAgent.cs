using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using DatingDiary.Model;
using System;
using System.Globalization;
using System.Linq;

namespace LiveTileScheduledTaskAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private static volatile bool _classInitialized;

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        public ScheduledAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;
                // Subscribe to the managed exception handler
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += ScheduledAgent_UnhandledException;
                });
            }
        }

        /// Code to execute on Unhandled Exceptions
        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            //TODO: Add code to perform your task in background 
            if (task.Name == "DatingDiaryTask")
            {
                if (task is PeriodicTask)
                {

                    // Execute periodic task actions here.
                    ShellTile TileToFind = ShellTile.ActiveTiles.First(); // OrDefault(x => x.NavigationUri.ToString().Contains("DefaultTitle=DatingApp"));

                    if (TileToFind != null)
                    {
                        string ConnectionString = @"isostore:/DatingDiaryDB.sdf";
                        DatingAppContext ctx = new DatingAppContext(ConnectionString);

                        // get the number of dates there are today ...
                        int dateCount = ctx.Dates.Count(x => x.DateOfMeeting.Date == DateTime.Now.Date);

                        // get the next date
                        Date nextDate = ctx.Dates.FirstOrDefault(x => x.DateOfMeeting.Date == DateTime.Now.Date);

                        StandardTileData NewTileData = new StandardTileData
                        {
                            Title = dateCount > 0 ? "Dating Diary" : string.Empty,
                            Count = dateCount,
                            BackTitle = nextDate.Person.FullName,
                            BackBackgroundImage = new Uri("BackBackground.png", UriKind.RelativeOrAbsolute),
                            BackContent = string.Format("Today @ {0}", nextDate.DateOfMeeting.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern, CultureInfo.CurrentCulture.DateTimeFormat))
                        };

                        TileToFind.Update(NewTileData);
                    }
                }
                else
                {
                    // Execute resource-intensive task actions here.
                }
            }
            ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(3));
            NotifyComplete();
        } 
    }
}