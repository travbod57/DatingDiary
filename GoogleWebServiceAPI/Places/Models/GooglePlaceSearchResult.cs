using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GoogleWebServiceApi.Models;

namespace GoogleWebServiceApi.Places.Models
{
	[DataContract]
	public class GooglePlaceSearchResult
	{

        [DataMember(Name= "geometry")]
        public GoogleMapsGeometry Geometry { get; set; }

        [DataMember(Name="icon")]
		public string Icon { get;  set; }

		[DataMember(Name="id")]
		public string Id { get;  set; }

        [DataMember(Name="name")]
        public string Name { get; set; }

		[DataMember(Name="rating")]
		public Single Rating { get;  set; }

		[DataMember(Name="reference")]
		public string Reference { get;  set; }

		[DataMember(Name="types")]
		public List<string> _types;

        [DataMember(Name = "formatted_address")]
        public string Formatted_Address { get; set; }

		public List<GooglePlaceSearchType> Types
		{
			get
			{
				if (_types == null) return null;
				return _types.Select(k => (GooglePlaceSearchType) Enum.Parse(typeof (GooglePlaceSearchType), k, false)).ToList();
			}
			 set { }
		}

		[DataMember(Name="vicinity")]
		public string Vicinity { get;  set; }
	}
}
