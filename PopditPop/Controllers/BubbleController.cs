using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using PopditDB.Models;
using PopditMobileApi;
using PopditPop.Classes;

namespace PopditPop.Controllers
{
    public class BubbleController : ApiController
    {
        private Entities db = new Entities();

        // POST: api/Bubble - Gets the bubbles that lie partially or fully within a zone.
        [HttpPost]
        public System.Web.Http.Results.JsonResult<List<BubbleMobile>> GetBubbles(Location loc)
        {
            // TBD - limit bubble set by user's filters - or handle this at pop time?
            
            // TBD - Set catalog and refresh radii using algorithm.
            double catalogRadius = 8000;  // Area in which to get bubbles.
            double refreshRadius = 6000;  // Area outside of which to refresh bubble catalog.

            Zone z = new Zone(loc.Latitude, loc.Longitude, catalogRadius);

            // A bubble overlaps a zone if
            // 1) its min or max lat is between the zone's min and min lat; and
            // 2) its min or max long is between the zone's min and max long.

            // NOTE - This algorithm can fail if the zone is entirely contained within the bubble;
            // Therefore, it is reliable only if the zone is **bigger** than the biggest allowable bubble.
            // The biggest allowable bubble radius is currently 2 miles or 3219 meters.

            // Get a list of bubbles popped by this user in the last 24 hours.
            DateTime dayAgo = DateTime.Now.AddDays(-1);
            List<Bubble> popped = db.Events.Where(e => e.ProfileId == AuthenticatedUserId && e.Timestamp > dayAgo).Select(e => e.Bubble).ToList();

            // Get a list of bubbles in the user's area.
            List<Bubble> bubbles = db.Bubbles.Where(b =>
            ((z.MinLatitude < b.MaxLatitude && b.MaxLatitude < z.MaxLatitude) ||
             (z.MinLatitude < b.MinLatitude && b.MinLatitude < z.MaxLatitude)) &&
            ((z.MinLongitude < b.MaxLongitude && b.MaxLongitude < z.MaxLongitude) ||
             (z.MinLongitude < b.MinLongitude && b.MinLongitude < z.MaxLongitude))).ToList();

            // Find the complement.
            List<Bubble> poppable = new List<Bubble>();
            foreach (Bubble b in bubbles)
                if (!popped.Contains(b)) poppable.Add(b);

            // Convert Bubble to lightweight BubbleMobile for use by mobile app.
            List<BubbleMobile> bubblesMobile = new List<BubbleMobile>();
            foreach (Bubble b in poppable)
            {
                BubbleMobile bm = new BubbleMobile();
                bm.Id = b.Id;
                bm.Latitude = b.Latitude;
                bm.Longitude = b.Longitude;
                bm.Radius = b.Radius.Meters;
                bubblesMobile.Add(bm);
            }

            // Build a bubble to represent the new refresh radius.
            BubbleMobile refreshBubble = new BubbleMobile();
            refreshBubble.Id = 0; // Marker for refresh bubble.
            refreshBubble.Radius = (int)refreshRadius;
            bubblesMobile.Add(refreshBubble);

            return Json(bubblesMobile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BubbleExists(int id)
        {
            return db.Bubbles.Count(e => e.Id == id) > 0;
        }
    }
}
