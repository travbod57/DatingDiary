using System;
using System.Runtime.Serialization;

namespace GoogleWebServiceApi.Models
{
	[DataContract]
	public abstract class GoogleWebServiceResponseBase
	{
		
		[DataMember(Name = "status")]
		public string _status;

		public GoogleMapsSearchStatus Status
		{
			get { return (GoogleMapsSearchStatus)Enum.Parse(typeof(GoogleMapsSearchStatus), _status, false); }
			private set { }

		}

	}
}