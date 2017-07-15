using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using DBLayer.Models;
using PopditPop.Models;

namespace PopditPop.Controllers
{
    public class BubbleController : ApiController
    {
        private PopditDBEntities db = new PopditDBEntities();

        // POST: api/Bubble - Gets the bubbles that lie partially or fully within a zone.
        [HttpPost]
        public System.Web.Http.Results.JsonResult<List<Bubble>> GetBubbles(Zone z)
        {
            // TBD - limit bubble set by user's filters.

            // A bubble overlaps a zone if 
            // 1) its min or max lat is between the zone's min and min lat; and
            // 2) its min or max long is between the zone's min and max long.

            // NOTE - This algorithm can fail if the zone is entirely contained within the bubble; 
            // Therefore, it is reliable only if the zone is **bigger** than the biggest allowable bubble.

            System.Web.Http.Results.JsonResult<List<Bubble>> result = Json(db.Bubbles.Where(b =>
            ((z.MinLatitude < b.MaxLatitude && b.MaxLatitude < z.MaxLatitude) ||
             (z.MinLatitude < b.MinLatitude && b.MinLatitude < z.MaxLatitude)) &&
            ((z.MinLongitude < b.MaxLongitude && b.MaxLongitude < z.MaxLongitude) ||
             (z.MinLongitude < b.MinLongitude && b.MinLongitude < z.MaxLongitude))
             ).ToList());

            return result;
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