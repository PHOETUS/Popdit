using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace PopditInterop
{
    class Utility
    {
        public enum WebApiMethod { Get, Post, Put, Delete }

        protected async Task<Stream> WebApi(WebApiMethod method, string servicePath, Object content = null)
        {
            HttpResponseMessage response = null;      

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["CoreURL"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", BasicAuthString());

                switch (method)
                {
                    case WebApiMethod.Get:
                        response = await client.GetAsync(servicePath).ConfigureAwait(false);
                        break;
                    case WebApiMethod.Post:
                        response = await client.PostAsJsonAsync(servicePath, content).ConfigureAwait(false);
                        break;
                    case WebApiMethod.Put:
                        response = await client.PutAsJsonAsync(servicePath, content).ConfigureAwait(false);
                        break;
                    case WebApiMethod.Delete:
                        response = await client.DeleteAsync(servicePath).ConfigureAwait(false);
                        break;
                }
            }

            if (response.StatusCode == System.Net.HttpStatusCode.OK ||
                response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return response.Content.ReadAsStreamAsync().Result;
            else
                throw new Exception("API call failed. " + response.StatusCode + " " + response.ReasonPhrase);
        }
    }
}
