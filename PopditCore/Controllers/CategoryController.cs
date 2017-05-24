using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PopditCore.Models;

namespace PopditCore.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category 
        public ActionResult Index()
        {
            return Json(mContext.Categories.Select(r => new
            {
                r.Id,
                r.Description,
                SupercategoryId = r.CategoryId
            }), JsonRequestBehavior.AllowGet);
        }

        // GET: Category/Read/5
        public ActionResult Read(int id)
        {
            Models.Category r = mContext.Categories.Single(e => (e.Id == id));
            return Json(new
            {
                r.Id,
                r.Description,
                SupercategoryId = r.CategoryId
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
