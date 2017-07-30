using System;
using System.Linq;
using System.Text;
using PopditDB.Models;
using System.Net.Http;
using System.Web.Http;
using System.Net;

namespace PopditCore.Controllers
{
    public class ApiController : System.Web.Http.ApiController
    {
        private PopditDBEntities db = new PopditDBEntities(); // TBD - Singleton?

        protected int AuthenticatedUserId
        {
            get
            {
                // TBD HACK - This would be a good place to handle authentication failure.
                string phone = "";
                string pwd = "";
                int id = 0;
                try
                {
                    string auth = Request.Headers.GetValues("Authorization").ToList()[0];
                    if (auth != null && auth.StartsWith("Basic"))
                    {
                        auth = auth.Substring("Basic ".Length).Trim();  // Trim off "Basic ".
                        Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                        string uidPwd = encoding.GetString(Convert.FromBase64String(auth));
                        int separatorIndex = uidPwd.IndexOf(':');
                        phone = uidPwd.Substring(0, separatorIndex);
                        pwd = uidPwd.Substring(separatorIndex + 1);
                    }
                    id = db.Profiles.Single(p => p.Phone == phone && p.Password == pwd).Id;
                    return id;
                }
                catch
                {
                    var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Authentication failed.  Please sign in." };
                    throw new HttpResponseException(msg);
                }                
            }
        }
    }
}