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
            Stream json = await WebApiGet("api/Filter");
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Models.Filter>));
            return View((List<Models.Filter>)serializer.ReadObject(json));
        }

        // GET: Filter/Read/5
        public ActionResult Read(int id)
        {
            return View();
        }

        // GET: Filter/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Filter/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Filter/Update/5
        [HttpPost]
        public async Task<ActionResult> Update(int id, FormCollection collection)
        {
            try
            {
                // TBD - this is a hack for testing.
                Models.Filter f = new Models.Filter();
                f.Id = id;
                f.Name = collection["Name"];
                f.ProfileId = 1;
                f.CategoryId = 11;
                f.ScheduleId = null;
                f.RadiusId = 4;
                f.Active = true;

                Stream json = await WebApiPut("api/Filter/" + id.ToString(), f);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
