using DatingDiary.Animations;
using DatingDiary.Behaviours;
using DatingDiary.Helpers;
using DatingDiary.Interfaces;
using DatingDiary.Model;
using DatingDiary.ViewModels.People;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DatingDiary.Extensions;
using System.Windows.Media;

namespace DatingDiary.Views.Dates
{
    public partial class DateView : BasePage, IPage
	{
		private string DateId { get; set; }
		private DateViewModel dateViewModel { get; set; }
        private IBackStackBehavior IBackStackBehaviour { get; set; }
        private GeoCoordinateWatcher _watcher;
        private bool _needDirections = false;
        private int SelectedPhotoCount = 0;
        private bool IsInSelectState = false;

		public DateView()
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

        protected override AnimatorHelperBase GetAnimation(AnimationType animationType, Uri toOrFrom)
        {

            //you could factor this into an intermediate base page to have soem other defaults
            //such as always continuuming to a pivot page or rultes based on the page, direction and where you are goign to/coming from

            return base.GetAnimation(animationType, toOrFrom);
        }

        protected override void AnimationsComplete(AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationType.NavigateForwardIn:

                    break;

                case AnimationType.NavigateBackwardIn:

                    break;
            }


            base.AnimationsComplete(animationType);
        }
     
		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			IDictionary<string, string> parameters = this.NavigationContext.QueryString;
            IBackStackBehaviour = new RemoveFromBackStackBehaviour();
			
            if (parameters.ContainsKey("DateId"))
				DateId = parameters["DateId"];

            IBackStackBehaviour.RemoveEntry(this, new List<string>() { "CEDateView", "AddNoteView", "EditNoteView" });

			dateViewModel = new DateViewModel(Int32.Parse(DateId), this);
			this.DataContext = dateViewModel;

