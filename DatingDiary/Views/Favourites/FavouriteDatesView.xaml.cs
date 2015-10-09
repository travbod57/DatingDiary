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
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.IO;
using DatingDiary.Helpers;
using Microsoft.Phone;
using System.Text;
using System.Windows.Markup;
using System.Threading;
using DatingDiary.Model;
using DatingDiary.Repository;
using DatingDiary.ViewModels.People;
using DatingDiary.Behaviours;
using DatingDiary.Interfaces;
using DatingDiary.Animations;
using DatingDiary.ViewModels.Favourites;

namespace DatingDiary.Views.Dates
{
    public partial class FavouriteDatesView : BasePage, IPage
	{
        private FavouriteDatesViewModel favouriteDatesViewModel { get; set; }

        public FavouriteDatesView()
		{
			InitializeComponent();
            AnimationContext = LayoutRoot;
            this.SubClassPage = this;

            AssessApplicationBarButtons(true, false, false);
		}

        public Grid GetLayoutRoot
        {
            get
            {
                return this.LayoutRoot;
            }
        }

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
            favouriteDatesViewModel = new FavouriteDatesViewModel(this);
            this.DataContext = favouriteDatesViewModel;

			base.OnNavigatedTo(e);
		}

        private void Edit_Click(object sender, EventArgs e)
        {
            this.listBoxWithCheckBoxes.IsInChooseState = true;
            AssessApplicationBarButtons(false, true, false);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.listBoxWithCheckBoxes.IsInChooseState = false;
            AssessApplicationBarButtons(true, false, false);
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            foreach (Date date in this.listBoxWithCheckBoxes.SelectedItems)
            {
                date.IsFavourite = false;
                favouriteDatesViewModel.Dates.Remove(date);
            }

            favouriteDatesViewModel.SubmitChanges();

            this.listBoxWithCheckBoxes.IsInChooseState = false;
            AssessApplicationBarButtons(true, false, false);
        }

        private void Date_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (!this.listBoxWithCheckBoxes.IsInChooseState)
            {
                StringBuilder destination = new StringBuilder("/Views/Dates/DateView.xaml");

                object item = ((ListBoxWithCheckBoxes)sender).SelectedItem;

                if (item != null)
                {
                    destination.AppendFormat("?DateId={0}", ((Date)((ListBoxWithCheckBoxes)sender).SelectedItem).Id);
                    NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
                }
            }
            else
                AssessApplicationBarButtons(false, true, this.listBoxWithCheckBoxes.SelectedItems.Count > 0);
        }

        private void AssessApplicationBarButtons(bool editState, bool cancelState, bool deleteState)
        {
            ((IApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = editState;
            ((IApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = cancelState;
            ((IApplicationBarIconButton)ApplicationBar.Buttons[2]).IsEnabled = deleteState;
        }
	}
}