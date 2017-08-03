using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using PopditDB.Models;
using HashidsNet;

namespace PopditCore.Controllers
{
    public class FilterController : ApiController
    {
        private PopditDBEntities db = new PopditDBEntities();
        // Six-character hash, silly salt.
        static Hashids hashids = new Hashids("Sing a Song of Sixpence", 6);

        // Index
        // GET: api/Filter
        public System.Web.Http.Results.JsonResult<List<Filter>> GetFilters()
        {
            return Json(db.Filters.Where(m => m.ProfileId == AuthenticatedUserId).OrderBy(m => m.Name).ToList());  // Security.
        }

        // Update
        // PUT: api/Filter/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFilter(int id, Filter newFilter)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (id != newFilter.Id) { return BadRequest(); }

            Filter oldFilter = db.Filters.Find(id);  // TBD - Security.
            oldFilter.Name = newFilter.Name ?? oldFilter.Name;
            oldFilter.ProfileId = newFilter.ProfileId;
            oldFilter.CategoryId = newFilter.CategoryId ?? oldFilter.CategoryId;
            oldFilter.ScheduleId = newFilter.ScheduleId ?? oldFilter.ScheduleId;
            oldFilter.RadiusId = newFilter.RadiusId; 
            oldFilter.Active = newFilter.Active;        
            oldFilter.PublicKey = hashids.Encode(new int[] { oldFilter.Id });

            db.Entry(oldFilter).State = EntityState.Modified;

            try { db.SaveChanges(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilterExists(id)) { return NotFound(); }
                else { throw; }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // Create
        // POST: api/Filter
        [ResponseType(typeof(Filter))]
        public IHttpActionResult PostFilter(Filter filter)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            filter.ProfileId = AuthenticatedUserId;  // Security.
            db.Filters.Add(filter);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = filter.Id }, filter);
        }

        // Delete
        // DELETE: api/Filter/5
        [ResponseType(typeof(Filter))]
        public IHttpActionResult DeleteFilter(int id)
        {
            Filter filter = db.Filters.Find(id);  // TBD - Security.
            if (filter == null)
            {
                return NotFound();
            }

            db.Filters.Remove(filter);
            db.SaveChanges();

            return Ok(filter);
        }

        // Get
        // GET: api/Filter/5
        [ResponseType(typeof(Filter))]
        public System.Web.Http.Results.JsonResult<Filter> GetFilter(int id)
        {
            Filter filter = db.Filters.Find(id);  // TBD - Security.
            return Json(filter);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FilterExists(int id)
        {
            return db.Filters.Count(e => e.Id == id) > 0;
        }
    }
}