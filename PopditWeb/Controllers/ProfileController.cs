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
        /*
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
        }*/

        // GET: Profile/Index
        public async Task<ActionResult> Index()
        {
            Stream json = await WebApiGet("api/Profile");
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Models.Profile>));
            List<Models.Profile> profileList = (List<Models.Profile>)serializer.ReadObject(json);
            return View(profileList[0]); // Return the first - and presumably only - profile, so that the page can use a Profile, instead of a List, as a model.
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
