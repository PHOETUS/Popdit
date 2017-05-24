using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using PopditCore.Models;
using System.Diagnostics;

namespace PopditCore.Controllers
{
    public class ProfileController : Controller
    {
        public ProfileController() : base() { }

        // POST: Profile/Create
        [HttpPost]
        public ActionResult Create()
        {
            try
            {
                //TBD

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Profile/Read/5
        public ActionResult Read(int id)
        {
            Profile r = mContext.Profiles.Single(e => (e.Id == id));
            // Password is deliberately omitted below.
            return Json(new {
                r.Id,
                r.Nickname,
                r.Email,
                r.Phone,
                r.CallbackAddress,
                DOB = r.DOB.GetValueOrDefault().ToString("d"),
                Male = r.Male,
                r.RadiusId,
                r.Radius.Description
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: Profile/Update
        [HttpPost]
        public ActionResult Update()
        {
            // Parse JSON from request.
            Request.InputStream.Position = 0;
            string json = new System.IO.StreamReader(Request.InputStream).ReadToEnd();
            JObject j = JObject.Parse(json);

            // Get Profile.
            int id = Int32.Parse(j["Id"].ToString());
            Profile pro = mContext.Profiles.Single(p => p.Id == id);

            // Update Profile.
            pro.Phone = (string)j["Phone"] ?? ""; // Unsettable
            pro.Password = (string)j["Password"] ?? pro.Password;
            pro.Nickname = (string)j["Nickname"] ?? pro.Nickname;
            pro.Email = (string)j["Email"] ?? pro.Email;
            pro.CallbackAddress = (string)j["Callback"] ?? ""; // Unsettable

            // Save changes.
            mContext.SaveChanges();

            // Return status.
            return Json(new
            {
                STATUS = 0
            }, JsonRequestBehavior.AllowGet);
        }

        // GET: Profile/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
