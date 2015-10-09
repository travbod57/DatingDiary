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
using DatingDiary.Views.Notes;

namespace DatingDiary.ViewModels.Notes
{
	public class EditNoteViewModel : ViewModelBase
	{
		public Note Note { get; set; }

		public EditNoteViewModel(int NoteId, PhoneApplicationPage page) : base (page)
		{
			this.Note = Ctx.Notes.Where(x => x.Id == NoteId).SingleOrDefault();
		}

		public override void SubmitChanges()
        {
            this.Note.AmendedDate = DateTime.Now;
 	        base.SubmitChanges();
        }

        public override string PageTitle
        {
            get
            {
                return "Edit Note";
            }
        }
	}
}
