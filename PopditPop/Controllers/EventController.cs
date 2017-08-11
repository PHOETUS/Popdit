using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using PopditDB.Models;
using PopditMobileApi;

namespace PopditPop.Controllers
{
    public class EventController : ApiController
    {
        private Entities db = new Entities();

        /*
        // GET: api/Event
        public IQueryable<Event> GetEvents()
        {
            return db.Events;
        }
       
        // GET: api/Event/5
        [ResponseType(typeof(Event))]
        public IHttpActionResult GetEvent(int id)
        {
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }

            return Ok(@event);
        }
        */
        
        Event FromInterop(EventMobile em)
        {
            Event e = new Event();
            e.BubbleId = em.BubbleId;
            e.Id = em.Id;
            e.ProfileId = em.ProfileId;
            e.Timestamp = em.Timestamp;
            return e;
        }

        // POST: api/Event
        [ResponseType(typeof(EventMobile))]
        public IHttpActionResult PostEvent(EventMobile em)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Get bubble and its creator.
            Bubble bubble = db.Bubbles.Find(em.BubbleId);
            db.Entry(bubble).Reference(b => b.Profile).Load();  // TBD - speed this up - this is crap.

            // Get user.
            int userId = AuthenticatedUserId;
            Profile user = db.Profiles.Find(userId);
            bool friendlyBubble = false;

            // Check to see if the bubble belongs to a friend.
            foreach (Friendship f in user.Friendships1)
                if (f.ProfileIdOwned == bubble.ProfileId) friendlyBubble = true;

            if (friendlyBubble)
            {
                // Save the event in the DB.
                em.ProfileId = userId;  // Security.
                Event e = FromInterop(em);
                db.Events.Add(e);
                db.SaveChanges();

                // Set EventMobileFields for return trip.
                em.ProviderName = bubble.Profile.Nickname;
                em.MsgTitle = bubble.Name;
                em.Msg = bubble.AlertMsg;
                em.Suppressed = false; // TBD - enable suppression.                
            }
            else em.Suppressed = true;

            return CreatedAtRoute("DefaultApi", new { id = em.Id }, em);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(int id)
        {
            return db.Events.Count(e => e.Id == id) > 0;
        }
    }
}