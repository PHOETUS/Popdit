using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
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
                string json = await WebApi(WebApiMethod.Get, "api/Friend");
                List<Friend> friendList = JsonConvert.DeserializeObject<List<Friend>>(json);
                return View(friendList);
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
                f.Tagline = collection["Tagline"];
                await WebApi(WebApiMethod.Post, "api/Friend", f);
                return RedirectToAction("Index", "Friend");
            }
            // Authentication failure?  No such friend?
            catch (Exception e) { return RedirectToAction("Index", "Friend"); }
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
                    await WebApi(WebApiMethod.Delete, "api/Friend/" + id.ToString());
                return RedirectToAction("Index", "Friend");
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("Index", "Home"); }
        }
    }
}
