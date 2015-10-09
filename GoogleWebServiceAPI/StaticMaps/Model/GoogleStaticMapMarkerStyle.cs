using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Media;
using GoogleWebServiceApi.StaticMaps.Exceptions;

namespace GoogleWebServiceApi.StaticMaps.Model
{
    public class GoogleStaticMapMarkerStyle
    {

        public  const string PipeEncoded = "%7C";
        /// <summary>
        /// the url of the image, URL encoded
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// (default true) indicates that the Static Maps service should construct an appropriate shadow for the image. This shadow is based on the image's visible region and its opacity/transparency.
        /// </summary>
        public bool Shadow { get; set; }

        private char? _label;
        /// <summary>
        /// defines the label of the marker
        /// </summary>
        /// <remarks>
        /// Label can only be [a-z] or [0-9], if the MarkerSize is set to small or tiny, then a label cannot be defined. In this case a CannotAddLabelToMarkerException will tbe thrown.
        /// Please refer to https://developers.google.com/maps/documentation/staticmaps/#MarkerStyles
        /// </remarks>
        public char? Label
        {
            get { return _label; }
            set
            {
                if (!value.HasValue)
                {
                    _label = value;
                    return;
                }
                if (!Regex.Match(value.Value.ToString(), "[0-9]|[A-Z]").Success)
                    throw new CannotAddLabelToMarkerException("Invalid character");
                if (MarkerSize.HasValue && (MarkerSize.Value == GoogleStaticMapMarkerSize.small || MarkerSize.Value == GoogleStaticMapMarkerSize.tiny))
                    throw new CannotAddLabelToMarkerException();
                _label = value;
            }
        }


        /// <summary>
        /// size of the marker
        /// </summary>
        public GoogleStaticMapMarkerSize? MarkerSize { get; set; }

        public string MarkerColorString
        {

            get
            {
                if (_markerColor == null)
                    return null;
                return string.Format("0x{0:X2}{1:X2}{2:X2}", _markerColor.Value.R, _markerColor.Value.G, _markerColor.Value.B);
            }
        }

        /// <summary>
        /// the color of the maker
        /// </summary>
        private Color? _markerColor;
        public Color? MarkerColor
        {
            set { _markerColor = value; }
        }



        /// <summary>
        /// Generate the style param 
        /// </summary>
        /// <returns></returns>
        public string CreateStyleParameter()
        {

            var parameters = new List<string>();
            if (!string.IsNullOrEmpty(ImageUrl))
                parameters.Add(string.Concat("icon:", ImageUrl));
            if (MarkerSize.HasValue)
                parameters.Add(string.Concat("size:", Enum.GetName(typeof(GoogleStaticMapMarkerSize), MarkerSize.Value)));
            if (Label.HasValue)
                parameters.Add(string.Concat("label:", Label));
            if (!Shadow)
                parameters.Add("shadow:false");
            if (!string.IsNullOrEmpty(MarkerColorString))
                parameters.Add(string.Concat("color:", MarkerColorString));

            return "&markers="+ string.Join(PipeEncoded, parameters);
        }

    }
}