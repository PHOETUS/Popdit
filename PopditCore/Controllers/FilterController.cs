using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using PopditDB.Models;
using System;

namespace PopditCore.Controllers
{
    public class FilterController : ApiController
    {
        private PopditDBEntities db = new PopditDBEntities();
        private int PublicKeyBase = 62;

        // GET: api/Filter
        public System.Web.Http.Results.JsonResult<List<Filter>> GetFilters()
        {
            return Json(db.Filters.Where(m => m.ProfileId == AuthenticatedUserId).OrderBy(m => m.Name).ToList());  // Security.
        }

        /*
        // GET: api/Filter/5
        [ResponseType(typeof(Filter))]
        public System.Web.Http.Results.JsonResult<Filter> GetFilter(int id)
        {
            Filter filter = db.Filters.Find(id);  // TBD - Security.
            return Json(filter);            
        }
        */

        // PUT: api/Filter/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFilter(int id, Filter newFilter)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (id != newFilter.Id) { return BadRequest(); }

            // Change only the changed fields in the filter.
            // Only the fields below are changeable via the API.
            Filter oldFilter = db.Filters.Find(id);  // TBD - Security.
            oldFilter.Name = newFilter.Name ?? oldFilter.Name;
            oldFilter.ProfileId = newFilter.ProfileId;
            oldFilter.CategoryId = newFilter.CategoryId ?? oldFilter.CategoryId;
            oldFilter.ScheduleId = newFilter.ScheduleId ?? oldFilter.ScheduleId;
            oldFilter.RadiusId = newFilter.RadiusId; 
            oldFilter.Active = newFilter.Active;
            oldFilter.PublicKey = DecimalToArbitrarySystem(id, PublicKeyBase);

            db.Entry(oldFilter).State = EntityState.Modified;

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
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            filter.ProfileId = AuthenticatedUserId;  // Security.
            db.Filters.Add(filter);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = filter.Id }, filter);
        }

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

        /// <summary>
        /// Converts the given number from the numeral system with the specified
        /// radix (in the range [2, 36]) to decimal numeral system.
        /// </summary>
        /// <param name="number">The arbitrary numeral system number to convert.</param>
        /// <param name="radix">The radix of the numeral system the given number
        /// is in (in the range [2, 36]).</param>
        /// <returns></returns>
        public static long ArbitraryToDecimalSystem(string number, int radix)
        {
            const string Digits = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (radix < 2 || radix > Digits.Length)
                throw new ArgumentException("The radix must be >= 2 and <= " +
                    Digits.Length.ToString());

            if (String.IsNullOrEmpty(number))
                return 0;

            // Make sure the arbitrary numeral system number is in upper case
            number = number.ToUpperInvariant();

            long result = 0;
            long multiplier = 1;
            for (int i = number.Length - 1; i >= 0; i--)
            {
                char c = number[i];
                if (i == 0 && c == '-')
                {
                    // This is the negative sign symbol
                    result = -result;
                    break;
                }

                int digit = Digits.IndexOf(c);
                if (digit == -1)
                    throw new ArgumentException(
                        "Invalid character in the arbitrary numeral system number",
                        "number");

                result += digit * multiplier;
                multiplier *= radix;
            }

            return result;
        }

        /// <summary>
        /// Converts the given decimal number to the numeral system with the
        /// specified radix (in the range [2, 36]).
        /// </summary>
        /// <param name="decimalNumber">The number to convert.</param>
        /// <param name="radix">The radix of the destination numeral system (in the range [2, 36]).</param>
        /// <returns></returns>
        public static string DecimalToArbitrarySystem(long decimalNumber, int radix)
        {
            const int BitsInLong = 64;
            const string Digits = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (radix < 2 || radix > Digits.Length)
                throw new ArgumentException("The radix must be >= 2 and <= " + Digits.Length.ToString());

            if (decimalNumber == 0)
                return "0";

            int index = BitsInLong - 1;
            long currentNumber = Math.Abs(decimalNumber);
            char[] charArray = new char[BitsInLong];

            while (currentNumber != 0)
            {
                int remainder = (int)(currentNumber % radix);
                charArray[index--] = Digits[remainder];
                currentNumber = currentNumber / radix;
            }

            string result = new String(charArray, index + 1, BitsInLong - index - 1);
            if (decimalNumber < 0)
            {
                result = "-" + result;
            }

            return result;
        }
    }
}