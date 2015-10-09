using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;
using System.Threading;
using System.IO.IsolatedStorage;
using DatingDiary.Interfaces;
using Microsoft.Phone.Shell;
using DatingDiary.ViewModels;
using DatingDiary.Behaviours;
using System.Windows.Navigation;
using System.Diagnostics;

namespace DatingDiary.Views
{
    public partial class SettingsView : BasePage, IPage
	{
        private SettingsViewModel settingsViewModel;
        private IBackStackBehavior IBackStackBehaviour { get; set; }

		public SettingsView()
		{
			InitializeComponent();
            this.SubClassPage = this;
            AnimationContext = LayoutRoot;
        }

        public Grid GetLayoutRoot
        {
            get
            {
                return this.LayoutRoot;
            }
        }

		private void StartPeriodicTask()
		{
            PeriodicTask periodicTask = new PeriodicTask("DatingDiaryTask");
			periodicTask.Description = "Are presenting a periodic task";

			try
			{
				ScheduledActionService.Add(periodicTask);
                ScheduledActionService.LaunchForTest("DatingDiaryTask", TimeSpan.FromSeconds(3));
				MessageBox.Show("Open the background agent success");
			}
			catch (InvalidOperationException exception)
			{
				if (exception.Message.Contains("exists already"))
				{
					MessageBox.Show("Since then the background agent success is already running");
				}
				if (exception.Message.Contains("BNS Error: The action is disabled"))
				{
					MessageBox.Show("Background processes for this application has been prohibited");
				}
				if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
				{
					MessageBox.Show("You open the daemon has exceeded the hardware limitations");
				}
			}
			catch (SchedulerServiceException)
			{

			}
		}
		private void StopPeriodicTask()
		{
			try
			{
                ScheduledActionService.Remove("DatingDiaryTask");
				MessageBox.Show("Turn off the background agent successfully");
			}
			catch (InvalidOperationException exception)
			{
				if (exception.Message.Contains("doesn't exist"))
				{
					MessageBox.Show("Since then the background agent success is not running");
				}
			}
			catch (SchedulerServiceException)
			{

			}
		}
		private void StartPeriodicTask_Click(object sender, RoutedEventArgs e)
		{
			StartPeriodicTask();
			SetData();
		}
		private void StopPeriodicTask_Click(object sender, RoutedEventArgs e)
		{
			StopPeriodicTask();
		}
		public void SetData()
		{
            //Mutex mutex = new Mutex(false, "ScheduledAgentData");
            //mutex.WaitOne();
            //IsolatedStorageSettings setting = (App.Current as App).AppContext.IsolatedStorageSettings;

            //if (!setting.Contains("ScheduledAgentData"))
            //    setting.Add("ScheduledAgentData", "Foreground data");
            //mutex.ReleaseMutex();
		}

        private void Security_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Security/SetPassCode.xaml", UriKind.Relative));
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            IDictionary<string, string> parameters = this.NavigationContext.QueryString;

            foreach (var item in this.NavigationService.BackStack)
            {
                Debug.WriteLine(item.Source.ToString());
            }

            IBackStackBehaviour = new RemoveFromBackStackBehaviour();
            IBackStackBehaviour.RemoveEntry(this, new List<string>() { "SetPassCode", "Register", "SettingsView" });          

            settingsViewModel = new SettingsViewModel(this);
            this.DataContext = settingsViewModel;

            if (parameters.ContainsKey("PassCodeSet"))
            {
                bool PassCodeSet;
                bool.TryParse(parameters["PassCodeSet"], out PassCodeSet);

                settingsViewModel.IsPassCodeRequired = PassCodeSet;
            }

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }

        private void passCodeToggle_Click(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = (ToggleSwitch)sender;
            bool isChecked = (bool)toggleSwitch.IsChecked;

            if (isChecked)
                NavigationService.Navigate(new Uri("/Views/Security/Register.xaml", UriKind.Relative));
        }


	}
}