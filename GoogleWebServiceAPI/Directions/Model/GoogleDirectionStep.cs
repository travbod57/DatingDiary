using System.Collections.Generic;
using System.Runtime.Serialization;
using GoogleWebServiceApi.Models;

namespace GoogleWebServiceApi.Directions.Model
{
    [DataContract]
    public class GoogleDirectionStep
    {
        [DataMember(Name="html_instructions")]
        public string HtmlInstructions { get; set; }

        /// <summary>
        /// The distance of this step in meters
        /// </summary>
        [DataMember(Name = "distance")]
        public GoogleWebServiceTextValue Distance { get; set; }
        /// <summary>
        /// The duration of this leg in seconds
        /// </summary>
        [DataMember(Name = "duration")]
        public GoogleWebServiceTextValue Duration { get; set; }

        [DataMember(Name = "polyline")]
        public GoogleMapsPolyline Polyline { get; set; }


    }

}