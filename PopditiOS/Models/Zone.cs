namespace PopditiOS.Models
{
    // A zone in which bubbles can exist.
    public class Zone
    {
        public Zone(decimal latitude, decimal longitude, int radius)
        {
            Latitude = latitude;
            Longitude = longitude;
            Radius = radius;
        }

        private const decimal degree = 111195;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Radius { get; set; }
        private decimal RadiusLat {  get { return Radius / degree; } }
        private decimal RadiusLong {  get { return Radius / degree; } }
        public decimal MaxLatitude {  get { return Latitude + RadiusLat;  } }
        public decimal MinLatitude { get { return Latitude - RadiusLat; } }
        public decimal MaxLongitude { get { return Longitude + RadiusLong; } }
        public decimal MinLongitude { get { return Longitude - RadiusLong; } }
    }
}