using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using PopditDB.Models;
using PopditWebApi;

namespace PopditCore.Controllers
{
    public class RadiusController : ApiController
    {
        private PopditDBEntities db = new PopditDBEntities();

        RadiusInterop ToInterop(Radius r)
        {
            RadiusInterop ri = new RadiusInterop();
            ri.Degrees = r.Degrees;
            ri.Description = r.Description;
            ri.Id = r.Id;
            ri.Meters = r.Meters;
            return ri;
        }

        Radius FromInterop(RadiusInterop ri)
        {
            Radius r = new Radius();
            r.Degrees = ri.Degrees;
            r.Description = ri.Description;
            r.Id = ri.Id;
            r.Meters = ri.Meters;
            return r;
        }

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