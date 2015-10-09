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
using DatingDiary.Model;
using DatingDiary.Helpers;
using System.Text;
using System.Windows.Data;
using DatingDiary.ViewModels.People;
using System.Windows.Navigation;
using DatingDiary.Interfaces;
using Microsoft.Phone.Shell;
using DatingDiary.Validation;
using DatingDiary.Animations;
using DatingDiary.Singletons;
using DatingDiary.Other;

namespace DatingDiary.Views.People
{
    public partial class EditPersonView : BasePage, IPage
	{
        private Person Person { get; set; }
		private EditPersonViewModel editPersonViewModel { get; set; }
        public Grid GetLayoutRoot
        {
            get
            {
                return this.LayoutRoot;
            }
        }

        public EditPersonView()
		{
			InitializeComponent();
            this.SubClassPage = this;
            PageTitle.Text = string.Format("Edit {0}", AppContext.Instance.DatingGender.Substring(0, AppContext.Instance.DatingGender.Length - 1));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
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
                        editPersonViewModel = new EditPersonViewModel(this);
                        this.DataContext = editPersonViewModel;
                    }

                };
                trans.Begin();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindingExpression be = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource();
        }

        private void listpicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListPickerValidationControl control = (ListPickerValidationControl)sender;

            if (control.SelectedItem != null)
            {
                IValidationRule v = (IValidationRule)control.ValidationRule;
                control.IsValid = v.Validate(((ICustomListBox)control.SelectedItem).Value == "Choose ..." ? "" : "valid"); ;
            }
        }

        #region Choose Photo
        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                byte[] imageBytes = new byte[e.ChosenPhoto.Length];

                Guid guid = Guid.NewGuid();

                //editPersonViewModel.Person.FileName = string.Format("{0}\\{1}.jpg", "ProfilePicture", guid.ToString());

                string fileName = string.Format("{0}.jpg", guid.ToString());
                editPersonViewModel.Person.FileName = fileName;

                ImageInfo profilePic = new ImageInfo() { Directory = "ProfilePicture", FileName = string.Format("{0}.jpg", guid.ToString()), IsSquare = true, Height = 150, Width = 150 };
                ImageInfo thumbnail = new ImageInfo() { Directory = "ProfileThumbnail", FileName = string.Format("{0}.jpg", guid.ToString()), IsSquare = true, Height = 70, Width = 70 };

                Storage.WriteImageToIsolatedStorage(e.ChosenPhoto, new List<ImageInfo>() { profilePic, thumbnail });

                editPersonViewModel.Save();
            }
        } 
        

		private void ChangePhoto_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Choose photo");

			PhotoChooserTask photoChooserTask = new PhotoChooserTask();
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
        #endregion

        #region CRUD
        private void Save_Click(object sender, EventArgs e)
        {
            ValidateObject(sender, e, this.ContentPanel.Children, new List<ListPickerValidationControl>() { this.CountryListpicker });

            if (IsPageValid)
            {
                editPersonViewModel.Save();
                StringBuilder destination = new StringBuilder("/Views/People/PersonView.xaml");
                destination.AppendFormat("?PersonId={0}", editPersonViewModel.Person.Id);

                NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
            }
            else
            {
                this.CountryListpicker.SelectionChanged -= listpicker_SelectionChanged;
                this.CountryListpicker.SelectionChanged += listpicker_SelectionChanged;
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
        
        #endregion

	}
}