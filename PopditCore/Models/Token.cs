using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PopditWeb.Models
{
    public class Token
    {
        public string Phone { get; set; }
        // TBD - security info here, e.g., the OAuth token.
    }
}