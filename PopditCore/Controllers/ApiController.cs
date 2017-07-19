using System.Linq;
using PopditDB.Models;

namespace PopditCore.Controllers
{
    public class ApiController : System.Web.Http.ApiController
    {
        private PopditDBEntities db = new PopditDBEntities(); // TBD - Singleton?

        protected int AuthenticatedUserId
        {
            get
            {
                // TBD HACK - get current user Id.  This would be a good place to handle authentication failure.
                string phone = Request.Headers.GetValues("Authorization").ToList()[0];  // TBD - hack for testing
                int id = db.Profiles.Single(p => p.Phone == phone).Id;
                return id;
            }
        }
    }
}