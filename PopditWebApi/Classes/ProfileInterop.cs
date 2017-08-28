using System.ComponentModel.DataAnnotations;

namespace PopditWebApi
{
    using System;

    public partial class ProfileInterop
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Nickname { get; set; }
        [Required, StringLength(120), DataType(DataType.MultilineText)]
        public string Tagline { get; set; }
        [Required, EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required, RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only letters and digits allowed")]
        public string Password { get; set; }
        [Required, Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }
        [Url(ErrorMessage = "Invalid URL")]
        public string CallbackAddress { get; set; }
        public DateTime DOB { get; set; }
        public Nullable<bool> Male { get; set; }
        public string Flags { get; set; }
    }
}
