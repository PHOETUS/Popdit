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
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class Profile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Profile()
        {
            this.Bubbles = new HashSet<Bubble>();
            this.Events = new HashSet<Event>();
            this.Filters = new HashSet<Filter>();
        }
    
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Nickname { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string CallbackAddress { get; set; }
        [DataMember]
        public Nullable<int> RadiusId { get; set; }
        [DataMember]
        public string DobJson
        {
            get { return String.Format("{0:yyyy-MM-dd HH:mm:ss}", DOB); }
            set { DOB = DateTime.Parse(value); }
        }
        public Nullable<System.DateTime> DOB { get; set; }
        [DataMember]
        public Nullable<bool> Male { get; set; }
        public System.DateTime LastSignIn { get; set; }
        public System.DateTime Created { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bubble> Bubbles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Event> Events { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Filter> Filters { get; set; }
        public virtual Radius Radius { get; set; }
    }
}
