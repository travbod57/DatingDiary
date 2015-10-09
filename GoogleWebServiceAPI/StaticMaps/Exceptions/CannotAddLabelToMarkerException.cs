using System;

namespace GoogleWebServiceApi.StaticMaps.Exceptions
{
    public class CannotAddLabelToMarkerException : Exception
    {
        public CannotAddLabelToMarkerException()
            : base("Cannot add label to marker because the marker has size tiny or small ")
        {
            {


            }
        }


        public CannotAddLabelToMarkerException(string message):
            base(message)
        {
            
        }
    }
}