using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PopditCore.Models;

namespace PopditCore.Controllers
{
    public class AddressController : Controller
    {
        public AddressController() : base() { }

        // POST: Address/Create
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

        // GET: Address/Read/5
        public ActionResult Read(int id)
        {
            Address r = mContext.Addresses.Single(e => (e.Id == id));
            return Json(new {
                r.Id,
                r.Street1,
                r.Street2,
                r.City,
                r.StateId,
                State = r.State.Name,
                r.Zip,
                r.CountryId,
                Country = r.Country.Name
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: Address/Update
        [HttpPost]
        public ActionResult Update()
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

        // GET: Address/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
