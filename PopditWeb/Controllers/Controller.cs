using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;

namespace PopditWeb.Controllers
{
    public class Controller : System.Web.Mvc.Controller
    {
        protected List<JToken> mObjectList;

        protected List<Object> mList;

        /*
        protected bool Authenticated()
        {
            string cookieName = "Popdit";
            HttpCookie cookie = HttpContext.Request.Cookies[cookieName];
            if (cookie != null)
            {
                string phone = cookie.Values["Phone"];
                string pwd = cookie.Values["Password"];
                if (true) return true; // Authenticate against WebApi.
                else return false; // Failed authentication.
            }
            else return false;  // No cookie found.
        }
        */

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

        protected void InitializeList(string serviceAddress, string json)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create("http://localhost:81/" + serviceAddress) as HttpWebRequest;
                if (json != null)
                {
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    using (var streamwriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamwriter.Write(json);
                        streamwriter.Flush();
                    }
                }
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                    }
                    var encoding = ASCIIEncoding.ASCII;
                    using (var reader = new StreamReader(response.GetResponseStream(), encoding))
                    {
                        String responseText = reader.ReadToEnd();
                        if (responseText.First().Equals('[')) // The JSON is an array
                        { mObjectList = JArray.Parse(responseText).ToList<JToken>(); }
                        else  // The JSON is a single item.
                        {
                            mObjectList = new List<JToken>();
                            mObjectList.Add(JObject.Parse(responseText));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e.Message);
            }
        }
    }
}