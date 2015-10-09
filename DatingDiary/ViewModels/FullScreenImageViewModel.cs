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
using DatingDiary.Repository;
using DatingDiary.Helpers;
using System.Windows.Media.Imaging;
using System.Linq;
using DatingDiary.Model;
using Microsoft.Phone.Controls;
using DatingDiary.ViewModels;

namespace DatingDiary.ViewModels
{
	public class FullScreenImageViewModel : ViewModelBase
	{
		public WriteableBitmap Image { get; set; }

		public FullScreenImageViewModel(int PhotoId, PhoneApplicationPage page) : base (page)
		{
			Repository<Photo> repPerson = new Repository<Photo>(Ctx);

			Photo photo = (from ph in Ctx.Photos where ph.Id == PhotoId select ph).SingleOrDefault();

			if (!String.IsNullOrEmpty(photo.FileName))
                Image = GeneralMethods.ResizeImageToFullPage(Storage.ReadImageFromIsolatedStorageAsStream(string.Format("{0}.jpg", photo.FileName)));
                
                //Image = Storage.ReadImageFromIsolatedStorageToWriteableBitmap(photo.FileName);
		}

        public override string PageTitle
        {
            get
            {
                return "Full Screen Image";
            }
        }
	}
}
