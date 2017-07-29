using System;

namespace PopditPop.Models
{ 
    // A zone in which bubbles can exist.
    public class Zone
    {
        public Zone(double latitude, double longitude, int radius)
        {
            Latitude = latitude;
            Longitude = longitude;
            Radius = radius;
            UpdateMaxMin();
        }

        public void UpdateMaxMin()
        {
            const double metersPerDegree = 111195;
            double LatRadius = metersPerDegree / Radius;
            MaxLatitude = Latitude + LatRadius;
            MinLatitude = Latitude - LatRadius;
            double LongRadius = metersPerDegree / Radius * (double)Math.Cos((double)Latitude);
            MaxLongitude = Longitude + LongRadius;
            MinLongitude = Longitude - LongRadius;
        }

        private const double degree = 111195;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Radius { get; set; }
        public double MaxLatitude { get; set; }
        public double MinLatitude { get; set; }
        public double MaxLongitude { get; set; }
        public double MinLongitude { get; set; }
    }
}