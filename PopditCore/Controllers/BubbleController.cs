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

namespace PopditCore.Controllers
{
    public class BubbleController : ApiController
    {
        private PopditDBEntities db = new PopditDBEntities();

        /*
        // GET: api/Bubble
        public IQueryable<Bubble> GetBubbles()
        {
            return db.Bubbles;
        }*/

        // GET: api/Filter
        public System.Web.Http.Results.JsonResult<List<Models.Bubble>> GetBubbles()
        {
            return Json(db.Bubbles.Where(m => m.ProfileId == AuthenticatedUserId).ToList());
        }

        // GET: api/Bubble/5
        [ResponseType(typeof(Bubble))]
        public IHttpActionResult GetBubble(int id)
        {
            Bubble bubble = db.Bubbles.Find(id);
            if (bubble == null)
            {
                return NotFound();
            }

            return Ok(bubble);
        }

        // PUT: api/Bubble/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBubble(int id, Bubble bubble)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bubble.Id)
            {
                return BadRequest();
            }

            db.Entry(bubble).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BubbleExists(id))
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

        // POST: api/Bubble
        [ResponseType(typeof(Bubble))]
        public IHttpActionResult PostBubble(Bubble bubble)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bubbles.Add(bubble);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = bubble.Id }, bubble);
        }

        // DELETE: api/Bubble/5
        [ResponseType(typeof(Bubble))]
        public IHttpActionResult DeleteBubble(int id)
        {
            Bubble bubble = db.Bubbles.Find(id);
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
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BubbleExists(int id)
        {
            return db.Bubbles.Count(e => e.Id == id) > 0;
        }
    }
}