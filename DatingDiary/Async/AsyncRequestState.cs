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
using System.Threading;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Controls.Primitives;
using DatingDiary.UserControls;
using Microsoft.Phone.Controls;
using DatingDiary.Views;
using Microsoft.Phone.Shell;

namespace DatingDiary.Async
{
    public class AsyncRequestState
    {
        public HttpWebRequest request;
        public AutoResetEvent allDone;  // putting this here instead of making it static allows multiple requests to be handled concurrently
        public Action<Stream> actionToPerform;
        public bool Success { get; set; }
        public string Message { get; set; }

        public AsyncRequestState(Action<Stream> action)
        {
            this.request = null;
            this.actionToPerform = action;
        }  
    }

    public class AsyncRequest
    {
        public ProgressIndicator PI { get; set; }
        public PhoneApplicationPage Page { get; set; }
        public string Message { get; set; }

        public AsyncRequest(ProgressIndicator pi, PhoneApplicationPage page)
        {
            this.PI = pi;
            this.Page = page;
        }

        public void Invoke(int timeout_ms, AsyncRequestState asyncRequestState, string location)
        {
            try
            {
                this.PI.IsVisible = true;
                this.Page.Opacity = 0.3;

                ThreadPool.QueueUserWorkItem(new WaitCallback(target =>
                {

                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(location);
                    asyncRequestState.request = httpWebRequest;
                    asyncRequestState.allDone = new AutoResetEvent(false);

                    try
                    {
                        IAsyncResult result = (IAsyncResult)httpWebRequest.BeginGetResponse(new AsyncCallback(ResultCallback), asyncRequestState);
                       
                        if (!asyncRequestState.allDone.WaitOne(timeout_ms))
                        {
                            if (httpWebRequest != null)
                            {
                                httpWebRequest.Abort();

                                ShowErrorMessage();
                            }
                        }

                    }
                    catch
                    {
                        ShowErrorMessage();
                    }
               }));
            }
            catch
            {
                ShowErrorMessage();
            }
        }

        private void ResultCallback(IAsyncResult asynchResult)
        {
            if (!asynchResult.IsCompleted)
                return;

            AsyncRequestState asyncRequestState = null;
            HttpWebResponse httpWebResponse = null;

            try
            {
                asyncRequestState = (AsyncRequestState)asynchResult.AsyncState;

                this.Page.Dispatcher.BeginInvoke(() =>
                {
                    PI.IsVisible = false;
                });

                //Thread.Sleep(15000);  // uncomment this line to test the timeout condition

                HttpWebRequest httpWebRequest = (HttpWebRequest)asyncRequestState.request;
                httpWebResponse = (HttpWebResponse)httpWebRequest.EndGetResponse(asynchResult);

                if (httpWebResponse.StatusCode != HttpStatusCode.OK)
                {
                    asyncRequestState.Success = false;
                    throw new WebException("EndGetResponse Status Code = " + httpWebResponse.StatusCode);
                }

                using (Stream responseStream = httpWebResponse.GetResponseStream())
                {
                    asyncRequestState.actionToPerform(responseStream);
                }


                this.Page.Dispatcher.BeginInvoke(() => 
                { 
                    MessageBoxResult mbResult = MessageBox.Show("Your pass code has been set", "Information", MessageBoxButton.OK);
                    if (mbResult == MessageBoxResult.OK)
                        (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/SettingsView.xaml?PassCodeSet=True", UriKind.Relative));
                });
            }
            catch
            {
                //if (asyncRequestState != null && asyncRequestState.request != null)
                //    ShowErrorMessage();
            }

            if (httpWebResponse != null)
                httpWebResponse.Close();

            asyncRequestState.allDone.Set();
        }

        #region Helpers
        public void XMLConsumption(Stream responseStream)
        {
            XDocument xdoc = XDocument.Load(responseStream);

            string query = (from d in xdoc.Root.Elements("Data")
                            select d.Element("Id").Value).First();

        }

        public void TextConsumption(Stream responseStream, FrameworkElement element)
        {
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(responseStream, encode);

            Char[] read = new Char[256];
            // Reads 256 characters at a time.     
            int count = readStream.Read(read, 0, 256);


            while (count > 0)
            {
                // Dumps the 256 characters on a string and displays the string to the console.
                String str = new String(read, 0, count);

                element.Dispatcher.BeginInvoke(() =>
                {
                    ((TextBlock)element).Text = str;

                });

                count = readStream.Read(read, 0, 256);
            }

        }

        private void ShowErrorMessage()
        {
            this.Page.Dispatcher.BeginInvoke(() =>
            {
                MessageBoxResult msgResult = MessageBox.Show("An error occurred connecting to the remote server", "Warning", MessageBoxButton.OK);
                if (msgResult == MessageBoxResult.OK)
                    this.Page.Opacity = 1;
            });
        }
        #endregion


    }
}
