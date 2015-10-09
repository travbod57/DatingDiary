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
using DatingDiary.ViewModels.Interests;
using DatingDiary.Interfaces;
using Microsoft.Phone.Shell;

namespace DatingDiary.Views.Interests
{
    public partial class AddInterestView : BasePage, IPage
    {
        private AddInterestViewModel addInterestViewModel;
        private int PersonId { get; set; }

        public AddInterestView()
        {
            InitializeComponent();
            this.SubClassPage = this;
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
            addInterestViewModel.SubmitChanges();
            NavigationService.GoBack();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            IDictionary<string, string> parameters = this.NavigationContext.QueryString;

            if (parameters.ContainsKey("PersonId"))
                PersonId = Int32.Parse(parameters["PersonId"]);

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                addInterestViewModel = new AddInterestViewModel(PersonId, this);
                this.DataContext = addInterestViewModel;
            }

            base.OnNavigatedTo(e);
        }
    }
}