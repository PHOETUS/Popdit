using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PopditCore.Models;

namespace PopditCore.Controllers
{
    public class RadiusController : Controller
    {
        // GET: Radius 
        public ActionResult Index()
        {
            return Json(mContext.Radii.Select(e => new
            {
                e.Id,
                e.Description,
                e.Meters
            }), JsonRequestBehavior.AllowGet);
        }

        // GET: Radius/Read/5
        public ActionResult Read(int id)
        {
            Models.Radius r = mContext.Radii.Single(e => (e.Id == id));
            return Json(new
            {
                r.Id,
                r.Description,
                r.Meters
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
