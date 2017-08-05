using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using PopditDB.Models;
using PopditWebApi;

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
            foreach (PopditDB.Models.Event e in events)
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
    }
}