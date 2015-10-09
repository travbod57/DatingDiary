using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatingDiary.Other
{
    public class IsolatedDatabaseCall
    {
        public delegate void CallCompletedEventHandler(object sender, EventArgs e);

        public event CallCompletedEventHandler CallCompleted;

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnChanged(EventArgs e)
        {
            if (CallCompleted != null)
                CallCompleted(this, e);
        }

        public void FetchData(Func<bool> makeCall)
        {
            bool completed = makeCall();

            if (completed)
                CallCompleted(null, EventArgs.Empty);
        }
    }
}
