using System;

namespace DBLayer.Models
{
    partial class Bubble
    {
        public void UpdateMaxMin()
        {
            const decimal metersPerDegree = 111195m;
            decimal LatRadius = metersPerDegree / Radius.Meters;
            MaxLatitude = Latitude + LatRadius;
            MinLatitude = Latitude - LatRadius;
            decimal LongRadius = metersPerDegree / Radius.Meters * (decimal)Math.Cos((double)Latitude);
            MaxLongitude = Longitude + LongRadius;
            MinLongitude = Longitude - LongRadius;
        }
    }
}
