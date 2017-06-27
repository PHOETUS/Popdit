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
using DBLayer.Models;

namespace PopditCore.Controllers
{
    public class RadiusController : ApiController
    {
        private PopditDBEntities db = new PopditDBEntities();

        // GET: api/Radius
        public IQueryable<Radius> GetRadii()
        {
            return db.Radii;
        }

        // GET: api/Radius/5
        [ResponseType(typeof(Radius))]
        public IHttpActionResult GetRadius(int id)
        {
            Radius radius = db.Radii.Find(id);
            if (radius == null)
            {
                return NotFound();
            }

            return Ok(radius);
        }

        /*
        // PUT: api/Radius/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRadius(int id, Radius radius)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != radius.Id)
            {
                return BadRequest();
            }

            db.Entry(radius).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RadiusExists(id))
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

        // POST: api/Radius
        [ResponseType(typeof(Radius))]
        public IHttpActionResult PostRadius(Radius radius)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Radii.Add(radius);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = radius.Id }, radius);
        }

        // DELETE: api/Radius/5
        [ResponseType(typeof(Radius))]
        public IHttpActionResult DeleteRadius(int id)
        {
            Radius radius = db.Radii.Find(id);
            if (radius == null)
            {
                return NotFound();
            }

            db.Radii.Remove(radius);
            db.SaveChanges();

            return Ok(radius);
        }
        */

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RadiusExists(int id)
        {
            return db.Radii.Count(e => e.Id == id) > 0;
        }
    }
}