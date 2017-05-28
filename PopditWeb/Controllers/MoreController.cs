using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PopditWeb.Controllers
{
    public class MoreController : Controller
    {
        public ActionResult Index() { return View(); }

        public ActionResult History() { return View(); }

        public ActionResult Metrics() { return View(); }

        public ActionResult Account() { return View(); }

        public ActionResult Developers() { return View(); }

        public ActionResult Contact() { return View(); }

        public ActionResult About() { return View(); }
    }
}