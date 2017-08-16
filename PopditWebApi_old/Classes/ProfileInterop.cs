using System.ComponentModel.DataAnnotations;

namespace PopditWebApi
{
    using System;

    public partial class ProfileInterop
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nickname required")]
        public string Nickname { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only letters and digits allowed")]
        public string Password { get; set; }
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }
        [Url(ErrorMessage = "Invalid URL")]
        public string CallbackAddress { get; set; }
        /*
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date")]
        public string DobJson
        {
            get { return String.Format("{0:yyyy-MM-dd}", DOB); }
            set
            {
                try { DOB = DateTime.Parse(value); }
                catch { DOB = null; }
            }
        }*/
        public DateTime DOB { get; set; }
        //private Nullable<System.DateTime> DOB { get; set; }
        public Nullable<bool> Male { get; set; }
        public string Flags { get; set; }
    }
}
