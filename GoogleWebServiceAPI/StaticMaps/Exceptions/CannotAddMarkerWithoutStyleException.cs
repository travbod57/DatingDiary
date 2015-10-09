using System;

namespace GoogleWebServiceApi.StaticMaps.Exceptions
{
    public class CannotAddMarkerWithoutStyleException: Exception
    {
        public CannotAddMarkerWithoutStyleException() :base("Cannot add a marker without style")
        {
            
        }
    }
}