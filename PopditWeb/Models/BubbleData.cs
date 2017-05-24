using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PopditWeb.Models
{
    public class BubbleData
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int AddressId { get; set; }
        public string AlertMsg { get; set; }
        public int RadiusId { get; set; }
        public bool Active { get; set; }
    }
}