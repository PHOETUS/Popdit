//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PopditWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class Event
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]        
        public int ProfileId { get; set; }
        [DataMember]
        public int TripId { get; set; }
        [DataMember]
        public System.DateTime Timestamp { get; set; }

        public virtual Bubble Bubble { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
