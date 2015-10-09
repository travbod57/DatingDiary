using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GoogleWebServiceApi.Models
{
    [DataContract]
    public class GoogleMapAddressComponent
    {
        [DataMember(Name="long_name")]
        public string LongName { get; set; }

        [DataMember(Name="short_name")]
        public string ShortName { get; set; }



        [DataMember(Name = "types")]
        public string[] _types;

        public IList<GoogleMapsAddressComponentTypes> Types
        {
            get
            {

            	var output = new List<GoogleMapsAddressComponentTypes>();
				foreach(var k in _types)
				{
					try
					{
						output.Add( (GoogleMapsAddressComponentTypes)Enum.Parse(typeof(GoogleMapsAddressComponentTypes), k, false));

					}
					catch (Exception)
					{
						
					}
				}
            	return output;


            }
            private set { }

        }
    }
}