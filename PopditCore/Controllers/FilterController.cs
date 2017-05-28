using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using PopditCore.Models;

namespace PopditCore.Controllers
{
    public class FilterController : ApiController
    {
        private PopditDBEntities db = new PopditDBEntities();

        // GET: api/Filter
        public System.Web.Http.Results.JsonResult<List<Models.Filter>> GetFilters()
        {
            return Json(db.Filters.Where(m => m.ProfileId == AuthenticatedUserId).ToList());
        }

        // GET: api/Filter/5
        [ResponseType(typeof(Models.Filter))]
        public System.Web.Http.Results.JsonResult<Models.Filter> GetFilter(int id)
        {
            Models.Filter filter = db.Filters.Find(id);
            return Json(filter);            
        }

        // PUT: api/Filter/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFilter(int id, Filter newFilter)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (id != newFilter.Id) { return BadRequest(); }

            db.Entry(newFilter).State = EntityState.Modified;

            try { db.SaveChanges(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilterExists(id)) { return NotFound(); }
                else { throw; }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Filter
        [ResponseType(typeof(Filter))]
        public IHttpActionResult PostFilter(Filter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Filters.Add(filter);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = filter.Id }, filter);
        }

        // DELETE: api/Filter/5
        [ResponseType(typeof(Filter))]
        public IHttpActionResult DeleteFilter(int id)
        {
            Filter filter = db.Filters.Find(id);
            if (filter == null)
            {
                return NotFound();
            }

            db.Filters.Remove(filter);
            db.SaveChanges();

            return Ok(filter);
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