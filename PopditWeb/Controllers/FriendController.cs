using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
using System.Runtime.Serialization.Json;
using PopditWebApi;
using System.Threading.Tasks;

namespace PopditWeb.Controllers
{
    public class FriendController : Controller
    {
        // INDEX
        // GET: Friend
        public async Task<ActionResult> Index()
        {
            try
            {
                Stream json = await WebApi(WebApiMethod.Get, "api/Friend");
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Friend>));
                return View((List<Friend>)serializer.ReadObject(json));
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("Index", "Home"); }
        }

        // CREATE - filled
        // POST: Friend/Create
        [HttpPost]
        public async Task<ActionResult> Create(FormCollection collection)
        {
            try
            {
                Friend f = new Friend();
                f.Nickname = collection["Nickname"];
                Stream json = await WebApi(WebApiMethod.Post, "api/Friend", f);
                return RedirectToAction("Index", "Friend");
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("Index", "Home"); }
        }

        // EDIT - filled (This method follows the pattern of the one in the bubble controller - that's why it's an update, not a delete.)
        // POST: Friend/Edit/5
        [HttpPost]
        public async Task<ActionResult> Update(int id, FormCollection collection)
        {
            try
            {
                // DELETE
                if (collection["command"].Equals("Confirm deletion"))
                {
                    Stream json = await WebApi(WebApiMethod.Delete, "api/Friend/" + id.ToString());
                }
                return RedirectToAction("Index", "Friend");
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("Index", "Home"); }
        }

        /*
        // DETAILS
        // GET: Friend/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // CREATE - blank
        // GET: Friend/Create
        public ActionResult Create()
        {
            return View();
        }

        // EDIT - blank
        // GET: Friend/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // DELETE - blank
        // GET: Friend/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // DELETE - filled
        // POST: Friend/Delete/5
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
        */
    }
}
