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
            Stream json = await WebApiGet("api/Bubble");
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Models.Bubble>));
            return View((List<Models.Bubble>)serializer.ReadObject(json));
        }

        // GET: Bubble/Read/5
        public ActionResult Read(int id)
        {
            return View();
        }

        // GET: Bubble/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bubble/Create
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

        // POST: Bubble/Update/5
        [HttpPost]
        public ActionResult Update(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Bubble/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
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
