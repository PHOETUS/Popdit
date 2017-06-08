using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;

namespace PopditWeb.Controllers
{
    public class FilterController : Controller
    {
        // GET: Filter/Index
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

            // Filters
            Stream json = await WebApiGet("api/Filter");
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Models.Filter>));
            return View((List<Models.Filter>)serializer.ReadObject(json));
        }

        // POST: Filter/Create
        [HttpPost]
        public async Task<ActionResult> Create(FormCollection collection)
        {
            // Create new filter from FormCollection
            Models.Filter f = new Models.Filter();
            f.Name = collection["Name"].ToString();
            f.RadiusId = 1; // TBD - remove this hack and replace with value from dropdown.

            Stream json = await WebApiPost("api/Filter", f);
            return RedirectToAction("Index", "Filter");
        }

        // POST: Filter/Update/5
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
                Models.Filter f = new Models.Filter();
                f.Id = id;
                f.Name = collection["Name"];
                f.ProfileId = 1;
                f.CategoryId = Int32.Parse(collection[i]);
                f.ScheduleId = null;
                f.RadiusId = Int32.Parse(collection[j]);
                f.Active = true;

                Stream json = await WebApiPut("api/Filter/" + id.ToString(), f);
            }
            // DELETE
            if (collection["command"].Equals("Confirm deletion"))
            {
                Stream json = await WebApiDelete("api/Filter/" + id.ToString());
            }
            return RedirectToAction("Index");
        }

        // GET: Filter/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Filter/Delete/5
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
