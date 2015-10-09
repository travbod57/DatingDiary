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
using System.Text.RegularExpressions;
using DatingDiary.Interfaces;
using Microsoft.Phone.Shell;

namespace DatingDiary.Views.Notes
{
    public partial class EditNoteView : BasePage, IPage
	{

		private string NoteId { get; set; }
		private EditNoteViewModel editNoteViewModel { get; set; }
	
        public EditNoteView()
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

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			IDictionary<string, string> parameters = this.NavigationContext.QueryString;

			if (parameters.ContainsKey("NoteId"))
			{
				NoteId = parameters["NoteId"];
			}

			if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
			{
				editNoteViewModel = new EditNoteViewModel(Int32.Parse(NoteId), this);
				this.DataContext = editNoteViewModel;
			}

			base.OnNavigatedTo(e);
		}

		private void Save_Click(object sender, EventArgs e)
		{
			editNoteViewModel.SubmitChanges();
			NavigationService.GoBack();
		}

		private void Cancel_Click(object sender, EventArgs e)
		{
			NavigationService.GoBack();
		}

        private void NoteContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }


        
	}
}