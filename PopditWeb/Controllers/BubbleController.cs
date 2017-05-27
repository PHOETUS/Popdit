using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
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

        /*
        // GET: Bubble/Update/5
        public ActionResult Update(int id)
        {
            InitializeList("Bubble/Details/" + id.ToString(), null);
            JObject bubble = (JObject)mObjectList[0];
            Models.BubbleData bd = new Models.BubbleData();
            bd.Id = Int32.Parse(bubble["Id"].ToString()); 
            bd.ProfileId = Int32.Parse(bubble["ProfileId"].ToString()); 
            bd.Name = (string)bubble["Name"];
            bd.Latitude = Decimal.Parse((string)bubble["Latitude"]);
            bd.Longitude = Decimal.Parse((string)bubble["Loingitude"]);
            bd.AddressId = Int32.Parse((string)bubble["AddressId"]);
            bd.AlertMsg = (string)bubble["AlertMsg"];
            bd.RadiusId = Int32.Parse(bubble["RadiusId"].ToString());
            return View(bd);
        }*/

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
