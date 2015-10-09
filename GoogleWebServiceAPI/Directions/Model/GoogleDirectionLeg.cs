using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using GoogleWebServiceApi.Models;

namespace GoogleWebServiceApi.Directions.Model
{
    [DataContract]
    public class GoogleDirectionLeg
    {

        [DataMember(Name="steps")]
        public IList<GoogleDirectionStep> Steps { get; set; }

        /// <summary>
        /// The distance of this leg in meters
        /// </summary>
        [DataMember(Name = "distance")]
        public GoogleWebServiceTextValue Distance { get; set; }


        /// <summary>
        /// The duration of this leg in seconds
        /// </summary>
        [DataMember(Name = "duration")]
        public GoogleWebServiceTextValue Duration { get; set; }


        [DataMember(Name = "start_location")]
        public GoogleMapsLocation StartLocation { get; set; }



        [DataMember(Name = "end_location")]
        public GoogleMapsLocation EndLocation { get; set; }


        [DataMember(Name="start_address")]
        public string StartAddress { get; set; }

        [DataMember(Name="end_address")]
        public string EndAddress { get; set; }
    }

  
}
