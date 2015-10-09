using System;
using GoogleWebServiceApi.Models;

namespace GoogleWebServiceApi.PlacesAutocomplete.Models
{
    public class GooglePlaceAutoCompleteRequest
    {
        public GooglePlaceAutoCompleteRequest(string input, bool sensor)
        {
            Input = input;
            Sensor = sensor;
        }


        public string Input { get; set; }
        public bool Sensor { get; set; }
        public int Offset { get; set; }
        public double? LocationLat { get; set; }
        public double? LocationLong { get; set; }
        
        public double Radius {get;set;}
        public GoogleMapsLanguage? Language { get; set; }
        public GoogleMapAutoCompletePlaceType? _placeType;
        public string PlaceType { 
            get{
                if (!_placeType.HasValue) return null;
                return Enum.GetName(typeof(GoogleMapAutoCompletePlaceType), _placeType);
            }
        }
        

    }
}
