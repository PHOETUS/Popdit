using System.ComponentModel.DataAnnotations;

namespace PopditWebApi
{
    using System;

    public partial class ProfileInterop
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Nickname { get; set; }
        [Required, StringLength(250)]
        public string Tagline { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only letters and digits allowed")]
        public string Password { get; set; }
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }
        [Url(ErrorMessage = "Invalid URL")]
        public string CallbackAddress { get; set; }
        public DateTime DOB { get; set; }
        public Nullable<bool> Male { get; set; }
        public string Flags { get; set; }
    }
}
