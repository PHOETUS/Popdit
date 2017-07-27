using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using PopditDB.Models;

namespace PopditCore.Controllers
{
    public class ProfileController : ApiController
    {
        private PopditDBEntities db = new PopditDBEntities();

        // GET: api/Profile
        public System.Web.Http.Results.JsonResult<List<Profile>> GetProfiles()
        {
            return Json(db.Profiles.Where(m => m.Id == AuthenticatedUserId).ToList()); // Security.
        }

        // GET: api/Profile/5
        [ResponseType(typeof(Profile))] 
        public System.Web.Http.Results.JsonResult<Profile> GetProfile(int id)
        {
            Profile profile = db.Profiles.Find(id);  // TBD - Security.
            return Json(profile);
        }

        // PUT: api/Profile/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProfile(int id, Profile newProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != newProfile.Id)
            {
                return BadRequest();
            }

            // Change only the changed fields in the profile.
            // Only the fields below are changeable via the API.
            // Non-nullable fields must be supplied.
            Profile oldProfile = db.Profiles.Find(id);  // TBD - Security.
            oldProfile.Nickname = newProfile.Nickname ?? oldProfile.Nickname;
            oldProfile.Email = newProfile.Email ?? oldProfile.Email;
            oldProfile.Password = newProfile.Password ?? oldProfile.Password;
            oldProfile.Phone = newProfile.Phone ?? oldProfile.Phone;
            oldProfile.CallbackAddress = newProfile.CallbackAddress ?? oldProfile.CallbackAddress;
            oldProfile.RadiusId = newProfile.RadiusId ?? oldProfile.RadiusId;
            oldProfile.DOB = newProfile.DOB ?? oldProfile.DOB;
            oldProfile.Male = newProfile.Male ?? oldProfile.Male;

            db.Entry(oldProfile).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
                    return NotFound();
                else
                    throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Profile - This method copies a profile.
        [ResponseType(typeof(Profile))]
        public IHttpActionResult PostProfile(Profile profile)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            DateTime now = DateTime.Now;
            profile.LastSignIn = now;
            profile.Created = now;

            db.Profiles.Add(profile);

            // Clone the specified profile.
            Profile template = db.Profiles.Find(1);  // Hard-coded id.
            foreach (Filter f in template.Filters)
            {
                Filter clone = new Filter();
                clone.Name = f.Name;
                clone.CategoryId = f.CategoryId;
                clone.RadiusId = f.RadiusId;
                clone.ScheduleId = f.ScheduleId;
                clone.Active = f.Active;
                // Let the new profile own it.
                clone.ProfileId = profile.Id;
                db.Filters.Add(clone);
            }
            foreach (Bubble b in template.Bubbles)
            {
                Bubble clone = new Bubble();
                clone.AddressId = b.AddressId;
                clone.AlertMsg = b.AlertMsg;
                clone.CategoryId = b.CategoryId;
                clone.Latitude = b.Latitude;
                clone.Longitude = b.Longitude;
                clone.Name = b.Name;
                clone.RadiusId = b.RadiusId;
                clone.ScheduleId = b.ScheduleId;
                clone.Active = b.Active;
                // Let the new profile own it.
                clone.ProfileId = profile.Id;
                db.Bubbles.Add(clone);
            }

            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = profile.Id }, profile);
        }

        // DELETE: api/Profile/5
        [ResponseType(typeof(Profile))]
        public IHttpActionResult DeleteProfile(int id)
        {
            Profile profile = db.Profiles.Find(id);  // TBD - Security.
            if (profile == null)
            {
                return NotFound();
            }

            db.Profiles.Remove(profile);
            db.SaveChanges();

            return Ok(profile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }

        private bool ProfileExists(int id) { return db.Profiles.Count(e => e.Id == id) > 0; }
    }
}