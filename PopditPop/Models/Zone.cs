using System;

namespace PopditPop.Models
{ 
    // A zone in which bubbles can exist.
    public class Zone
    {
        public Zone(decimal latitude, decimal longitude, int radius)
        {
            Latitude = latitude;
            Longitude = longitude;
            Radius = radius;
            UpdateMaxMin();
        }

        public void UpdateMaxMin()
        {
            const decimal metersPerDegree = 111195m;
            decimal LatRadius = metersPerDegree / Radius;
            MaxLatitude = Latitude + LatRadius;
            MinLatitude = Latitude - LatRadius;
            decimal LongRadius = metersPerDegree / Radius * (decimal)Math.Cos((double)Latitude);
            MaxLongitude = Longitude + LongRadius;
            MinLongitude = Longitude - LongRadius;
        }

        private const decimal degree = 111195;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Radius { get; set; }
        public decimal MaxLatitude { get; set; }
        public decimal MinLatitude { get; set; }
        public decimal MaxLongitude { get; set; }
        public decimal MinLongitude { get; set; }
    }
}