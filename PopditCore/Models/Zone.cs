using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PopditCore.Models
{
    // A zone in which bubbles can exist.
    [DataContract]
    public class Zone
    {
        private const decimal degree = 111195;
        [DataMember]
        public decimal Latitude { get; set; }
        [DataMember]
        public decimal Longitude { get; set; }
        [DataMember]
        public int Radius { get; set; }
        private decimal RadiusLat {  get { return Radius / degree; } }
        private decimal RadiusLong {  get { return Radius / degree; } }
        public decimal MaxLatitude {  get { return Latitude + RadiusLat;  } }
        public decimal MinLatitude { get { return Latitude - RadiusLat; } }
        public decimal MaxLongitude { get { return Longitude + RadiusLong; } }
        public decimal MinLongitude { get { return Longitude - RadiusLong; } }
    }
}