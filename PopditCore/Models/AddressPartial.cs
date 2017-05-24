using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace PopditCore.Models
{
    public partial class Address
    {
        public static String Print(Address a)
        {
            if (a == null) { return ""; }
            String s = "";
            if (a.Street1 != null)      { s = s + a.Street1; }
            if (a.Street2 != null)      { s = s + "\n" + a.Street2; }
            if (a.City != null)         { s = s + "\n" + a.City; }
            if (a.StateId != null)      { s = s + ", " + a.State.Name; }
            if (a.Zip != null)          { s = s + " " + a.Zip; }
            if (a.CountryId != null)    { s = s + "\n" + a.Country.Name; }
            s.Trim();
            return s;
        }
    }
}