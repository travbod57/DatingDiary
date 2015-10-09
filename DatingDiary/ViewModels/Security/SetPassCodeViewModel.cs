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
using Microsoft.Phone.Controls;
using DatingDiary.Async;
using System.Net.NetworkInformation;
using System.Threading;
using DatingDiary.Views.Security;
using DatingDiary.Other;
using Microsoft.Phone.Shell;

namespace DatingDiary.ViewModels.Security
{
    public class SetPassCodeViewModel : ViewModelBase
    {


        public SetPassCodeViewModel(PhoneApplicationPage page) : base(page)
        {

        }

        public void RegisterCodeForRecovery(string email, ProgressIndicator pi)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                AsyncRequest async = new AsyncRequest(pi, this.Page);
                AsyncRequestState asyncRequestState = new AsyncRequestState(async.XMLConsumption);
                async.Invoke(5000, asyncRequestState, string.Format("http://www.thinkbackpacking.com/DatingDiary/Register.php?email={0}&pin={1}", email, ((SetPassCode)Page).txtRetype.Text));

                //return new SetPassCodeResult() { Success = asyncRequestState.success, Message = asyncRequestState.success ? "Your pass code has been set" : "There was an error in connecting the remote server. Your pass code has not been set" };
            }
            else
                MessageBox.Show("You do not have any connectivity to complete the operation at this time", "Information", MessageBoxButton.OK);
                //return new SetPassCodeResult() { Success = false, Message = "You do not have any connectivity to complete the operation at this time" };
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
