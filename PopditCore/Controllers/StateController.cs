using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PopditCore.Models;

namespace PopditCore.Controllers
{
    public class StateController : Controller
    {
        // GET: State 
        public ActionResult Index()
        {
            return Json(mContext.States.Select(r => new
            {
                r.Id,
                r.Name,
                r.Code
            }), JsonRequestBehavior.AllowGet);
        }

        // GET: State/Read/5
        public ActionResult Read(int id)
        {
            Models.State r = mContext.States.Single(e => (e.Id == id));
            return Json(new
            {
                r.Id,
                r.Name,
                r.Code
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
