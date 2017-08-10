using System;
using System.Text;
using System.Linq;
using PopditDB.Models;

namespace PopditPop.Controllers
{
    public class ApiController : System.Web.Http.ApiController
    {
        private Entities db = new Entities(); // TBD - Singleton?

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
                }
                catch { }  // If anything goes wrong, just return 0.
                return id;
            }
        }
    }
}