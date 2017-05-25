using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PopditCore.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using System.ServiceModel.Channels;

namespace PopditCore.Controllers
{
    public class FilterController : ApiController
    {
        private PopditDBEntities db = new PopditDBEntities();

        // GET: api/Filter
        public System.Web.Http.Results.JsonResult<List<Models.Filter>> GetFilters() // IQueryable<Filter>
        //public System.ServiceModel.Channels. GetFilters()
        {
            return Json(db.Filters.ToList());
            //return JsonConvert.SerializeObject(db.Filters.ToList());
        }

        // GET: api/Filter/5
        [ResponseType(typeof(Filter))]
        public IHttpActionResult GetFilter(int id)
        {
            Filter filter = db.Filters.Find(id);
            if (filter == null)
            {
                return NotFound();
            }

            return Ok(filter);
        }

        // PUT: api/Filter/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFilter(int id, Filter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != filter.Id)
            {
                return BadRequest();
            }

            db.Entry(filter).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilterExists(id))
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