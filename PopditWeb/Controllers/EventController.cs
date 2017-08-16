using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using PopditWebApi;

namespace PopditWeb.Controllers
{
    public class EventController : Controller
    {
        // GET: Event
        public async Task<ActionResult> Index()
        {
            try
            {
                string json = await WebApi(WebApiMethod.Get, "api/Event");
                List<EventInterop> eventList = JsonConvert.DeserializeObject<List<EventInterop>>(json);
                return View(eventList);
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("SignOut", "Home"); }
        }
    }
}
