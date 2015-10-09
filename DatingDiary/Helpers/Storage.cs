using System;
using System.Net;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.Phone;
using System.Windows.Resources;
using System.Windows.Media.Imaging;
using DatingDiary.Model;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DatingDiary.Helpers
{
	public class Storage
	{
        #region Write
        public static void WriteImageToIsolatedStorage(Stream imageStream, List<ImageInfo> images)
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                BitmapImage bitmap;

                foreach (ImageInfo imgInfo in images)
                {
                    if (!string.IsNullOrEmpty(imgInfo.Directory) && !myIsolatedStorage.DirectoryExists(imgInfo.Directory))
                        myIsolatedStorage.CreateDirectory(imgInfo.Directory);

                    string path = System.IO.Path.Combine(imgInfo.Directory, imgInfo.FileName);
                    IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(path);
                    
                    // saves images that are not square in shape
                    if (!imgInfo.IsSquare)
                    {
                        bitmap = new BitmapImage();
                        bitmap.SetSource(imageStream);

                        WriteableBitmap wb = new WriteableBitmap(bitmap);
                        wb.SaveJpeg(fileStream, imgInfo.Width, imgInfo.Height, 0, 85);
                    }
                    else
                    {
                        // resizes images to a square for use with thumbnails
                        WriteableBitmap wb = GeneralMethods.ResizeImageToSquare(imageStream, imgInfo.SquareSize);
                        wb.SaveJpeg(fileStream, imgInfo.SquareSize, imgInfo.SquareSize, 0, 70);
                    }

                    fileStream.Close();
                }
            }
        }

        public static void WriteTextToIsolatedStorageFile(string fileName, string writeThis)
        {
            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();

            if (!myIsolatedStorage.FileExists(fileName))
            {
                //Write to new file
                using (StreamWriter writeFile = new StreamWriter(new IsolatedStorageFileStream(fileName, FileMode.Create, FileAccess.Write, myIsolatedStorage)))
                {
                    writeFile.WriteLine(writeThis);
                    writeFile.Close();
                }
            }
            else
            {
                //Write to existing file
                IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Write);
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(writeThis);
                    writer.Close();
                }

            }
        }

        #endregion

        #region Read

        //public static void LoadLocalProfile(string fileName)
        //{
        //    IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();

        //    if (myIsolatedStorage.FileExists(fileName))
        //    {
        //        using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(fileName, FileMode.Open, myIsolatedStorage))
        //        {
        //            byte[] buffer = new byte[stream.Length];
        //            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(ReadCompleted), stream);
        //        }
        //    }
        //}

        static byte[] buffer;

        public static void AsyncImageLoad(string fileName, Person person)
        {
            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();

            using (IsolatedStorageFileStream stream = file.OpenFile(fileName, FileMode.Open))
            {
                buffer = new byte[stream.Length];
                ImageAsyncState state = new ImageAsyncState() { Stream = stream, Person = person };
                stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(ImageReadCompleted), state);
            }
        }

        static void ImageReadCompleted(IAsyncResult result)
        {
            ImageAsyncState state = (ImageAsyncState)result.AsyncState;
            state.Stream.EndRead(result);

            MemoryStream mStream = new MemoryStream(buffer);
            BitmapImage image = new BitmapImage();
            image.SetSource(mStream);

            WriteableBitmap wb = new WriteableBitmap(image);
            state.Person.Image = wb;
        }

        public static WriteableBitmap ReadImageFromIsolatedStorageToWriteableBitmap(string fileName)
        {

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(fileName))
                {
                    using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                    {
                        // Decode the JPEG stream.
                        return PictureDecoder.DecodeJpeg(fileStream);
                    }
                }
                else
                    return ReadImageFromContent("./Images/LittleDude.jpg");
            }
        }

        public static WriteableBitmap ReadImageFromContent(string filename)
        {
            // Create a stream for the JPEG file
            StreamResourceInfo sri = null;
            Uri jpegUri = new Uri(filename, UriKind.Relative);
            sri = Application.GetResourceStream(jpegUri);

            // Decode the JPEG stream.
            return PictureDecoder.DecodeJpeg(sri.Stream);
        }

        public static Stream ReadImageFromIsolatedStorageAsStream(string fileName)
        {

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(fileName))
                {
                    return myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read);
                }
                else
                {
                    return null;
                }
            }
        }

        public static string ReadTextFromIsolatedStorageFile(string fileName)
        {
            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();

            if (myIsolatedStorage.FileExists(fileName))
            {
                IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read);

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    return reader.ReadLine();
                }
            }
            else
                return string.Empty;
        } 
        #endregion

        #region Delete
        public static void DeleteFile(string fileName)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                store.DeleteFile(fileName);
        } 
        #endregion
	}

    public class ImageAsyncState
    {
        public IsolatedStorageFileStream Stream { get; set; }
        public Person Person { get; set; }
    }
}
