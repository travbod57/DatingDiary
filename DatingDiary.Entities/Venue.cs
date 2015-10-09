//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatingDiary.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Venue
    {
        public Venue()
        {
            this.Dates = new HashSet<Date>();
        }
    
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
        public string Address { get; set; }
        public Nullable<bool> IsFavourite { get; set; }
    
        public virtual ICollection<Date> Dates { get; set; }
    }
}
