using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PopditPop.Models;
using System.Diagnostics;
using System.IO;

namespace PopditPop.Controllers
{
    public class FixController : PopditController
    {
        // POST: Fix/Create
        [HttpPost]
        public ActionResult Create()
        {
            try
            {
                // Parse JSON from request.
                Request.InputStream.Position = 0;
                string json = new System.IO.StreamReader(Request.InputStream).ReadToEnd();               
                JObject j = JObject.Parse(json);
               
                Debug.WriteLine("Fix");
                // Who, when, where is the fix?
                int id = (int)j.GetValue("ProfileId");
                DateTime timestamp = DateTime.Now;  // We assume minimal latency or total packet loss, therefore no extremely late fixes.
                decimal latitude = (decimal)j.GetValue("Latitude");
                decimal longitude = (decimal)j.GetValue("Longitude");
                Debug.WriteLine("Id: " + id.ToString() + " Latitude: " + latitude.ToString() + " Longitude: " + longitude.ToString() + " Time: " + timestamp.ToString());
              
                // TBD - make this a really efficient stored proc.

                // Find the active Trips that the fix popped, if any.
                var trips = mContext.Bubbles.Where
                    (t => 
                    t.MinLatitude <= latitude &&
                    t.MaxLatitude >= latitude &&
                    t.MinLongitude <= longitude &&
                    t.MaxLongitude >= longitude &&
                    t.Active == true);
 
                if (trips.Count() > 0) // If the fix popped any bubbles ...
                {
                    // Get the Profile's active Filters.
                    var filters = mContext.Filters.Where
                        (f =>
                        f.ProfileId == id &&
                        f.Active == true);

                    // Match Trips with Filters.
                    foreach (PopditPop.Models.Filter f in filters)
                    {
                        foreach (Bubble t in trips)
                        {
                            // TBD - Make this matching section account for supercategories, schedules, etc.
                            int filterCategoryId = f.CategoryId ?? 0;
                            if (filterCategoryId == t.CategoryId)  // If the Filter matches the Trip ...                            
                            {                                
                                // PROCESS THE MATCH - This is where the magic happens.
                                Event e = new Event();
                                e.ProfileId = id;
                                e.Timestamp = timestamp;
                                e.TripId = t.Id;
                                mContext.Events.Add(e);                                                                 
                            }
                        }
                    }
                    mContext.SaveChanges();
                }
                return Json(0);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: Fix failed : " + e.Message);
                return Json(e.Message + " ----- " + e.InnerException);
            }
        }
                    
        public ActionResult Test()
        {
            Debug.WriteLine("Test");
            return Json("Hello", JsonRequestBehavior.AllowGet);
        }
        
   }
}
