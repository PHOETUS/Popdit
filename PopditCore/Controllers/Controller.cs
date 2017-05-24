using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PopditCore.Models;

namespace PopditCore.Controllers
{
    public class Controller : System.Web.Mvc.Controller
    {
        protected PopditDBEntities mContext;

        public Controller() { mContext = new PopditDBEntities(); }        
    }
}