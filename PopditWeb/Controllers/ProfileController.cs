using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using System.Net;
using System.Runtime.Serialization.Json;

namespace PopditWeb.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile/Update/5
        public ActionResult Update(int id)
        {
            InitializeList("Profile/Read/" + id.ToString(), null);
            JObject profile = (JObject)mObjectList[0];
            Models.ProfileData pd = new Models.ProfileData();
            pd.Id = Int32.Parse(profile["Id"].ToString());
            pd.Phone = (string)profile["Phone"];
            pd.Nickname = (string)profile["Nickname"];
            pd.Email = (string)profile["Email"];
            pd.Callback = (string)profile["CallbackAddress"];

            return View(pd);
        }

        // POST: Profile/Update/5
        [HttpPost]
        public ActionResult Update(Models.ProfileData pd)
        {
            try
            {
                string json = JsonConvert.SerializeObject(pd);
                InitializeList("Profile/Update", json);
                return RedirectToAction("Update");
            }
            catch
            {
                return View();
            }
        }
    }
}
