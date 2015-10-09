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
using Microsoft.Phone.UserData;
using System.Windows.Media.Imaging;
using System.Windows.Markup;
using System.Threading;
using DatingDiary.Model;
using DatingDiary.Interfaces;
using Microsoft.Phone.Shell;
using DatingDiary.ViewModels;

namespace DatingDiary.Views
{
    public partial class ImportContactView : BasePage, IPage
	{
		AddressChooserTask addressTask;

		public ImportContactView()
		{
			this.DataContext = new ImportContactViewModel(this);
			InitializeComponent();
            this.SubClassPage = this;

			addressTask = new AddressChooserTask();
			addressTask.Completed += new EventHandler<AddressResult>(addressTask_Completed);
        }

        public Grid GetLayoutRoot
        {
            get
            {
                return this.LayoutRoot;
            }
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
			foreach (var result in e.Results)
			{
				//this.tbdisplayName.Text = "Name: " + result.DisplayName;
				//this.tbEmail.Text = "E-mail address: " + result.EmailAddresses.FirstOrDefault().EmailAddress;
				//this.tbPhone.Text = "Phone Number: " + result.PhoneNumbers.FirstOrDefault();
				//this.tbPhysicalAddress.Text = "Address: " + result.Addresses.FirstOrDefault().PhysicalAddress.AddressLine1;
				//this.tbWebsite.Text = "Website: " + result.Websites.FirstOrDefault();

				((ImportContactViewModel)this.DataContext).Contact = new Person();
				((ImportContactViewModel)this.DataContext).Contact.FirstName = result.DisplayName;
				((ImportContactViewModel)this.DataContext).Contact.PhoneNumber = result.PhoneNumbers.FirstOrDefault().PhoneNumber;
				((ImportContactViewModel)this.DataContext).Contact.Email = result.EmailAddresses.FirstOrDefault().EmailAddress;

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

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			addressTask.Show();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{

		}
	}
}