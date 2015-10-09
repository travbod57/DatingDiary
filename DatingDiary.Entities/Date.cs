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
    
    public partial class Date
    {
        public Date()
        {
            this.Photos = new HashSet<Photo>();
            this.Notes = new HashSet<Note>();
        }
    
        public int Id { get; set; }
        public System.DateTime DateOfMeeting { get; set; }
        public int PersonId { get; set; }
        public int VenueId { get; set; }
        public double Rating { get; set; }
        public Nullable<bool> IsFavourite { get; set; }
    
        public virtual Person Person { get; set; }
        public virtual Venue Venue { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}