			base.OnNavigatedTo(e);
		}




		private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			switch (((Pivot)sender).SelectedIndex)
			{
				case 0:
					ApplicationBar = ((ApplicationBar)this.Resources["DateView_Details_AppBar"]);
					break;

				case 1:
					ApplicationBar = ((ApplicationBar)this.Resources["DateView_Notes_AppBar"]);
					break;

				case 2:
                    ApplicationBar = ((ApplicationBar)this.Resources["DateView_NonSelectState_Photos_AppBar"]);
                    ((IApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true;
                    ((IApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = true;
					break;
			}
		}



        #region Notes
        private void AddNote_Click(object sender, EventArgs e)
        {
            StringBuilder destination = new StringBuilder("/Views/Notes/AddNoteView.xaml");
            destination.AppendFormat("?DateId={0}", DateId);

            NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
        }

        private void EditNote_Click(object sender, EventArgs e)
        {
            Note note = (Note)((LongListSelector)this.NotesLongListSelector).SelectedItem;

            if (note != null)
            {
                StringBuilder destination = new StringBuilder("/Views/Notes/EditNoteView.xaml");
                destination.AppendFormat("?NoteId={0}", note.Id);

                NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
            }
        }

        private void DeleteNote_Click(object sender, EventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this note?", "Warning", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
                dateViewModel.DeleteNote((Note)this.NotesLongListSelector.SelectedItem);
        } 
        #endregion









        public void Home_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Home.xaml", UriKind.Relative));
        }

        private void AddTo_Favourites_Click(object sender, EventArgs e)
        {
            dateViewModel.AddToFavourites();
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            var rootVisual = Application.Current.RootVisual as PhoneApplicationFrame;
            MessageBox.Show(string.Format("frame height : {0}, width : {1}", rootVisual.Height, rootVisual.Width));
        }

        private void EditDateDetails_Click(object sender, EventArgs e)
        {
            StringBuilder destination = new StringBuilder("/Views/Dates/CEDateView.xaml");
            destination.AppendFormat("?DateId={0}", ((DateViewModel)this.DataContext).Date.Id);

            NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
        }




        #region Mapping
        private void ShowOnMap_Click(object sender, EventArgs e)
        {
            StartLocationServices(false);
        }

        private void GetDirections_Click(object sender, EventArgs e)
        {
            StartLocationServices(true);
        }

        private void StartLocationServices(bool needDirections)
        {
            _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            _watcher.MovementThreshold = 20;
            _watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            _watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);

            _watcher.Start();

            _needDirections = needDirections;
        }

        private void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    MessageBox.Show("Location Service is not enabled on the device");
                    break;

                case GeoPositionStatus.NoData:
                    MessageBox.Show("The Location Service is working, but it cannot get location data.");
                    break;
            }
        }

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (e.Position.Location.IsUnknown)
            {
                MessageBox.Show("Please wait while your prosition is determined");
                return;
            }

            if (_needDirections)
            {
                BingMapsDirectionsTask Direction = new BingMapsDirectionsTask();
                LabeledMapLocation start = new LabeledMapLocation("Current Location", new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude));
                LabeledMapLocation End = new LabeledMapLocation(dateViewModel.Date.Venue.Name, new GeoCoordinate(dateViewModel.Date.Venue.Latitude, dateViewModel.Date.Venue.Longitude));
                Direction.Start = start;
                Direction.End = End;

                _watcher.Stop();
                Direction.Show();
            }
            else
            {
                BingMapsTask bingMapsTask = new BingMapsTask();
                LabeledMapLocation currentLocation = new LabeledMapLocation("Current Location", new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude));

                bingMapsTask.ZoomLevel = 10;
                bingMapsTask.Center = currentLocation.Location;
                _watcher.Stop();
                bingMapsTask.Show();
            }
        } 
        #endregion

        #region Photo Tab Events

        private void Delete_Photos_Click(object sender, EventArgs e)
        {
            this.IsInSelectState = false;
            ApplicationBar = ((ApplicationBar)this.Resources["DateView_NonSelectState_Photos_AppBar"]);

            for (int i = PhotoListBox.Items.Count - 1; i >= 0; i--)
            {
                if (((Photo)PhotoListBox.Items[i]).IsSelected)
                {
                    dateViewModel.DeletePhoto((Photo)PhotoListBox.Items[i]);
                    ((DateViewModel)this.DataContext).Photos.Remove((Photo)PhotoListBox.Items[i]);
                }
                else
                {
                    ListBoxItem lbi = (ListBoxItem)PhotoListBox.ItemContainerGenerator.ContainerFromIndex(i);
                    Image image = GeneralMethods.FindChild<Image>(lbi, "ImageMask");
                    image.Opacity = 0;
                    image.Visibility = Visibility.Collapsed;

                    Border border = (Border)lbi.GetVisualChild(0);
                    border.BorderBrush = new SolidColorBrush(Colors.White);

                    ((Photo)PhotoListBox.Items[i]).IsSelected = false;
                    TiltEffect.SetIsTiltEnabled(((Photo)PhotoListBox.Items[i]).Image, true);
                }
            }

            ((IApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true;
            ((IApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = true;

            SelectedPhotoCount = 0;
        }

        private void Select_Photos_Click(object sender, EventArgs e)
        {
            this.IsInSelectState = true;
            ApplicationBar = ((ApplicationBar)this.Resources["DateView_SelectState_Photos_AppBar"]);

            for (int i = PhotoListBox.Items.Count - 1; i >= 0; i--)
            {
                ListBoxItem lbi = (ListBoxItem)PhotoListBox.ItemContainerGenerator.ContainerFromIndex(i);
                Image image = GeneralMethods.FindChild<Image>(lbi, "ImageMask");

                // shouldn't really need to do these two but something funny happends if I don't
                image.Visibility = Visibility.Visible;
                image.Opacity = 0;

                Border border = (Border)lbi.GetVisualChild(0);
                border.BorderBrush = new SolidColorBrush(Colors.Blue);

                ((Photo)PhotoListBox.Items[i]).IsSelected = false;
                TiltEffect.SetIsTiltEnabled(((Photo)PhotoListBox.Items[i]).Image, false);
            }

            ((IApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = false;
            ((IApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = true;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.IsInSelectState = false;
            ApplicationBar = ((ApplicationBar)this.Resources["DateView_NonSelectState_Photos_AppBar"]);

            for (int i = PhotoListBox.Items.Count - 1; i >= 0; i--)
            {
                ListBoxItem lbi = (ListBoxItem)PhotoListBox.ItemContainerGenerator.ContainerFromIndex(i);
                Image image = GeneralMethods.FindChild<Image>(lbi, "ImageMask");
                image.Opacity = 0;
                image.Visibility = Visibility.Collapsed;

                Border border = (Border)lbi.GetVisualChild(0);
                border.BorderBrush = new SolidColorBrush(Colors.White);

                ((Photo)PhotoListBox.Items[i]).IsSelected = false;
                TiltEffect.SetIsTiltEnabled(((Photo)PhotoListBox.Items[i]).Image, true);
            }

            ((IApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true;
            ((IApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = true;

            SelectedPhotoCount = 0;
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Image image = (Image)sender;
            Photo item = image.DataContext as Photo;

            item.IsSelected = !item.IsSelected;

            if (!this.IsInSelectState)
            {
                e.Handled = true; // stops it going through to the interactivity that triggesr the storyboard.

                StringBuilder destination = new StringBuilder("/Views/FullScreenImageView.xaml");
                destination.AppendFormat("?PhotoId={0}", image.Tag);

                NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
            }
            else
            {
                if (item.IsSelected)
                    SelectedPhotoCount++;
                else
                    SelectedPhotoCount--;

                ((IApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = SelectedPhotoCount > 0;
                ((IApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = true;
            }


            // else
                // go to image viewer page

            //StringBuilder destination = new StringBuilder("/Views/FullScreenImageView.xaml");
            //destination.AppendFormat("?PhotoId={0}", image.Tag);

            //StringBuilder destination = new StringBuilder("/Views/PhotoCarousel.xaml");
            //destination.AppendFormat("?DateId={0}&PhotoId={1}", DateId, image.Tag);

            //NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
        }

        private void Date_PhotoLauncher_Click(object sender, EventArgs e)
        {
            PhotoChooserTask photoChooserTask = new PhotoChooserTask() { PixelWidth = 500, PixelHeight = 500 };
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);

            try
            {
                photoChooserTask.Show();
            }
            catch
            {
                MessageBox.Show("An error occurred.");
            }
        }

        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                byte[] imageBytes = new byte[e.ChosenPhoto.Length];

                Guid guid = Guid.NewGuid();
                string fileName = string.Format("DatePicture\\{0}", guid.ToString());

                Photo p1 = new Photo() { DateId = ((DateViewModel)this.DataContext).Date.Id, Description = "Description", FileName = fileName, CreatedDate = DateTime.Now.ToString() };
                ((DateViewModel)this.DataContext).Ctx.Photos.InsertOnSubmit(p1);

                int ScreenWidth = Int32.Parse(System.Windows.Application.Current.Host.Content.ActualWidth.ToString());
                int ScreenHeight = Int32.Parse(System.Windows.Application.Current.Host.Content.ActualHeight.ToString());

                ImageInfo fullImage = new ImageInfo() { Directory = "DatePicture", FileName = string.Format("{0}.jpg", guid.ToString()), IsSquare = false, Height = ScreenHeight, Width = ScreenWidth };
                ImageInfo thumbnail = new ImageInfo() { Directory = "DatePicture", FileName = string.Format("{0}Small.jpg", guid.ToString()), IsSquare = true, Height = 136, Width = 136 };

                Storage.WriteImageToIsolatedStorage(e.ChosenPhoto, new List<ImageInfo>() { thumbnail, fullImage });

                //Storage.WriteImageToIsolatedStorage(e.ChosenPhoto, new List<ImageInfo>() { new ImageInfo() { Directory = "DatePicture", FileName = string.Format("{0}Small.jpg", guid.ToString()), Height = 136, Width = 136 } });

                ((DateViewModel)this.DataContext).Ctx.SubmitChanges();
            }
        }

        #endregion

	}
}