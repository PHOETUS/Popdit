using System;

namespace PopditDB.Models
{
    partial class Bubble
    {
        public void UpdateMaxMin()
        {
            const decimal metersPerDegree = 111195m;
            decimal LatRadius = Radius.Meters / metersPerDegree;
            MaxLatitude = Latitude + LatRadius;
            MinLatitude = Latitude - LatRadius;
            decimal LongRadius = Radius.Meters / metersPerDegree * (decimal)Math.Abs(Math.Cos((double)Latitude));
            MaxLongitude = Longitude + LongRadius;
            MinLongitude = Longitude - LongRadius;
        }
    }
}
