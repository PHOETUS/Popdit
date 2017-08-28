using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PopditDB.Models;
using PopditWebApi;

namespace PopditCore.Controllers
{
    public class FriendController : ApiController
    {
        private Entities db = new Entities();

        Friend ToInterop(Profile p)
        {
            Friend f = new Friend();
            f.Id = p.Id;
            f.Nickname = p.Nickname;
            f.Tagline = p.Tagline;
            return f;
        }

        // DETAILS
        // GET: api/Friend
        public IQueryable<Friend> GetFriends()
        {
            List<Friend> interops = new List<Friend>();
            foreach (Friendship friendship in db.Profiles.Find(AuthenticatedUserId).Friendshipz)
                interops.Add(ToInterop(friendship.Profile));           
            return interops.AsQueryable<Friend>();
        }

        // CREATE
        // POST: api/Friend
        [ResponseType(typeof(void))]
        public  IHttpActionResult PostProfile(Friend f)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Get friend.
            Profile friend = db.Profiles.Single(p => p.Nickname == f.Nickname);
            // Get user.
            Profile user = db.Profiles.Find(AuthenticatedUserId);

            if (!FriendshipExists(user.Id, friend.Id))
            {
                // Add friendship.
                Friendship friendship = new Friendship();
                friendship.ProfileIdOwner = user.Id;
                friendship.ProfileIdOwned = friend.Id;
                user.Friendshipz.Add(friendship);
                db.SaveChanges();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE
        // DELETE: api/Friend/5
        [ResponseType(typeof(Friend))]
        public async Task<IHttpActionResult> DeleteProfile(int id)
        {
            int userId = AuthenticatedUserId;
            Friendship friendship = db.Friendships.Single(f => f.ProfileIdOwner == userId && f.ProfileIdOwned == id);
            db.Friendships.Remove(friendship);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProfileExists(int id)
        {
            return db.Profiles.Count(e => e.Id == id) > 0;
        }

        private bool FriendshipExists(int ownerId, int friendId)
        {
            return db.Friendships.Count(f => f.ProfileIdOwner == ownerId && f.ProfileIdOwned == friendId) > 0;
        }

        /*
        // UPDATE
        // PUT: api/Friend/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProfile(int id, Profile profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profile.Id)
            {
                return BadRequest();
            }

            db.Entry(profile).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
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

        // DETAILS
        // GET: api/Friend/5
        [ResponseType(typeof(Profile))]
        public async Task<IHttpActionResult> GetProfile(int id)
        {
            Profile profile = await db.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }
        */
    }
}