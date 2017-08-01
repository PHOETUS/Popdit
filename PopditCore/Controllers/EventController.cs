using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PopditDB.Models;
using PopditMobile.Models;

namespace PopditCore.Controllers
{
    public class EventController : ApiController
    {
        private PopditDBEntities db = new PopditDBEntities();

        // GET: api/Event
        public IQueryable<EventMobile> GetEvents()
        {
            // Get all of the events for this profile in the last 24 hours in reverse chrono order and include their bubbles.
            DateTime dayAgo = DateTime.Now.AddDays(-1);
            var events = db.Events.Where(e => e.ProfileId == AuthenticatedUserId && e.Timestamp > dayAgo).OrderByDescending(e => e.Timestamp).Include(e => e.Bubble.Profile);  // Security.

            List<EventMobile> eventList = new List<EventMobile>();
            foreach (Event e in events)
            {
                EventMobile em = new EventMobile();

                em.Id = e.Id;
                em.TimestampJson = e.TimestampJson;
                em.ProviderName = e.Bubble.Profile.Nickname;
                em.MsgTitle = e.Bubble.Name;
                em.Msg = e.Bubble.AlertMsg;
                em.ProfileId = e.ProfileId;
                em.BubbleId = e.BubbleId;
                em.Latitude = e.Bubble.Latitude;
                em.Longitude = e.Bubble.Longitude;
                
                eventList.Add(em);
            }
            return eventList.AsQueryable<EventMobile>();
        }

        /*
        // GET: api/Event/5
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> GetEvent(int id)
        {
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            return Ok(@event);
        }

        // PUT: api/Event/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEvent(int id, Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @event.Id)
            {
                return BadRequest();
            }

            db.Entry(@event).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Event
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> PostEvent(Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Events.Add(@event);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = @event.Id }, @event);
        }

        // DELETE: api/Event/5
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> DeleteEvent(int id)
        {
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            db.Events.Remove(@event);
            await db.SaveChangesAsync();

            return Ok(@event);
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
        */
    }
}