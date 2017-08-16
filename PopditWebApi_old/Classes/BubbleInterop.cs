using System.ComponentModel.DataAnnotations;

namespace PopditWebApi
{
    public partial class BubbleInterop
    {
        public int Id { get; set; }
        public int InternalId { get; set; }
        public int ProfileId { get; set; }
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only letters and digits allowed")]
        public string Name { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.000000}")]
        public double Latitude { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.000000}")]
        public double Longitude { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Valid address required")]
        public string Address;
        [DataType(DataType.MultilineText)]
        public string AlertMsg { get; set; }
        public int RadiusId { get; set; }
        public bool Active { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }
        [DataType(DataType.Url, ErrorMessage = "Invalid URL")]
        public string Url { get; set; }
    }
}
