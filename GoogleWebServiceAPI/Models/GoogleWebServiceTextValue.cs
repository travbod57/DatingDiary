using System.Runtime.Serialization;

namespace GoogleWebServiceApi.Models
{
    [DataContract]
	public class GoogleWebServiceTextValue
	{
        [DataMember(Name= "value")]
        public double Value { get; set; }

        [DataMember(Name= "text")]
        public string Text { get; set; }
	}
}