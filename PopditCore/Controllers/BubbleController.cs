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
        private Entities db = new Entities();

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

        BubbleInterop ToInterop(Bubble b)
        {
            BubbleInterop bi = new BubbleInterop();
            bi.Active = b.Active;
            bi.Address = b.Address;
            bi.AlertMsg = b.AlertMsg;
            bi.Id = b.Id;
            bi.Latitude = b.Latitude;
            bi.Longitude = b.Longitude;
            bi.Name = b.Name;
            bi.ProfileId = b.ProfileId;
            bi.RadiusId = b.RadiusId;
            bi.Phone = b.Phone;
            bi.Url = b.Url;
            return bi;
        }

        Bubble FromInterop(BubbleInterop bi)
        {
            Bubble b = new Bubble();
            b.Active = bi.Active;
            b.Address = bi.Address;
            b.AlertMsg = bi.AlertMsg;
            b.Id = bi.Id;
            b.Latitude = bi.Latitude;
            b.Longitude = bi.Longitude;
            b.Name = bi.Name;
            b.ProfileId = bi.ProfileId;
            b.RadiusId = bi.RadiusId;
            b.Phone = bi.Phone;
            b.Url = bi.Url;
            return b;
        }

        // INDEX
        // GET: api/Bubble
        public System.Web.Http.Results.JsonResult<List<BubbleInterop>> GetBubbles()
        {
            List<Bubble> bubbles = db.Bubbles.Where(m => m.ProfileId == AuthenticatedUserId).OrderBy(m => m.Name).ToList(); // Security.
            List<BubbleInterop> interops = new List<BubbleInterop>();
            foreach (Bubble b in bubbles) interops.Add(ToInterop(b));
            return Json(interops);
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

        // UPDATE
        // PUT: api/Bubble/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBubble(int id, BubbleInterop newBubble)
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
            oldBubble.RadiusId = newBubble.RadiusId;
            oldBubble.Active = newBubble.Active;
            oldBubble.Phone = newBubble.Phone ?? oldBubble.Phone;
            oldBubble.Url = newBubble.Url ?? oldBubble.Url;

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

        // CREATE
        // POST: api/Bubble
        [ResponseType(typeof(Bubble))]
        public IHttpActionResult PostBubble(BubbleInterop bi)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            bi.ProfileId = AuthenticatedUserId; // Security

            // if there's an address, geocode it into the lat/long.
            if (bi.Address != null && bi.Address.Length > 0)
            {
                Location loc = Geocode(bi.Address).Result;
                bi.Latitude = loc.Latitude;
                bi.Longitude = loc.Longitude;
            }

            Bubble b = FromInterop(bi);

            b.Radius = db.Radii.Find(bi.RadiusId);  // TBD - too expensive
            b.UpdateMaxMin();
            db.Bubbles.Add(b);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = b.Id }, b);
        }

        // DELETE
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

            return StatusCode(HttpStatusCode.NoContent);
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