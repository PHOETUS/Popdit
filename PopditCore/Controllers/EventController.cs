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
        private Entities db = new Entities();

        // GET: api/Event
        public IQueryable<EventInterop> GetEvents()
        {
            // Get all of the events for this profile in the last 24 hours in reverse chrono order and include their bubbles.
            DateTime dayAgo = DateTime.Now.AddDays(-1);
            var events = db.Events.Where(e => e.ProfileId == AuthenticatedUserId && e.Timestamp > dayAgo).OrderByDescending(e => e.Timestamp).Include(e => e.Bubble.Profile);  // Security.

            List<EventInterop> eventList = new List<EventInterop>();
            foreach (PopditDB.Models.Event e in events)
            {
                EventInterop em = new EventInterop();

                em.Id = e.Id;
                em.TimestampJson = e.Timestamp.ToString();
                em.ProviderName = e.Bubble.Profile.Nickname;
                em.MsgTitle = e.Bubble.Name;
                em.Msg = e.Bubble.AlertMsg;
                em.Phone = e.Bubble.Phone;
                em.Url = e.Bubble.Url;
                //em.ProfileId = e.ProfileId;
                //em.BubbleId = e.BubbleId;
                em.Latitude = e.Bubble.Latitude;
                em.Longitude = e.Bubble.Longitude;
                
                eventList.Add(em);
            }
            return eventList.AsQueryable<EventInterop>();
        }
    }
}