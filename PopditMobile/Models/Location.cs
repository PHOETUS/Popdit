namespace PopditMobile.Models
{
    public class Location
    {
        public decimal Latitude;
        public decimal Longitude;

        public Location(decimal Lat, decimal Long)
        {
            Latitude = Lat;
            Longitude = Long;
        }
    }
}
