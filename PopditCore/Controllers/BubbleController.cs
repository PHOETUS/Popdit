using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PopditCore.Models;

namespace PopditCore.Controllers
{
    public class BubbleController : Controller
    {
        // POST: Bubble/Create
        [HttpPost]
        public ActionResult Create()
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Read");
            }
            catch
            {
                return View();
            }
        }

        // GET: Bubble
        public ActionResult Index()
        {
            return Json(mContext.Bubbles.Select(r => new
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

        // GET: Bubble/Read/5
        public ActionResult Read(int id)
        {
            Bubble r = mContext.Bubbles.Single(e => (e.Id == id));

            return Json(new
            {
                r.Id,
                r.Name,
                r.CategoryId,
                r.Category.Description,
                r.AddressId,
                r.Street1,
                r.Street2,
                r.CitySateZip,
                r.Country,
                r.Latitude,
                r.Longitude,
                r.RadiusId,
                Radius = r.Radius.Description,
                r.AlertMsg,
                r.ProfileId,
                r.Profile.Nickname,
                r.ScheduleId,
                r.Active
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: Trip/Edit/5
        [HttpPost]
        public ActionResult Update(int id)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Read");
            }
            catch
            {
                return View();
            }
        }

        // GET: Trip/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
