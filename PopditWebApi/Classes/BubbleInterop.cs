using System.ComponentModel.DataAnnotations;

namespace PopditWebApi
{
    public partial class BubbleInterop
    {
        public int Id { get; set; }
        public int InternalId { get; set; }
        public int ProfileId { get; set; }     
        [MaxLength(50, ErrorMessage = "Maximum length 50 characters")]
        public string Name { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.000000}")]
        public double Latitude { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.000000}")]
        public double Longitude { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Valid address required"), MaxLength(200, ErrorMessage = "Maximum length 200 characters")]
        public string Address;
        [DataType(DataType.MultilineText), MaxLength(1000, ErrorMessage = "Maximum length 1000 characters")]
        public string AlertMsg { get; set; }
        public int RadiusId { get; set; }
        public bool Active { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }
        [DataType(DataType.Url, ErrorMessage = "Invalid URL"), MaxLength(250, ErrorMessage = "Maximum length 250 characters")]
        public string Url { get; set; }
        public int Pops { get; set; }
    }
}
