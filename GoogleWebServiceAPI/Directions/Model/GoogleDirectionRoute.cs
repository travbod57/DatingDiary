using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using GoogleWebServiceApi.Models;

namespace GoogleWebServiceApi.Directions.Model
{
    [DataContract]
    public class GoogleDirectionRoute
    {
        [DataMember(Name="summary")]
        public string Summary { get; set; }

        [DataMember(Name="legs")]
        public IList<GoogleDirectionLeg> Legs { get; set; }

        [DataMember(Name="waypoint_order")]
        public IList<int> WaypointOrder { get; set; }

        [DataMember(Name="bounds")]
        public GoogleMapsBound Bounds { get; set; }
    }
}
