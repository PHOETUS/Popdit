using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace PopditWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            // Get query string.
            string f = Request.QueryString["f"];
            // Get cookie.
            HttpCookie cookie = HttpContext.Request.Cookies["Popdit"];

            // If there's no cookie or it's expired, just display the sign in page.  
            if (cookie == null) return View();
            else
            {
                // If there's a query string, go to the filters page.
                if (f != null) return RedirectToAction("Index", "Filter", new { f = f });
                // Otherwise go to the Pops page.
                else return RedirectToAction("Index", "Event");
            }
        }

        // POST: Home
        public ActionResult SignIn(FormCollection collection)
        {
            string phone = collection["Phone"];
            string pwd = collection["Password"];

            string cookieName = "Popdit";
            int cookieDaysToLive = Convert.ToInt32(ConfigurationManager.AppSettings["CookieDaysToLive"]);

            HttpCookie cookie = HttpContext.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie.Values["Phone"] = Regex.Replace(phone, "[^0-9.]", ""); // Strip out alpha characters.
            cookie.Values["Password"] = pwd;
            cookie.Expires = DateTime.Now.AddDays(cookieDaysToLive);
            HttpContext.Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Event");
        }

        public ActionResult SignOut()
        {
            string cookieName = "Popdit"; // TBD - Do not hard-code.

            HttpCookie cookie = HttpContext.Request.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Response.Cookies.Add(cookie);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Terms()
        {
            return RedirectToAction("TermsSignedOut", "More");
        }
    }
}