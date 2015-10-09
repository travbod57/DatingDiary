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
using DatingDiary.ViewModels.Notes;
using DatingDiary.Interfaces;
using DatingDiary.ViewModels;
using Microsoft.Phone.Shell;

namespace DatingDiary.Views.Notes
{
    public partial class AddNoteView : BasePage, IPage
	{

        private int DateId { get; set; }
        private AddNoteViewModel addNoteViewModel { get; set; }

        public AddNoteView()
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

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			IDictionary<string, string> parameters = this.NavigationContext.QueryString;

            if (parameters.ContainsKey("DateId"))
                DateId = Int32.Parse(parameters["DateId"]);

			if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
			{
                addNoteViewModel = new AddNoteViewModel(DateId, this);
                this.DataContext = addNoteViewModel;
			}

			base.OnNavigatedTo(e);
		}

		private void Save_Click(object sender, EventArgs e)
		{
            addNoteViewModel.SubmitChanges();
			NavigationService.GoBack();
		}

		private void Cancel_Click(object sender, EventArgs e)
		{
			NavigationService.GoBack();
		}

	}
}