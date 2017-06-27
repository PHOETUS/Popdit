using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using DBLayer.Models;

namespace PopditCore.Controllers
{
    public class BubbleController : ApiController
    {
        private PopditDBEntities db = new PopditDBEntities();

        // GET: api/Bubble
        public System.Web.Http.Results.JsonResult<List<Bubble>> GetBubbles()
        {
            return Json(db.Bubbles.Where(m => m.ProfileId == AuthenticatedUserId).OrderBy(m => m.Name).ToList());
        }

        // GET: api/Bubble/5
        [ResponseType(typeof(Bubble))]
        public System.Web.Http.Results.JsonResult<Bubble> GetBubble(int id)
        {
            Bubble bubble = db.Bubbles.Find(id);
            return Json(bubble);
        }

        // PUT: api/Bubble/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBubble(int id, Bubble newBubble)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (id != newBubble.Id) { return BadRequest(); }

            // Change only the changed fields in the profile.
            // Only the fields below are changeable via the API.
            // Non-nullable fields must be supplied.
            Bubble oldBubble = db.Bubbles.Find(id);
            oldBubble.Name = newBubble.Name ?? oldBubble.Name;
            oldBubble.Latitude = newBubble.Latitude; // ?? oldBubble.Latitude;
            oldBubble.Longitude = newBubble.Longitude; // ?? oldBubble.Longitude;
            
            //Not sure what this function was doing but it's not present now.
            //oldBubble.UpdateBounds();
            oldBubble.AlertMsg = newBubble.AlertMsg ?? oldBubble.AlertMsg;
            oldBubble.AddressId = newBubble.AddressId ?? oldBubble.AddressId;
            oldBubble.ProfileId = newBubble.ProfileId; // ?? oldBubble.ProfileId;
            oldBubble.CategoryId = newBubble.CategoryId; // ?? oldBubble.CategoryId;
            oldBubble.ScheduleId = newBubble.ScheduleId; // ?? oldBubble.ScheduleId;
            oldBubble.RadiusId = newBubble.RadiusId; // ?? oldBubble.RadiusId;
            oldBubble.Active = newBubble.Active; // ?? oldBubble.Active;

            db.Entry(oldBubble).State = EntityState.Modified;

            try { db.SaveChanges(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!BubbleExists(id)) { return NotFound(); }
                else { throw; }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Bubble
        [ResponseType(typeof(Bubble))]
        public IHttpActionResult PostBubble(Bubble bubble)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            bubble.ProfileId = AuthenticatedUserId;
            db.Bubbles.Add(bubble);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = bubble.Id }, bubble);
        }

        // DELETE: api/Bubble/5
        [ResponseType(typeof(Bubble))]
        public IHttpActionResult DeleteBubble(int id)
        {
            Bubble bubble = db.Bubbles.Find(id);
            if (bubble == null)
            {
                return NotFound();
            }

            db.Bubbles.Remove(bubble);
            db.SaveChanges();

            return Ok(bubble);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { db.Dispose(); }
            base.Dispose(disposing);
        }

        private bool BubbleExists(int id)
        {
            return db.Bubbles.Count(e => e.Id == id) > 0;
        }
    }
}