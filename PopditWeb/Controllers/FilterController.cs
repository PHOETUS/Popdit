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

namespace PopditWeb.Controllers
{
    public class FilterController : Controller
    {
        // GET: Filter
        public ActionResult Index()
        {
            InitializeList("Filter", null);
            List<Models.FilterData> fdList = new List<Models.FilterData>();
            foreach (JObject j in mObjectList)
            {
                Models.FilterData fd = new Models.FilterData();
                fd.Id = Int32.Parse((string)j["Id"]);
                fd.Name = (string)j["Name"];
                fdList.Add(fd);
            }
            return View(fdList);
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

        // GET: Filter/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Filter/Update/5
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
