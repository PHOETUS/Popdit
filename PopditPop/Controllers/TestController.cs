namespace PopditPop.Controllers
{
    public class TestController : ApiController
    {
        // GET: api/Test
        [System.Web.Http.HttpGet]
        public System.Web.Http.Results.JsonResult<string> Test()
        {
            return Json("Success!");
        }
    }
}
