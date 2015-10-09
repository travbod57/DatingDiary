using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GoogleWebServiceApi.Directions.Model
{
    [DataContract]
    public class GoogleDirectionResponse
    {



        [DataMember(Name="status")]
        public string _status;


        public GoogleDirectionResponseStatusCode Status
        {
            get
            {
                return
                    (GoogleDirectionResponseStatusCode)
                    Enum.Parse(typeof (GoogleDirectionResponseStatusCode), _status, false);

            }
            private set { }
        }

        [DataMember(Name="routes")]
        public IList<GoogleDirectionRoute> Routes { get; set; }



    }
}
