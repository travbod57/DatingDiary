using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Linq;
using DatingDiary.Model;
using Microsoft.Phone.Controls;
using System.Text.RegularExpressions;

namespace DatingDiary.ViewModels.Notes
{
	public class AddNoteViewModel : ViewModelBase
	{
		public Note Note { get; set; }

        public AddNoteViewModel(int dateId, PhoneApplicationPage page) : base (page)
		{
            this.Note = new Note();

            Date date = Ctx.Dates.Single(d => d.Id == dateId);
            date.Notes.Add(this.Note);
            
		}

        public override void SubmitChanges()
		{
            this.Note.CreatedDate = DateTime.Now;
			base.SubmitChanges();
		}

        public override string PageTitle
        {
            get
            {
                return "Add Note";
            }
        }
	}
}
