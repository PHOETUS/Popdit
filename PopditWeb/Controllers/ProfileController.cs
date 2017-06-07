using System.Collections.Generic;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;

namespace PopditWeb.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile/Index
        public async Task<ActionResult> Index()
        {
            Stream json = await WebApiGet("api/Profile");
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Models.Profile>));
            List<Models.Profile> profileList = (List<Models.Profile>)serializer.ReadObject(json);
            return View(profileList[0]); // Return the first - and presumably only - profile, so that the page can use a Profile, instead of a List, as a model.
        }

        // GET: Profile/Create
        public ActionResult Create() { return View(); }

        // POST: Profile/Create
        [HttpPost]
        public async Task<ActionResult> Create(Models.Profile pd)
        {
                Stream json = await WebApiPost("api/Profile/1", pd);
                return RedirectToAction("Index", "Home");
        }

        // POST: Profile/Update/5
        [HttpPost]
        public async Task<ActionResult> Update(Models.Profile pd)
        {
            Stream json = await WebApiPut("api/Profile/" + pd.Id.ToString(), pd);
            return RedirectToAction("Index");
        }
    }
}
