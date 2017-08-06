using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System;
using PopditWebApi;

namespace PopditWeb.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile/Index
        public async Task<ActionResult> Index()
        {
            try
            {
                Stream json = await WebApi(WebApiMethod.Get, "api/Profile");
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<ProfileInterop>));
                List<ProfileInterop> profileList = (List<ProfileInterop>)serializer.ReadObject(json);
                return View(profileList[0]); // Return the first - and presumably only - profile, so that the page can use a Profile, instead of a List, as a model.
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("Index", "Home"); }
        }

        // GET: Profile/Create
        public ActionResult Create() { return View(); }

        // POST: Profile/Create
        [HttpPost]
        public async Task<ActionResult> Create(ProfileInterop pd)
        {
            try
            {
                Stream json = await WebApi(WebApiMethod.Post, "api/Profile/1", pd);
                return RedirectToAction("Index", "Home");
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("Index", "Home"); }
        }

        // POST: Profile/Update/5
        [HttpPost]
        public async Task<ActionResult> Update(ProfileInterop pd)
        {
            try
            {
                Stream json = await WebApi(WebApiMethod.Put, "api/Profile/" + pd.Id.ToString(), pd);
                return RedirectToAction("Index");
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("Index", "Home"); }
        }
    }
}

