using System.ComponentModel.DataAnnotations;

namespace PopditWebApi
{
    public partial class BubbleInterop
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.000000}")]
        public double Latitude { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.000000}")]
        public double Longitude { get; set; }
        public string Address;
        [DataType(DataType.MultilineText)]
        public string AlertMsg { get; set; }
        public int RadiusId { get; set; }
        public int ScheduleId { get; set; }
        public bool Active { get; set; }
        public string Phone { get; set; }
        public string Url { get; set; }
    }
}
