using CoreLocation;

namespace PopditiOS.Models
{
    public partial class BubbleMobile
    { 
        public CLCircularRegion Region { get; set; } // Added for mobile app.
        public int Id { get; set; }
        //public int ProfileId { get; set; }
        //public string Name { get; set; }
        //public int CategoryId { get; set; }
        public decimal Latitude { get; set; }
        //public Nullable<decimal> MinLatitude { get; set; }
        //public Nullable<decimal> MaxLatitude { get; set; }
        public decimal Longitude { get; set; }
        //public Nullable<decimal> MaxLongitude { get; set; }
        //public Nullable<decimal> MinLongitude { get; set; }
        //public Nullable<int> AddressId { get; set; }
        //public string AlertMsg { get; set; }
        public int Radius { get; set; }
        //public int ScheduleId { get; set; }
        //public bool Active { get; set; }
    }
}