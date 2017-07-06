using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace PopditWeb.Controllers
{
    public class Controller : System.Web.Mvc.Controller
    {
        protected List<Object> mList;

        protected async Task<Stream> WebApiPost(string servicePath, Object content)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:81/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", HttpContext.Request.Cookies["Popdit"].Values["Phone"]);  // TBD - this is just a hack for testing

                    HttpResponseMessage response = await client.PostAsJsonAsync(servicePath, content).ConfigureAwait(false);
                    return response.Content.ReadAsStreamAsync().Result;
                }
            }
            catch (Exception e)
            {
                // TBD - Handle error
                return null;
            }
        }

        protected async Task<Stream> WebApiGet(string servicePath)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:81/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", HttpContext.Request.Cookies["Popdit"].Values["Phone"]);  // TBD - this is just a hack for testing

                    HttpResponseMessage response = await client.GetAsync(servicePath).ConfigureAwait(false);
                    return response.Content.ReadAsStreamAsync().Result;
                }
            }
            catch (Exception e)
            {
                // TBD - Handle error
                return null;
            }
        }

        protected async Task<Stream> WebApiDelete(string servicePath)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:81/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", HttpContext.Request.Cookies["Popdit"].Values["Phone"]);  // TBD - this is just a hack for testing

                    HttpResponseMessage response = await client.DeleteAsync(servicePath).ConfigureAwait(false);
                    return response.Content.ReadAsStreamAsync().Result;
                }
            }
            catch (Exception e)
            {
                // TBD - Handle error
                return null;
            }
        }

        protected async Task<Stream> WebApiPut(string servicePath, Object content)
        {            
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:81/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", HttpContext.Request.Cookies["Popdit"].Values["Phone"]);  // TBD - this is just a hack for testing

                    HttpResponseMessage response = await client.PutAsJsonAsync(servicePath, content).ConfigureAwait(false);
                    return response.Content.ReadAsStreamAsync().Result;
                }
            }
            catch (Exception e)
            {
                // TBD - Handle error
                return null;
            }
        }
    }
}