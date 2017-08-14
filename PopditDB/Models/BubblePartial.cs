using System;

namespace PopditDB.Models
{
    partial class Bubble : IEquatable<Bubble>
    {
        public void UpdateMaxMin()
        {
            const double metersPerDegree = 111195;
            double LatRadius = Radius.Meters / metersPerDegree;
            MaxLatitude = Latitude + LatRadius;
            MinLatitude = Latitude - LatRadius;
            double LongRadius = Radius.Meters / metersPerDegree * Math.Abs(Math.Cos(Math.PI / 180 * Latitude));
            MaxLongitude = Longitude + LongRadius;
            MinLongitude = Longitude - LongRadius;
        }

        public bool Equals(Bubble other)
        {
            return (this.Id == other.Id);
        }
    }
}
