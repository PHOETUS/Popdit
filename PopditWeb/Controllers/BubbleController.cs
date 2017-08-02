using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using PopditInterop;

namespace PopditWeb.Controllers
{
    public class BubbleController : Controller
    {
        // GET: Bubble/Index
        public async Task<ActionResult> Index()
        {
            try
            {
                // Categories
                Stream jsonCategories = await WebApi(WebApiMethod.Get, "api/Category");
                DataContractJsonSerializer serializerCategory = new DataContractJsonSerializer(typeof(List<Category>));
                ViewData["Categories"] = (List<Category>)serializerCategory.ReadObject(jsonCategories);

                // Radii
                Stream jsonRadii = await WebApi(WebApiMethod.Get, "api/Radius");
                DataContractJsonSerializer serializerRadius = new DataContractJsonSerializer(typeof(List<Radius>));
                ViewData["Radii"] = (List<Radius>)serializerRadius.ReadObject(jsonRadii);

                Stream json = await WebApi(WebApiMethod.Get, "api/Bubble");
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<BubbleInterop>));
                return View((List<BubbleInterop>)serializer.ReadObject(json));
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("Index", "Home"); }
        }

        // POST: Bubble/Create
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Create(FormCollection collection)
        {
            try
            {
                // Get CategoryID key.
                int i = 0;
                while (i < collection.Keys.Count && !collection.Keys[i].Contains("CategoryId")) i++;

                // Get RadiusId key.
                int j = 0;
                while (j < collection.Keys.Count && !collection.Keys[j].Contains("RadiusId")) j++;

                // Create new bubble from FormCollection
                BubbleInterop b = new BubbleInterop();
                b.Name = collection["Name"].ToString();
                b.CategoryId = ConvertToInt(collection[i]);
                b.AlertMsg = collection["AlertMsg"].ToString();
                b.RadiusId = ConvertToInt(collection[j]);
                b.Active = collection["Active"].Contains("true");
                b.Address = collection["Address"].ToString();

                string lat = collection["Latitude"].ToString();
                if (lat.Length == 0) lat = "0";
                b.Latitude = double.Parse(lat);
                string lng = collection["Latitude"].ToString();
                if (lng.Length == 0) lng = "0";
                b.Longitude = double.Parse(lng);

                Stream json = await WebApi(WebApiMethod.Post, "api/bubble", b);
                return RedirectToAction("Index", "Bubble");
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("Index", "Home"); }
        }

        // POST: Bubble/Update/5
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Update(int id, FormCollection collection)
        {
            try
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
                    BubbleInterop b = new BubbleInterop();
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
                    b.Latitude = double.Parse(lat);
                    string lng = collection["Latitude"].ToString();
                    if (lng.Length == 0) lng = "0";
                    b.Longitude = double.Parse(lng);

                    Stream json = await WebApi(WebApiMethod.Put, "api/Bubble/" + id.ToString(), b);
                }
                // DELETE
                if (collection["command"].Equals("Confirm deletion"))
                {
                    Stream json = await WebApi(WebApiMethod.Delete, "api/Bubble/" + id.ToString());
                }
                return RedirectToAction("Index");
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("Index", "Home"); }
        }
    }
}
