using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PopditWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()  { return View(); }

        // POST: Home
        public ActionResult SignIn(FormCollection collection)
        {
            string phone = collection["Phone"];
            string pwd = collection["Password"];

            string cookieName = "Popdit"; // TBD - Do not hard-code.
            int cookieDaysToLive = 7; // TBD - Do not hard-code.

            HttpCookie cookie = HttpContext.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie.Values["Phone"] = phone; 
            cookie.Values["Password"] = pwd;
            cookie.Expires = DateTime.Now.AddDays(cookieDaysToLive);
            HttpContext.Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Event");
        }

        public ActionResult SignOut()
        {
            string cookieName = "Popdit"; // TBD - Do not hard-code.

            HttpCookie cookie = HttpContext.Request.Cookies[cookieName];
            if (cookie != null) cookie.Expires = DateTime.Now.AddHours(-1);

            return RedirectToAction("Index", "Home");
        }
    }
}