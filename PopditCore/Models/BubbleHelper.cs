using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PopditCore.Models
{
    public partial class Bubble
    {
        private const decimal degree = 111195.0m;

        // Update max and min latitude and longitude
        public void UpdateBounds()
        {
            // Radius in meters.
            decimal m = Radius.Meters;
            // Radius in degrees latitude or longitude.
            decimal deltaLat = m / degree;
            decimal deltaLong = m / degree * (decimal)Math.Cos((double)Latitude);
            // Update
            MaxLatitude = Latitude + deltaLat;
            MinLatitude = Latitude = deltaLat;
            MaxLongitude = Longitude + deltaLong;
            MinLongitude = Longitude - deltaLong;
        }
    }
}