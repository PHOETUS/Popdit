using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using PopditDB.Models;
using PopditMobile.Models;

namespace PopditPop.Controllers
{
    public class EventController : ApiController
    {
        private PopditDBEntities db = new PopditDBEntities();

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

        // POST: api/Event
        [ResponseType(typeof(EventMobile))]
        public IHttpActionResult PostEvent(EventMobile @event)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Set up Event fields for storage in DB.
            Event e = new Event();
            e.BubbleId = @event.BubbleId;
            e.ProfileId = @event.ProfileId;
            e.TimestampJson = @event.TimestampJson;

            db.Events.Add(e);
            db.SaveChanges();

            // Set EventMobileFields for return trip.
            db.Entry(e).Reference(b => b.Profile).Load();  // TBD - speed this up - this is crap.
            @event.ProviderName = e.Profile.Nickname;
            db.Entry(e).Reference(b => b.Bubble).Load();  // TBD - speed this up - this is crap.
            @event.MsgTitle = e.Bubble.Name;
            @event.Msg = e.Bubble.AlertMsg;            
            
            return CreatedAtRoute("DefaultApi", new { id = @event.Id }, @event);
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