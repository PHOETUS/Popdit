using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PopditDB.Models;
using PopditWebApi;

namespace PopditCore.Controllers
{
    public class LogEventsController : ApiController
    {
        private Entities db = new Entities();

        LogEventInterop ToInterop(LogEvent le)
        {
            LogEventInterop lei = new LogEventInterop();
            lei.Message = le.Message;
            lei.InnerException = le.InnerException;
            return lei;
        }

        LogEvent FromInterop(LogEventInterop lei)
        {
            LogEvent le = new LogEvent();
            le.Message = lei.Message;
            le.InnerException = lei.InnerException;
            return le;
        }

        // CREATE
        // POST: api/LogEvents
        [ResponseType(typeof(LogEventInterop))]
        public async Task<IHttpActionResult> PostLogEvent(LogEventInterop lei)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            LogEvent le = FromInterop(lei);

            db.LogEvents.Add(le);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = le.Id }, lei);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LogEventExists(int id)
        {
            return db.LogEvents.Count(e => e.Id == id) > 0;
        }

        /*
        // GET: api/LogEvents
        public IQueryable<LogEvent> GetLogEvents()
        {
            return db.LogEvents;
        }

        // GET: api/LogEvents/5
        [ResponseType(typeof(LogEvent))]
        public async Task<IHttpActionResult> GetLogEvent(int id)
        {
            LogEvent logEvent = await db.LogEvents.FindAsync(id);
            if (logEvent == null)
            {
                return NotFound();
            }

            return Ok(logEvent);
        }

        // PUT: api/LogEvents/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLogEvent(int id, LogEvent logEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != logEvent.Id)
            {
                return BadRequest();
            }

            db.Entry(logEvent).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogEventExists(id))
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

        // DELETE: api/LogEvents/5
        [ResponseType(typeof(LogEvent))]
        public async Task<IHttpActionResult> DeleteLogEvent(int id)
        {
            LogEvent logEvent = await db.LogEvents.FindAsync(id);
            if (logEvent == null)
            {
                return NotFound();
            }

            db.LogEvents.Remove(logEvent);
            await db.SaveChangesAsync();

            return Ok(logEvent);
        }
        */
    }
}