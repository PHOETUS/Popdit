using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PopditWeb.Models
{
    public class FilterData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProfileId { get; set; }
        public string Nickname { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public int ScheduleId { get; set; }
        public int RadiusId { get; set; }
        public string Radius { get; set; }
        public bool Active { get; set; }
    }
}