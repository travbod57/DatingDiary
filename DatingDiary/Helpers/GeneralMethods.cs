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
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.Text;

namespace DatingDiary.Helpers
{
	public static class GeneralMethods
	{
		public static int? CalculateAge(DateTime dob)
		{
			if (dob <= DateTime.Now)
			{
				// Get the raw number of years
				int YearsAge = DateTime.Now.Year - dob.Year;
				// If the birthday hasn't occured this year, subtract one year from the age
				if (DateTime.Now.Month < dob.Month || (DateTime.Now.Month == dob.Month && DateTime.Now.Day < dob.Day))
					YearsAge--;

				return YearsAge;
			}
			else
				return null;
		}

        public static Stream CropSquare(Stream stream)
        {
            //turn stream into bitmapimage object
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);

            //calculate bounding box
            int iOriginalWidth = bitmapImage.PixelWidth;
            int iOriginalHeight = bitmapImage.PixelHeight;

            if (iOriginalWidth == iOriginalHeight)
            {
                //image is already square - return it
                return stream;
            }

            int smallestSide = (iOriginalHeight < iOriginalWidth) ? iOriginalHeight : iOriginalWidth;

            //generate temporary control to render image
            Image temporaryImage = new Image { Source = bitmapImage, Width = iOriginalWidth, Height = iOriginalHeight };

            //create writeablebitmap
            WriteableBitmap wb = new WriteableBitmap(smallestSide, smallestSide);
            wb.Render(temporaryImage, new TranslateTransform { X = (iOriginalWidth - smallestSide) / -2, Y = (iOriginalHeight - smallestSide) / -2 });
            wb.Invalidate();

            //get stream from writeablebitmap
            Stream streamResizedImage = new MemoryStream(); //will need to be disposed by whatever is using this method
            wb.SaveJpeg(streamResizedImage, smallestSide, smallestSide, 0, 70);
            return streamResizedImage;
        }

        public static WriteableBitmap ResizeImageToFullPage(Stream stream)
        {
            var rootVisual = Application.Current.RootVisual as PhoneApplicationFrame;

            //turn stream into bitmapimage object
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);

            //calculate bounding box
            int iOriginalWidth = bitmapImage.PixelWidth;
            int iOriginalHeight = bitmapImage.PixelHeight;
            double ratio;

            // Tall image
            if (iOriginalHeight > iOriginalWidth)
            {
                ratio = (double)rootVisual.ActualWidth / (double)iOriginalWidth;

                double newWidth = rootVisual.ActualWidth;
                double newHeight = iOriginalHeight * ratio;

                //generate temporary control to render image
                Image temporaryImage = new Image { Source = bitmapImage, Width = newWidth, Height = newHeight };

                //create writeablebitmap
                WriteableBitmap wb = new WriteableBitmap((int)newWidth, (int)Math.Round(newHeight, 0));
                wb.Render(temporaryImage, new TranslateTransform { X = 0, Y = 0 });
                wb.Invalidate();

                //get stream from writeablebitmap
                Stream streamResizedImage = new MemoryStream(); //will need to be disposed by whatever is using this method
                wb.SaveJpeg(streamResizedImage, (int)Math.Round(newWidth, 0), (int)Math.Round(newHeight, 0), 0, 70);
                return wb;
            }
            else
            {
                ratio = (double)rootVisual.ActualHeight / (double)iOriginalHeight;

                double newHeight = rootVisual.ActualHeight;
                double newWidth = iOriginalWidth * ratio;

                //generate temporary control to render image
                Image temporaryImage = new Image { Source = bitmapImage, Width = newWidth, Height = newHeight };

                //create writeablebitmap
                WriteableBitmap wb = new WriteableBitmap((int)newWidth, (int)Math.Round(newHeight, 0));
                wb.Render(temporaryImage, new TranslateTransform { X = 0, Y = 0 });
                wb.Invalidate();

                //get stream from writeablebitmap
                Stream streamResizedImage = new MemoryStream(); //will need to be disposed by whatever is using this method
                wb.SaveJpeg(streamResizedImage, (int)Math.Round(newWidth, 0), (int)Math.Round(newHeight, 0), 0, 70);
                return wb;
            }
        }

        /// <summary>
        /// Saves an image to isolated storage as a squae thumbnail
        /// </summary>
        /// <param name="fileStream">The file created to use in isolated storage</param>
        /// <param name="imageStream">The raw stream to save</param>
        /// <param name="dimension">A dimension of the square</param>
        /// <returns></returns>
        public static WriteableBitmap ResizeImageToSquare(Stream imageStream, int dimension)
        {
            //turn stream into bitmapimage object
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(imageStream);

            //calculate bounding box
            int iOriginalWidth = bitmapImage.PixelWidth;
            int iOriginalHeight = bitmapImage.PixelHeight;
            double ratio;

            // Tall image
            if (iOriginalHeight > iOriginalWidth)
            {
                ratio = (double)dimension / (double)iOriginalWidth;

                double newWidth = dimension;
                double newHeight = iOriginalHeight * ratio;

                //generate temporary control to render image
                Image temporaryImage = new Image { Source = bitmapImage, Width = newWidth, Height = newHeight };

                //create writeablebitmap
                WriteableBitmap wb = new WriteableBitmap(dimension, dimension);
                wb.Render(temporaryImage, new TranslateTransform { X = 0, Y = ((newHeight / 2) - (dimension / 2)) * -1 });
                wb.Invalidate();

                return wb;
            }
            else
            {
                ratio = (double)dimension / (double)iOriginalHeight;

                double newHeight = dimension;
                double newWidth = iOriginalWidth * ratio;

                //generate temporary control to render image
                Image temporaryImage = new Image { Source = bitmapImage, Width = newWidth, Height = newHeight };

                //create writeablebitmap
                WriteableBitmap wb = new WriteableBitmap(dimension, dimension);
                wb.Render(temporaryImage, new TranslateTransform { X = ((newWidth / 2) - (dimension / 2)) * -1, Y = 0 });
                wb.Invalidate();

                return wb;
            }
        }

        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }



        public static string XmlSerializeToString(this object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

        public static T XmlDeserializeFromString<T>(string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        public static object XmlDeserializeFromString(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }
	}
}
