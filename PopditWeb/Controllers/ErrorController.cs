using System;
using System.Web.Mvc;
using PopditWebApi;
using System.Threading.Tasks;

namespace PopditWeb.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public async Task<ActionResult> Index()
        {
            Exception e = Server.GetLastError();
            LogEventInterop lei = new LogEventInterop();
            lei.Title = "Web error";
            lei.Message = e.Message;
            lei.InnerException = e.InnerException.Message;
            lei.StackTrace = e.StackTrace;
            await WebApi(WebApiMethod.Post, "api/LogEvent", lei);

            return View("Error");
        }
    }
}