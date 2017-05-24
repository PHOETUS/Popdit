using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PopditCore.Models
{
    public partial class Bubble
    {
        public String Street1
        {
            get
            {
                if (this.Address == null) return "";
                else return this.Address.Street1;
            }
        }

        public String Street2
        {
            get
            {
                if (this.Address == null) return "";
                else return this.Address.Street2;
            }
        }

        public String CitySateZip
        {
            get
            { 
                Address a = this.Address;
                if (a == null) { return ""; }
                String s = "";
                if (a.City != null) { s = s + a.City; }
                if (a.StateId != null) { s = s + ", " + a.State.Name; }
                if (a.Zip != null) { s = s + " " + a.Zip; }
                s.Trim();
                return s;
            }
        }

        public String Country
        {
            get
            {
                if (this.Address == null) return "";
                if (this.Address.Country == null) return "";
                return this.Address.Country.Name;
            }
        }
    }
}