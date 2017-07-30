using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System;

namespace PopditWeb.Controllers
{
    public class FilterController : Controller
    {
        // GET: Filter/Index
        public async Task<ActionResult> Index()
        {
            try
            {
                // Categories
                Stream jsonCategories = await WebApi(WebApiMethod.Get, "api/Category");
                DataContractJsonSerializer serializerCategory = new DataContractJsonSerializer(typeof(List<Models.Category>));
                ViewData["Categories"] = (List<Models.Category>)serializerCategory.ReadObject(jsonCategories);

                // Radii
                Stream jsonRadii = await WebApi(WebApiMethod.Get, "api/Radius");
                DataContractJsonSerializer serializerRadius = new DataContractJsonSerializer(typeof(List<Models.Radius>));
                ViewData["Radii"] = (List<Models.Radius>)serializerRadius.ReadObject(jsonRadii);

                // Filters
                Stream json = await WebApi(WebApiMethod.Get, "api/Filter");
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Models.Filter>));
                return View((List<Models.Filter>)serializer.ReadObject(json));
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("Index", "Home"); }
        }

        // POST: Filter/Create
        [HttpPost]
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

                // Create new filter from FormCollection
                Models.Filter f = new Models.Filter();
                f.Name = collection["Name"].ToString();
                f.CategoryId = ConvertToNullableInt(collection[i]);
                f.RadiusId = ConvertToInt(collection[j]);
                f.Active = collection["Active"].Contains("true");

                Stream json = await WebApi(WebApiMethod.Post, "api/Filter", f);
                return RedirectToAction("Index", "Filter");
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("Index", "Home"); }
        }

        // POST: Filter/Update/5
        [HttpPost]
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
                    Models.Filter f = new Models.Filter();
                    f.Id = id;
                    f.Name = collection["Name"];
                    f.ProfileId = ConvertToInt(collection["ProfileId"]);
                    f.CategoryId = ConvertToNullableInt(collection[i]);
                    f.ScheduleId = null;
                    f.RadiusId = ConvertToInt(collection[j]);
                    f.Active = collection["Active"].Contains("true");

                    Stream json = await WebApi(WebApiMethod.Put, "api/Filter/" + id.ToString(), f);
                }
                // DELETE
                if (collection["command"].Equals("Confirm deletion"))
                {
                    Stream json = await WebApi(WebApiMethod.Delete, "api/Filter/" + id.ToString());
                }
                return RedirectToAction("Index");
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("Index", "Home"); }
        }
    }
}
