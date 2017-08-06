using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using PopditDB.Models;
using PopditMobileApi;

namespace PopditPop.Controllers
{
    public class EventController : ApiController
    {
        private PopditDBEntities db = new PopditDBEntities();

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

            // Set up Event fields for storage in DB.
            em.ProfileId = AuthenticatedUserId;  // Security.
            Event e = FromInterop(em);
            db.Events.Add(e);
            db.SaveChanges();

            // Set EventMobileFields for return trip.
            Bubble bubble = db.Bubbles.Find(em.BubbleId);
            db.Entry(bubble).Reference(b => b.Profile).Load();  // TBD - speed this up - this is crap.
            em.ProviderName = bubble.Profile.Nickname;
            em.MsgTitle = bubble.Name;
            em.Msg = bubble.AlertMsg;            
            
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