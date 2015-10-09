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
using DatingDiary.Helpers;
using System.Windows.Media.Imaging;
using DatingDiary.Model;
using DatingDiary.Repository;
using Microsoft.Phone.Controls;
using DatingDiary.Views;
using DatingDiary.Interfaces;
using System.ComponentModel;
using System.Threading;
using System.Collections.Generic;

namespace DatingDiary.ViewModels.People
{
	public class DateViewModel : ViewModelBase, IViewModel
	{
		public Date Date { get; set; }

		public ObservableCollection<Photo> Photos { get; set; }
        public ObservableCollection<Note> Notes { get; set; }
        public int SelectedPhotoCount { get; set; }

		public Visibility ShowNotesEmptyMessage
		{
			get { return this.Date.Notes.Count == 0 ? Visibility.Visible : Visibility.Collapsed; }
		}

        public Visibility NotesLongListSelector
		{
			get { return this.Date.Notes.Count > 0 ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowPhotosEmptyMessage
		{
			get { return this.Date.Photos.Count == 0 ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility ShowPhotosListBox
		{
			get { return this.Date.Photos.Count > 0 ? Visibility.Visible : Visibility.Collapsed; }
		}

		public DateViewModel(int dateId, PhoneApplicationPage page) : base (page)
		{
  
			this.Date = Ctx.Dates.SingleOrDefault(x => x.Id == dateId);
            this.Notes = new ObservableCollection<Note>(this.Date.Notes.OrderByDescending(x => x.CreatedDate).ToList());
            this.Photos = new ObservableCollection<Photo>((from ph in Ctx.Photos where ph.DateId == dateId select ph).ToList());

            foreach (Photo ph in this.Photos)
                ph.Image = Storage.ReadImageFromIsolatedStorageToWriteableBitmap(string.Format("{0}Small.jpg", ph.FileName));
		}


		public void DeleteNote(Note note)
		{
            this.Notes.Remove(note);
            this.Ctx.Notes.DeleteOnSubmit(note);
			SubmitChanges();
		}

        public void DeletePhoto(Photo photo)
        {
            Ctx.Photos.DeleteOnSubmit(photo);
            Storage.DeleteFile(string.Format("{0}Small.jpg", photo.FileName));
			SubmitChanges();
        }

        public override string PageTitle
        {
            get
            {
                return this.Date.Venue.Name;
            }
        }

        public void AddToFavourites()
        {
            if (this.Date.IsFavourite == null || !(bool)this.Date.IsFavourite)
            {
                this.Date.IsFavourite = true;
                SubmitChanges();

                MessageBox.Show("This date has already been added to favourites");
            }
            else
                MessageBox.Show("This date is already a favourite");
        }

	}
}
