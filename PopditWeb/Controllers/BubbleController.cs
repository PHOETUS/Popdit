using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;

namespace PopditWeb.Controllers
{
    public class BubbleController : Controller
    {
        // GET: Bubble/Index
        public async Task<ActionResult> Index()
        {
            // Categories
            Stream jsonCategories = await WebApiGet("api/Category");
            DataContractJsonSerializer serializerCategory = new DataContractJsonSerializer(typeof(List<Models.Category>));
            ViewData["Categories"] = (List<Models.Category>)serializerCategory.ReadObject(jsonCategories);

            // Radii
            Stream jsonRadii = await WebApiGet("api/Radius");
            DataContractJsonSerializer serializerRadius = new DataContractJsonSerializer(typeof(List<Models.Radius>));
            ViewData["Radii"] = (List<Models.Radius>)serializerRadius.ReadObject(jsonRadii);

            Stream json = await WebApiGet("api/Bubble");
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Models.Bubble>));
            return View((List<Models.Bubble>)serializer.ReadObject(json));
        }

        // POST: Bubble/Create
        [HttpPost]
        public async Task<ActionResult> Create(FormCollection collection)
        {
            // Create new bubble from FormCollection
            Models.Bubble b = new Models.Bubble();
            b.Name = collection["Name"].ToString();
            b.CategoryId = 11; // TBD - hack
            b.Latitude = decimal.Parse(collection["Latitude"].ToString());
            b.Longitude = decimal.Parse(collection["Longitude"].ToString());
            b.AlertMsg = collection["AlertMsg"].ToString();
            b.RadiusId = 1; // TBD - remove this hack and replace with value from dropdown.
            b.Active = collection["Active"].Contains("true");

            Stream json = await WebApiPost("api/bubble", b);
            return RedirectToAction("Index", "Bubble");
        }

        // POST: Bubble/Update/5
        [HttpPost]
        public async Task<ActionResult> Update(int id, FormCollection collection)
        {
            // Get CategoryID key.
            int i = 0;
            while (i < collection.Keys.Count && !collection.Keys[i].Contains("CategoryId")) i++;

            // Get RadiusId key.
            int j = 0;
            while (j < collection.Keys.Count && !collection.Keys[j].Contains("RadiusId")) j++;

            // TBD - this is a hack for testing.
            Models.Bubble b = new Models.Bubble();
            b.Id = id;
            b.ProfileId = Int32.Parse(collection["ProfileId"]);
            b.Name = collection["Name"].ToString();
            b.CategoryId = Int32.Parse(collection[i]);
            b.Latitude = decimal.Parse(collection["Latitude"].ToString());
            b.Longitude = decimal.Parse(collection["Longitude"].ToString());
            b.AlertMsg = collection["AlertMsg"].ToString();
            b.ScheduleId = 99; // TBD - hack
            b.RadiusId = Int32.Parse(collection[j]);
            b.Active = collection["Active"].Contains("true");

            Stream json = await WebApiPut("api/Bubble/" + id.ToString(), b);
            return RedirectToAction("Index");
        }

        // POST: Bubble/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
