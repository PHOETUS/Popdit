using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PopditCore.Models;

namespace PopditCore.Controllers
{
    public class FilterController : Controller
    {
        // POST: Filter/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Filter
        public ActionResult Index()
        {
            return Json(mContext.Filters.Select(r => new
            {
                r.Id,
                r.Name,
                r.ProfileId,
                r.Profile.Nickname,
                r.CategoryId,
                Category = r.Category.Description,
                r.ScheduleId,
                r.RadiusId,
                Radius = r.Radius.Description,
                r.Active
            }).OrderBy(r => r.Name), JsonRequestBehavior.AllowGet);
        }

        // GET: Filter/Read/5
        public ActionResult Read(int id)
        {
            Models.Filter r = mContext.Filters.Single(e => (e.Id == id));
            return Json(new
            {
                r.Id,
                r.Name,
                r.ProfileId,
                r.Profile.Nickname,
                r.CategoryId,
                Category = r.Category.Description,
                r.ScheduleId,
                r.RadiusId,
                Radius = r.Radius.Description,
                r.Active
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: Filter/Update/5
        [HttpPost]
        public ActionResult Update(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Filter/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
