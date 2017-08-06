using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System;
using PopditWebApi;
using HashidsNet;

namespace PopditWeb.Controllers
{
    public class FilterController : Controller
    {
        static Hashids hashids = new Hashids("Sing a Song of Sixpence", 6);

        // GET: Filter/Index
        public async Task<ActionResult> Index()
        {
            try
            {
                // If this page was given a hashed filter ID, get a copy of that filter 
                // and show it on the page so the user can decide whether he wants to keep it.
                string f = Request.QueryString["f"];                
                if (f != null)
                {
                    // Decode hashed Filter Id.
                    f = hashids.Decode(f)[0].ToString();
                    Stream jsonFilter = await WebApi(WebApiMethod.Get, "api/Filter/" + f);
                    DataContractJsonSerializer serializerFilter = new DataContractJsonSerializer(typeof(FilterInterop));
                     FilterInterop copiedFilter = (FilterInterop)serializerFilter.ReadObject(jsonFilter);
                    copiedFilter.Id = 0;  // STERILIZE THE COPY SO IT CANNOT OVERWRITE THE ORIGINAL!
                    ViewData["CopiedFilter"] = copiedFilter;
                }

                // Categories
                Stream jsonCategories = await WebApi(WebApiMethod.Get, "api/Category");
                DataContractJsonSerializer serializerCategory = new DataContractJsonSerializer(typeof(List<Category>));
                ViewData["Categories"] = (List<Category>)serializerCategory.ReadObject(jsonCategories);

                // Radii
                Stream jsonRadii = await WebApi(WebApiMethod.Get, "api/Radius");
                DataContractJsonSerializer serializerRadius = new DataContractJsonSerializer(typeof(List<RadiusInterop>));
                ViewData["Radii"] = (List<RadiusInterop>)serializerRadius.ReadObject(jsonRadii);

                // Filters
                Stream json = await WebApi(WebApiMethod.Get, "api/Filter");
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<FilterInterop>));
                return View((List<FilterInterop>)serializer.ReadObject(json));
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
                FilterInterop f = new FilterInterop();
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
                    FilterInterop f = new FilterInterop();
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
