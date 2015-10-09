using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GoogleWebServiceApi.Places.Models;
using DatingDiary.ViewModels.Dates;
using DatingDiary.Model;
using DatingDiary.Services;
using DatingDiary.Views.People;
using DatingDiary.ViewModels.People;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.UserData;
using System.Windows.Media.Imaging;


namespace DatingDiary.UserControls
{
    public partial class ImportContactUserControl : UserControl
    {
        private CEPersonView _cePersonView;
        private CEPersonViewModel _cePersonViewModel;
        private DialogService _dialogService;
        private AddressChooserTask addressTask;

        public ImportContactUserControl(CEPersonView cePersonView, DialogService dialogService)
        {
            InitializeComponent();
            _cePersonView = cePersonView;
            _cePersonViewModel = (CEPersonViewModel)_cePersonView.DataContext;
            _dialogService = dialogService;
        }

        void addressTask_Completed(object sender, AddressResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                Contacts contacts = new Contacts();
                contacts.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(contacts_SearchCompleted);
                contacts.SearchAsync(e.DisplayName, FilterKind.DisplayName, null);
            }
        }

        void contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {
            addressTask = new AddressChooserTask();
            addressTask.Completed += new EventHandler<AddressResult>(addressTask_Completed);

            foreach (var result in e.Results)
            {
                //this.tbdisplayName.Text = "Name: " + result.DisplayName;
                //this.tbEmail.Text = "E-mail address: " + result.EmailAddresses.FirstOrDefault().EmailAddress;
                //this.tbPhone.Text = "Phone Number: " + result.PhoneNumbers.FirstOrDefault();
                //this.tbPhysicalAddress.Text = "Address: " + result.Addresses.FirstOrDefault().PhysicalAddress.AddressLine1;
                //this.tbWebsite.Text = "Website: " + result.Websites.FirstOrDefault();

                _cePersonViewModel.ThePerson = new Person();
                _cePersonViewModel.ThePerson.FirstName = result.DisplayName;
                _cePersonViewModel.ThePerson.PhoneNumber = result.PhoneNumbers.FirstOrDefault().PhoneNumber;
                _cePersonViewModel.ThePerson.Email = result.EmailAddresses.FirstOrDefault().EmailAddress;

                if (result.GetPicture() != null)
                {
                    BitmapImage bmp = new BitmapImage();
                    bmp.SetSource(result.GetPicture());
                    Image img = new Image();
                    img.Source = bmp;
                    this.ContentPanel.Children.Add(img);
                }
            }
        }


        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text))
                MessageBox.Show("Please enter the name of a contact");
            else
            {
                EmptyMessage.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = true;


            }
        }

        private void ResultsListSelector_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }
    }
}
