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
using PopditWebApi;

namespace PopditCore.Controllers
{
    public class ProfileController : ApiController
    {
        private PopditDBEntities db = new PopditDBEntities();

        ProfileInterop ToInterop(Profile p)
        {
            ProfileInterop pi = new ProfileInterop();
            pi.CallbackAddress = p.CallbackAddress;
            pi.DOB = p.DOB;
            pi.Email = p.Email;
            pi.Id = p.Id;
            pi.Male = p.Male;
            pi.Nickname = p.Nickname;
            pi.Password = p.Password;
            pi.Phone = p.Phone;
            pi.RadiusId = p.RadiusId;
            return pi;
        }

        Profile FromInterop(ProfileInterop pi)
        {
            Profile p = new Profile();
            p.CallbackAddress = pi.CallbackAddress;
            p.DOB = pi.DOB;
            p.Email = pi.Email;
            p.Id = pi.Id;
            p.Male = pi.Male;
            p.Nickname = pi.Nickname;
            p.Password = pi.Password;
            p.Phone = pi.Phone;
            p.RadiusId = pi.RadiusId;
            return p;
        }

        // GET: api/Profile
        public System.Web.Http.Results.JsonResult<List<ProfileInterop>> GetProfiles()
        {
            List<Profile> Profiles = db.Profiles.Where(m => m.Id == AuthenticatedUserId).OrderBy(m => m.Nickname).ToList(); // Security.
            List<ProfileInterop> interops = new List<ProfileInterop>();
            foreach (Profile b in Profiles) interops.Add(ToInterop(b));
            return Json(interops);
        }

        // GET: api/Profile/5
        [ResponseType(typeof(ProfileInterop))] 
        public System.Web.Http.Results.JsonResult<ProfileInterop> GetProfile(int id)
        {
            Profile profile = db.Profiles.Find(id);  // TBD - Security.
            return Json(ToInterop(profile));
        }

        // PUT: api/Profile/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProfile(int id, ProfileInterop newProfile)
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
        [ResponseType(typeof(ProfileInterop))]
        public IHttpActionResult PostProfile(ProfileInterop pi)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            Profile profile = FromInterop(pi);

            DateTime now = DateTime.Now;
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