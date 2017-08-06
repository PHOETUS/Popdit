using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using PopditDB.Models;
using PopditWebApi;

namespace PopditCore.Controllers
{
    public class BubbleController : ApiController
    {
        private PopditDBEntities db = new PopditDBEntities();

        async Task<Location> Geocode(string address)
        {
            address = address.Replace(' ', '+');
            string url = System.Configuration.ConfigurationManager.AppSettings["GeocodeURL"];
            url = url + "address=" + address + "&key=" + System.Configuration.ConfigurationManager.AppSettings["GeocodeApiKey"];

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string json = await client.GetStringAsync(url).ConfigureAwait(false);
                    // Parse the location out of the Google API geocode results.
                    dynamic location = JObject.Parse(json)["results"][0]["geometry"]["location"];
                    return new Location((double)location["lat"], (double)location["lng"]);
                }
            }
            catch (Exception e)
            {
                // TBD - Handle error
                return null;
            }
        }

        // GET: api/Bubble
        public System.Web.Http.Results.JsonResult<List<Bubble>> GetBubbles()
        {
            return Json(db.Bubbles.Where(m => m.ProfileId == AuthenticatedUserId).OrderBy(m => m.Name).ToList()); // Security.
        }

        /*
        // GET: api/Bubble/5
        [ResponseType(typeof(Bubble))]
        public System.Web.Http.Results.JsonResult<Bubble> GetBubble(int id)
        {
            Bubble bubble = db.Bubbles.Find(id);
            return Json(bubble);
        }
        */

        // PUT: api/Bubble/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBubble(int id, Bubble newBubble)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (id != newBubble.Id) { return BadRequest(); }

            // Change only the changed fields in the profile.
            // Only the fields below are changeable via the API.
            // Non-nullable fields must be supplied.
            Bubble oldBubble = db.Bubbles.Find(id);  // TBD - Security.
            oldBubble.Name = newBubble.Name ?? oldBubble.Name;
            oldBubble.Latitude = newBubble.Latitude;
            oldBubble.Longitude = newBubble.Longitude;
            oldBubble.AlertMsg = newBubble.AlertMsg ?? oldBubble.AlertMsg;
            oldBubble.ProfileId = newBubble.ProfileId;
            oldBubble.CategoryId = newBubble.CategoryId;
            oldBubble.ScheduleId = newBubble.ScheduleId;
            oldBubble.RadiusId = newBubble.RadiusId;
            oldBubble.Active = newBubble.Active;
            // oldBubble.Address = newBubble.Address ?? oldBubble.Address;

            // if the address changed, and it's not null or zero-length, geocode it into the lat/long.
            if (newBubble.Address != oldBubble.Address && newBubble.Address != null && newBubble.Address.Length > 0)
            {
                Location loc = Geocode(newBubble.Address).Result;
                oldBubble.Latitude = loc.Latitude;
                oldBubble.Longitude = loc.Longitude;
                oldBubble.Address = newBubble.Address;
            }

            // Force loading of Radius for use in UpdateMaxMin.
            db.Entry(oldBubble).Reference(b => b.Radius).Load();  // TBD - too expensive
            // Update the max and min lat and long.
            oldBubble.UpdateMaxMin();

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

            bubble.ProfileId = AuthenticatedUserId; // Security

            // if there's an address, geocode it into the lat/long.
            if (bubble.Address != null && bubble.Address.Length > 0)
            {
                Location loc = Geocode(bubble.Address).Result;
                bubble.Latitude = loc.Latitude;
                bubble.Longitude = loc.Longitude;
            }

            bubble.Radius = db.Radii.Find(bubble.RadiusId);  // TBD - too expensive
            bubble.UpdateMaxMin();
            db.Bubbles.Add(bubble);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = bubble.Id }, bubble);
        }

        // DELETE: api/Bubble/5
        [ResponseType(typeof(Bubble))]
        public IHttpActionResult DeleteBubble(int id)
        {
            Bubble bubble = db.Bubbles.Find(id); // TBD - Security.
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