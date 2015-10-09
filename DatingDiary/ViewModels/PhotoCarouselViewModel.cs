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
using DatingDiary.Model;
using System.Collections.ObjectModel;
using DatingDiary.Helpers;
using Microsoft.Phone.Controls;
using System.Linq;
using DatingDiary.Views;
using System.Windows.Media.Imaging;

namespace DatingDiary.ViewModels
{
    public class PhotoCarouselViewModel : ViewModelBase
    {
        public ObservableCollection<Photo> Photos { get; set; }
        public ObservableCollection<WriteableBitmap> Images { get; set; }

        public PhotoCarouselViewModel(int dateId, int photoId, PhoneApplicationPage page) : base(page)
        {
            this.Photos = new ObservableCollection<Photo>((from ph in Ctx.Photos where ph.DateId == dateId select ph).ToList());

            //foreach (Photo ph in this.Photos)
            //    ph.Image = GeneralMethods.ResizeImageToThumbnail(Storage.ReadFromIsolatedStorageAsStream(ph.FileName, ph.Width, ph.Height), 480);
            Images = new ObservableCollection<WriteableBitmap>();

         
                    

            foreach (Photo ph in this.Photos)
                Images.Add(GeneralMethods.ResizeImageToFullPage(Storage.ReadImageFromIsolatedStorageAsStream(ph.FileName))); //Storage.ReadImageFromIsolatedStorageToWriteableBitmap(ph.FileName));
            


            ((PhotoCarousel)page).PivotControl.SelectedIndex = this.Photos.IndexOf(this.Photos.SingleOrDefault(x => x.Id == photoId));
            //((PhotoCarousel)page).PanoramaControl.SelectedIndex
            //((PhotoCarousel)page).PanoramaControl.SelectedIndex = this.Photos.IndexOf(this.Photos.SingleOrDefault(x => x.Id == photoId));
        }

        public override string PageTitle
        {
            get
            {
                return string.Empty;
            }
        }
    }
}
