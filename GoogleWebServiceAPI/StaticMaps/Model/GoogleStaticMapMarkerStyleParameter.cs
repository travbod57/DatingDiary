using System;
using System.Collections.Generic;
using System.Linq;
using GoogleWebServiceApi.StaticMaps.Exceptions;

namespace GoogleWebServiceApi.StaticMaps.Model
{
    /// <summary>
    /// represents the parker parameter of Google Static API
    /// </summary>
    public class GoogleStaticMapMarkerStyleParameter
    {
        public GoogleStaticMapMarkerStyleParameter()
        {
            MarkerStyles = new Dictionary<GoogleStaticMapMarker, GoogleStaticMapMarkerStyle>();

        }


        public Dictionary<GoogleStaticMapMarker, GoogleStaticMapMarkerStyle> MarkerStyles { get; set; }

        /// <summary>
        /// Add a marker, along with its style
        /// </summary>
        /// <param name="marker"></param>
        /// <param name="style"></param>
        public void AddMarker(GoogleStaticMapMarker marker, GoogleStaticMapMarkerStyle style)
        {
            if (style == null) throw new CannotAddMarkerWithoutStyleException();
            MarkerStyles.Add(marker, style);
        }

        /// <summary>
        /// remove a marker
        /// </summary>
        /// <param name="marker"></param>
        public void RemoveMarker(GoogleStaticMapMarker marker)
        {
            MarkerStyles.Remove(marker);
        }


        /// <summary>
        /// Build the marker parameters based on the styles and the marker locations
        /// </summary>
        /// <returns></returns>
        public string BuildMarkerParameter()
        {
            var uniqueStyles = MarkerStyles.Values.Distinct();
            //create a style->Marker dictionary
            var parameters = uniqueStyles.ToDictionary(c => c,
                                                       c => MarkerStyles.Where(d => d.Value == c).Select(e => e.Key));

            var output = "";
            //now go through each style, generate the param for it
            var allMarkerParameters = new List<string>();
            foreach(var  k in parameters)
            {
                
                //generate the style
                var style = k.Key.CreateStyleParameter();
                var markerParameter = string.Concat( style, GoogleStaticMapMarkerStyle.PipeEncoded);

                //get all the location of the style
                var markerLocations = k.Value.Select(
                    delegate(GoogleStaticMapMarker m)
                        {
                            if (!string.IsNullOrEmpty(m.MarkerAddress))
                                return m.MarkerAddress;
                            return m.MarkerCoordinate.Latitude + "," + m.MarkerCoordinate.Longitude;
                        }
                    );
                //concat the locations to the parameter
                markerParameter = string.Concat(markerParameter,
                                                string.Join(GoogleStaticMapMarkerStyle.PipeEncoded, markerLocations));
                output = string.Concat(output, markerParameter);

            }
            return output;

        }
    }
}