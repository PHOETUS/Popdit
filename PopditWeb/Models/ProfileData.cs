using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PopditWeb.Models
{
    public class ProfileData
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Callback { get; set; }
    }
}