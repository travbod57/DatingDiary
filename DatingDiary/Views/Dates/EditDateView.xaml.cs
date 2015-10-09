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
using DatingDiary.ViewModels.Dates;
using DatingDiary.Interfaces;
using DatingDiary.Other;
using Microsoft.Phone.Controls.Primitives;
using Microsoft.Phone.Shell;
using DatingDiary.Validation;
using DatingDiary.Model;
using DatingDiary.Animations;
using System.Windows.Navigation;

namespace DatingDiary.Views.Dates
{
    public partial class EditDateView : BasePage, IPage
	{
		private DateTime CombinedDateTime { get; set; }
		private DateTime Time { get; set; }
		private DateTime Date { get; set; }
		private string DateId { get; set; }
		private EditDateViewModel editDateViewModel { get; set; }


        public EditDateView()
		{
			InitializeComponent();
            this.SubClassPage = this;
            PageTitle.Text = "Edit Date";
            this.txtVenue.ItemFilter += SearchVenue;
        }

        bool SearchVenue(string search, object value)
        {
            if (value != null)
            {
                if (search == ((Venue)value).Name.ToString())
                    return false;
  
                // Look for a match in the last letter.
                if (((Venue)value).Name.ToString().ToLower().Contains(search.ToLower()))
                    return true;
            }
            // If no match, return false.
            return false;
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

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
            base.OnNavigatedTo(e);

			IDictionary<string, string> parameters = this.NavigationContext.QueryString;

			if (parameters.ContainsKey("DateId"))
				DateId = parameters["DateId"];


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
                        editDateViewModel = new EditDateViewModel(Int32.Parse(DateId), this);
                        this.DataContext = editDateViewModel;

                        DoubleLoopingDataSource doubleLoopingDataSource = new DoubleLoopingDataSource() { MinValue = 0, MaxValue = 5, SelectedItem = editDateViewModel.EditDate.Rating };
                        doubleLoopingDataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(LoopingSelector_SelectionChanged);

                        IntLoopingDataSource intLoopingDataSource = new IntLoopingDataSource() { MinValue = 0, MaxValue = 10, SelectedItem = editDateViewModel.EditDate.Rating };
                        intLoopingDataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(LoopingSelector_SelectionChanged);
                        this.selector.DataSource = doubleLoopingDataSource;
                    }

                };
                trans.Begin();
            }
		}

        private void LoopingSelector_SelectionChanged(object sender, EventArgs e)
        {
            editDateViewModel.EditDate.Rating = (double)((LoopingDataSourceBase)sender).SelectedItem;
        }

		private void Save_Click(object sender, EventArgs e)
		{
            ValidateObject(sender, e, this.ContentPanel.Children, new List<ListPickerValidationControl>() { this.listpicker });

            if (IsPageValid)
            {
                editDateViewModel.SubmitChanges();
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

		private void Search_Click(object sender, EventArgs e)
		{
            NavigationService.Navigate(new Uri("/Views/Venues/VenueSearch.xaml?SourcePage=EditDate", UriKind.Relative));
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