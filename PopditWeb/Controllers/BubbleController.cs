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
            // Get CategoryID key.
            int i = 0;
            while (i < collection.Keys.Count && !collection.Keys[i].Contains("CategoryId")) i++;

            // Get RadiusId key.
            int j = 0;
            while (j < collection.Keys.Count && !collection.Keys[j].Contains("RadiusId")) j++;

            // Create new bubble from FormCollection
            Models.Bubble b = new Models.Bubble();
            b.Name = collection["Name"].ToString();
            b.CategoryId = ConvertToInt(collection[i]);
            b.AlertMsg = collection["AlertMsg"].ToString();
            b.RadiusId = ConvertToInt(collection[j]);
            b.Active = collection["Active"].Contains("true");
            b.Address = collection["Address"].ToString();

            string lat = collection["Latitude"].ToString();
            if (lat.Length == 0) lat = "0";
            b.Latitude = decimal.Parse(lat);
            string lng = collection["Latitude"].ToString();
            if (lng.Length == 0) lng = "0";
            b.Longitude = decimal.Parse(lng);

            Stream json = await WebApiPost("api/bubble", b);
            return RedirectToAction("Index", "Bubble");
        }

        // POST: Bubble/Update/5
        [HttpPost]
        public async Task<ActionResult> Update(int id, FormCollection collection)
        {
            // UPDATE
            if (collection["command"].Equals("Update"))
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
                b.ProfileId = ConvertToInt(collection["ProfileId"]);
                b.Name = collection["Name"].ToString();
                b.CategoryId = ConvertToInt(collection[i]);
                b.AlertMsg = collection["AlertMsg"].ToString();
                b.ScheduleId = 99; // TBD - hack
                b.RadiusId = ConvertToInt(collection[j]);
                b.Active = collection["Active"].Contains("true");
                b.Address = collection["Address"].ToString();

                string lat = collection["Latitude"].ToString();
                if (lat.Length == 0) lat = "0";
                b.Latitude = decimal.Parse(lat);
                string lng = collection["Latitude"].ToString();
                if (lng.Length == 0) lng = "0";
                b.Longitude = decimal.Parse(lng);

                Stream json = await WebApiPut("api/Bubble/" + id.ToString(), b);
            }
            // DELETE
            if (collection["command"].Equals("Confirm deletion"))
            {
                Stream json = await WebApiDelete("api/Bubble/" + id.ToString());
            }
            return RedirectToAction("Index");
        }
    }
}
