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
    public partial class FavouriteVenuesView : BasePage, IPage
	{
        private FavouriteVenuesViewModel favouriteVenuesViewModel { get; set; }

        public FavouriteVenuesView()
		{
			InitializeComponent();
            AnimationContext = LayoutRoot;
            this.SubClassPage = this;
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
            favouriteVenuesViewModel = new FavouriteVenuesViewModel(this);
            this.DataContext = favouriteVenuesViewModel;

			base.OnNavigatedTo(e);
		}

        private void Edit_Click(object sender, EventArgs e)
        {
            this.listBoxWithCheckBoxes.IsInChooseState = true;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.listBoxWithCheckBoxes.IsInChooseState = false;
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            foreach (Venue venue in this.listBoxWithCheckBoxes.SelectedItems)
            {
                venue.IsFavourite = false;
                favouriteVenuesViewModel.Venues.Remove(venue);
            }

            favouriteVenuesViewModel.SubmitChanges();

            this.listBoxWithCheckBoxes.IsInChooseState = false;
        }
	}
}