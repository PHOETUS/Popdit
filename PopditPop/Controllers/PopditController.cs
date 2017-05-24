using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PopditPop.Models;

namespace PopditPop.Controllers
{
    public class PopditController : Controller
    {
        protected PopditDBEntities mContext;

        public PopditController() { mContext = new PopditDBEntities(); }        
    }
}