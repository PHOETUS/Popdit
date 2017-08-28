using System;
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
                
        Event FromInterop(EventMobile em)
        {
            Event e = new Event();
            e.BubbleId = em.BubbleId;
            e.Id = em.Id;
            e.ProfileId = em.ProfileId;
            e.Timestamp = em.Timestamp;
            return e;
        }

        EventMobile ToInterop(Event e, Bubble b)
        {
            EventMobile em = new EventMobile();
            em.Id = e.Id;
            em.ProviderName = b.Profile.Nickname;
            em.MsgTitle = b.Name;
            em.Msg = b.AlertMsg;
            em.Timestamp = e.Timestamp;
            return em;
        }

        // POST: api/Event
        [ResponseType(typeof(EventMobile))]
        public IHttpActionResult PostEvent(EventMobile em)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Get user.
            int userId = AuthenticatedUserId;
            Profile user = db.Profiles.Find(userId);

            // Get bubble and its creator.
            Bubble bubble = db.Bubbles.Find(em.BubbleId);
            //db.Entry(bubble).Reference(b => b.Profile).Load();  // TBD - speed this up - this is crap.

            // Don't pop anything that's been popped in the last 24 hours.
            DateTime dayAgo = DateTime.Now.AddDays(-1);

            if (bubble.IsFriendly(user) && bubble.IsFresh(user, dayAgo))
            {
                // Save the event in the DB.
                em.ProfileId = userId;  // Security.
                Event evnt = FromInterop(em);
                db.Events.Add(evnt);
                db.SaveChanges();

                // Set EventMobileFields for return trip.
                em = ToInterop(evnt, bubble);
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