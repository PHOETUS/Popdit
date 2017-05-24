using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PopditCore.Models;

namespace PopditCore.Controllers
{
    public class CountryController : Controller
    {
        // GET: Country 
        public ActionResult Index()
        {
            return Json(mContext.Countries.Select(r => new
            {
                r.Id,
                r.Name
            }), JsonRequestBehavior.AllowGet);
        }

        // GET: Country/Read/5
        public ActionResult Read(int id)
        {
            Models.Country r = mContext.Countries.Single(e => (e.Id == id));
            return Json(new
            {
                r.Id,
                r.Name
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
