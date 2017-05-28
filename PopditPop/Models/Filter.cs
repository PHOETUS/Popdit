//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PopditPop.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class Filter
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int ProfileId { get; set; }
        [DataMember]
        public Nullable<int> CategoryId { get; set; }
        [DataMember]
        public Nullable<int> ScheduleId { get; set; }
        [DataMember]
        public int RadiusId { get; set; }
        [DataMember]
        public bool Active { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual Radius Radius { get; set; }
    }
}
