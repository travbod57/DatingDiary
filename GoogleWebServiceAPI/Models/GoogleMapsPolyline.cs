using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GoogleWebServiceApi.Models
{
    [DataContract]
    public class GoogleMapsPolyline
    {
        [DataMember(Name = "points")] public string _points;

        public IList<GoogleMapsLocation> Points
        {
            get { return DecodePolylinePoints(_points); }


        }



        private List<GoogleMapsLocation> DecodePolylinePoints(string encodedPoints)
        {
            if (encodedPoints == null || encodedPoints == "") return null;
            List<GoogleMapsLocation> poly = new List<GoogleMapsLocation>();
            char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            try
            {
                while (index < polylinechars.Length)
                {
                    // calculate next latitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int) polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length)
                        break;

                    currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                    //calculate next longitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int) polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length && next5bits >= 32)
                        break;

                    currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                    var p = new GoogleMapsLocation();
                    p.Latitude = Convert.ToDouble(currentLat)/100000.0;
                    p.Longitude = Convert.ToDouble(currentLng)/100000.0;
                    poly.Add(p);
                }
            }
            catch (Exception ex)
            {
                // logo it
            }
            return poly;
        }
    }
}
