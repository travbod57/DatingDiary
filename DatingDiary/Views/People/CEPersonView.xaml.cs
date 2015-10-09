using DatingDiary.Helpers;
using DatingDiary.Interfaces;
using DatingDiary.Model;
using DatingDiary.Other;
using DatingDiary.Singletons;
using DatingDiary.Validation;
using DatingDiary.ViewModels.People;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;

namespace DatingDiary.Views.People
{
    public partial class CEPersonView : BasePage, IPage
    {
        private AddressChooserTask addressTask;
        private int PersonId { get; set; }
        private CEPersonViewModel cePersonViewModel { get; set; }
        public Grid GetLayoutRoot
        {
            get
            {
                return this.LayoutRoot;
            }
        }

        public CEPersonView()
        {
            InitializeComponent();
            this.SubClassPage = this;
            PageTitle.Text = string.Format("Add {0}", AppContext.Instance.DatingGender.Substring(0, AppContext.Instance.DatingGender.Length - 1));
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
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
                        cePersonViewModel = new CEPersonViewModel(this, PersonId);
                        this.DataContext = cePersonViewModel;
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


        #region Choose Photo
        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                byte[] imageBytes = new byte[e.ChosenPhoto.Length];

                Guid guid = Guid.NewGuid();

                cePersonViewModel.ThePerson.FileName = string.Format("{0}\\{1}.jpg", "ProfilePicture", guid.ToString());

                string fileName = string.Format("{0}.jpg", guid.ToString());
                Storage.WriteImageToIsolatedStorage(e.ChosenPhoto, new List<ImageInfo>() { new ImageInfo() { Directory = "ProfilePicture", FileName = fileName, Height = 150, Width = 150 } });

                cePersonViewModel.SubmitChanges();
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
            if (IsPageValid)
            {
                cePersonViewModel.SubmitChanges();
                StringBuilder destination = new StringBuilder("/Views/People/PersonView.xaml");
                destination.AppendFormat("?PersonId={0}", cePersonViewModel.ThePerson.Id);

                NavigationService.Navigate(new Uri(destination.ToString(), UriKind.Relative));
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        } 
        #endregion

        #region Import Contact
        private void Import_Click(object sender, EventArgs e)
        {
            addressTask = new AddressChooserTask();
            addressTask.Completed += new EventHandler<AddressResult>(addressTask_Completed);
            addressTask.Show();
        }

        private void addressTask_Completed(object sender, AddressResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                Contacts contacts = new Contacts();
                contacts.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(contacts_SearchCompleted);
                contacts.SearchAsync(e.DisplayName, FilterKind.DisplayName, null);
            }
        }

        private void contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {
            foreach (var result in e.Results)
            {
                cePersonViewModel.ThePerson.FirstName = result.CompleteName.FirstName;
                cePersonViewModel.ThePerson.SecondName = result.CompleteName.LastName;
                cePersonViewModel.ThePerson.PhoneNumber = result.PhoneNumbers.Count(x => x.Kind == PhoneNumberKind.Mobile) > 0 ? result.PhoneNumbers.First(x => x.Kind == PhoneNumberKind.Mobile).PhoneNumber : null;
                cePersonViewModel.ThePerson.Email = result.EmailAddresses.Count() > 0 ? result.EmailAddresses.FirstOrDefault().EmailAddress : null;

                if (result.GetPicture() != null)
                {
                    Guid guid = Guid.NewGuid();

                    cePersonViewModel.ThePerson.FileName = string.Format("{0}\\{1}.jpg", "ProfilePicture", guid.ToString());

                    string fileName = string.Format("{0}.jpg", guid.ToString());
                    Storage.WriteImageToIsolatedStorage(result.GetPicture(), new List<ImageInfo>() { new ImageInfo() { Directory = "ProfilePicture", FileName = fileName, Height = 150, Width = 150 } });

                    //BitmapImage bmp = new BitmapImage();
                    //addPersonViewModel.Person.Image = GeneralMethods.ResizeImageToThumbnail(result.GetPicture(), 150);
                    //bmp.SetSource(result.GetPicture());
                    //Image img = new Image();
                    //img.Source = bmp;
                    //this.ContentPanel.Children.Add(img);
                }
            }
        } 
        #endregion

    }
}