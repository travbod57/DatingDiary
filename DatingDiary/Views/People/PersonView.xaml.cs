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
using System.Text;
using DatingDiary.Model;
using Microsoft.Phone.Shell;
using DatingDiary.Extensions;
using Microsoft.Phone.Tasks;
using DatingDiary.Helpers;
using System.Reflection;
using DatingDiary.ViewModels.People;
using System.Windows.Navigation;
using DatingDiary.Behaviours;
using DatingDiary.ViewModels;
using DatingDiary.Interfaces;
using DatingDiary.Animations;
using DatingDiary.Singletons;
namespace DatingDiary.Views.People
{
    public partial class PersonView : BasePage, IPage
	{
		private string PersonId { get; set; }
        private PersonViewModel personViewModel { get; set; }
        private IBackStackBehavior IBackStackBehaviour { get; set; }

        public PersonView()
		{
			InitializeComponent();
            this.SubClassPage = this;
            AnimationContext = LayoutRoot;

		}

        #region IPage
        public Grid GetLayoutRoot
        {
            get
            {
                return this.LayoutRoot;
            }
        } 
        #endregion


        //protected override AnimatorHelperBase GetAnimation(AnimationType animationType, Uri toOrFrom)
        //{
        //    if (animationType == AnimationType.NavigateForwardIn)
        //        return new TurnstileForwardInAnimator() { RootElement = LayoutRoot };
        //    else if (animationType == AnimationType.NavigateBackwardOut)
        //        return new TurnstileBackwardOutAnimator() { RootElement = LayoutRoot };
        //    else
        //        return null;
        //    //else
        //    // return new SlideDownAnimator() { RootElement = LayoutRoot };
        //}

		#region Navigation
		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
            
			IDictionary<string, string> parameters = this.NavigationContext.QueryString;

			if (parameters.ContainsKey("PersonId"))
				PersonId = parameters["PersonId"];

            IBackStackBehaviour = new RemoveFromBackStackBehaviour();
            IBackStackBehaviour.RemoveEntry(this, new List<string>() { "CEPersonView", "AddInterestView", "CEDateView" });

            personViewModel = new PersonViewModel(Int32.Parse(PersonId), this);
            this.DataContext = personViewModel;

			base.OnNavigatedTo(e);
		}
		#endregion

		#region Other Events
		private void Date_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			StringBuilder destination = new StringBuilder("/Views/Dates/DateView.xaml");

			object item = ((LongListSelector)sender).SelectedItem;

			if (item != null)
			{
				destination.AppendFormat("?DateId={0}", ((Date)((LongListSelector)sender).SelectedItem).Id);
				NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
			}
		}

		private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			
			switch (((Pivot)sender).SelectedIndex)
			{
				case 0:

					ApplicationBar = ((ApplicationBar)this.Resources["PersonView_Details_AppBar"]);

					IApplicationBarIconButton editDetailsButton = (IApplicationBarIconButton)ApplicationBar.Buttons[0];
                    editDetailsButton.Text = string.Format("Edit {0}", AppContext.Instance.DatingGender.ToSingular());

					IApplicationBarMenuItem deletePersonMenuItem = (IApplicationBarMenuItem)ApplicationBar.MenuItems[0];
                    deletePersonMenuItem.Text = string.Format("Delete {0}", AppContext.Instance.DatingGender.ToSingular());

					break;

				case 1:
					ApplicationBar = ((ApplicationBar)this.Resources["PersonView_Dates_AppBar"]);

					break;
				case 2:
                    ApplicationBar = ((ApplicationBar)this.Resources["PersonView_Interests_AppBar"]);
					break;
			}
		} 
		#endregion

		private void Details_Edit_Click(object sender, EventArgs e)
		{
            
			Person person = personViewModel.Person;

			if (person != null)
			{
                //AppContext.Instance.PassedData = person;

                StringBuilder destination = new StringBuilder("/Views/People/CEPersonView.xaml");
                destination.AppendFormat("?PersonId={0}", ((PersonViewModel)this.DataContext).Person.Id);

                NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
			}
		}

		private void Details_Delete_Click(object sender, EventArgs e)
		{
			MessageBoxResult result = MessageBox.Show(string.Format("Are you sure you want to delete {0}?", personViewModel.Person.FullName), "Warning", MessageBoxButton.OKCancel);

			if (result == MessageBoxResult.OK)
			{
				personViewModel.Delete();
				NavigationService.Navigate(new Uri("/Views/Home.xaml", UriKind.Relative));
			}
		}

		private void Dates_Add_Click(object sender, EventArgs e)
		{
            StringBuilder destination = new StringBuilder("/Views/Dates/CEDateView.xaml");
            destination.AppendFormat("?PersonId={0}", PersonId);
            NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
		}

        private void Interests_Add_Click(object sender, EventArgs e)
        {
            StringBuilder destination = new StringBuilder("/Views/Interests/AddInterestView.xaml");
            destination.AppendFormat("?PersonId={0}", PersonId);
            NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //((TextBox)sender).Text
        }

        private void AddTo_Favourites_Click(object sender, EventArgs e)
        {
            personViewModel.AddToFavourites();
        }

	}
}