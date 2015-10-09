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
using System.Windows.Markup;
using System.Threading;
using System.Xml.Serialization;
using DatingDiary.Model;
using GoogleWebServiceApi.Places.Models;
using System.Text;
using DatingDiary.ViewModels.Dates;
using DatingDiary.Interfaces;
using Microsoft.Phone.Shell;
using DatingDiary.UserControls;
using DatingDiary.Services;
using System.ComponentModel;
using DatingDiary.Validation;
using DatingDiary.Other;
using DatingDiary.Views;
using DatingDiary.Animations;
using System.Windows.Navigation;

namespace DatingDiary.Views.Dates
{
    public partial class AddDateView : BasePage, IPage
	{
        private AddDateViewModel addDateViewModel;
        private DialogService _dialogService;
		private DateTime CombinedDateTime { get; set; }
		private DateTime Time { get; set; }
		private DateTime Date { get; set; }
        private int PersonId { get; set; }
        private ProgressIndicator Pi;

		public AddDateView()
		{
			InitializeComponent();
            this.SubClassPage = this;
            PageTitle.Text = "Add Date";

            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Pi = Microsoft.Phone.Shell.SystemTray.ProgressIndicator;
        }

        public Grid GetLayoutRoot
        {
            get
            {
                return this.LayoutRoot;
            }
        }


		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
			binding.UpdateSource();
		}

		private void Save_Click(object sender, EventArgs e)
		{
            ValidateObject(sender, e, this.ContentPanel.Children, new List<ListPickerValidationControl>() { this.listpicker });

            if (IsPageValid)
            {
                addDateViewModel.SubmitChanges();
                NavigationService.GoBack();
            }
            else
            {
                this.listpicker.SelectionChanged -= listpicker_SelectionChanged;
                this.listpicker.SelectionChanged += listpicker_SelectionChanged;
            }
		}

		private void Cancel_Click(object sender, EventArgs e)
		{
			NavigationService.GoBack();
		}

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // hack to always enable the Venue textbox if it has been disabled in the event the user clicks search while keyboard is showing
            this.BackKeyPress += EnableVenueTextBox;

            IDictionary<string, string> parameters = this.NavigationContext.QueryString;

            if (parameters.ContainsKey("PersonId"))
                PersonId = Int32.Parse(parameters["PersonId"]);

            base.OnNavigatedTo(e);

            if (e.NavigationMode != NavigationMode.Back)
            {
                SlideTransition transition = new SlideTransition();
                transition.Mode = SlideTransitionMode.SlideRightFadeIn;

                PhoneApplicationPage page = (PhoneApplicationPage)((PhoneApplicationFrame)Application.Current.RootVisual).Content;
                ITransition trans = transition.GetTransition(page);

                trans.Completed += delegate
                {
                    trans.Stop();

                    if (e.NavigationMode == NavigationMode.New)
                    {
                        addDateViewModel = new AddDateViewModel(this);
                        this.DataContext = addDateViewModel;
                    }

                };
                trans.Begin();
            }
        }

        private void EnableVenueTextBox(object sender, CancelEventArgs e)
        {
            if (!this.txtVenue.IsEnabled)
                this.txtVenue.IsEnabled = true;
        }

        void OnDialogServiceClosed(object sender, EventArgs e)
        {
            _dialogService.Closed -= OnDialogServiceClosed;
        }

        void OnDialogServiceOpened(object sender, EventArgs e)
        {
            _dialogService.Opened -= OnDialogServiceOpened;

            // hack to collapse the keyboard if it's open when venue search is pressed
            this.txtVenue.IsEnabled = false;
        }

        private void Search_Click(object sender, EventArgs e)
        {
            //_dialogService = new DialogService() { Child = new FindVenueUserControl(this, _dialogService, Pi) };
            _dialogService.IsOpen = true;
            //_dialogService.Child = new FindVenueUserControl(this, _dialogService);
            _dialogService.AnimationType = DialogService.AnimationTypes.Slide;
            _dialogService.Closed += OnDialogServiceClosed;
            _dialogService.Opened += OnDialogServiceOpened;
            _dialogService.Show();

        }

        private void listpicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListPickerValidationControl control = (ListPickerValidationControl)sender;

            if (control.SelectedItem != null)
            {
                IValidationRule v = (IValidationRule)control.ValidationRule;
                control.IsValid = v.Validate(((CustomListBoxItem<Person>)control.SelectedItem).Value == "Choose ..." ? "" : "valid"); ;
            }
        }

  
	}
}