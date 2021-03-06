﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace PopditWeb.Controllers
{
    public class Controller : System.Web.Mvc.Controller
    {
        protected List<Object> mList;

        string BasicAuthString()
        {
            String uidPwd = "";
            try
            {
                string uid = HttpContext.Request.Cookies["Popdit"].Values["Phone"];
                string pwd = HttpContext.Request.Cookies["Popdit"].Values["Password"];
                uidPwd = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(uid + ":" + pwd));
            }
            catch { } // Swallow exception - if something went wrong, just return an empty auth string.
            return "Basic " + uidPwd;
        }

        public static int ConvertToInt(string x)
        {
            if (x == null) return 0;
            if (x.Length == 0) return 0;
            Int32.TryParse(x, out int r);
            return r;
        }

        public static int? ConvertToNullableInt(string x)
        {
            if (x == null) return null;
            if (x.Length == 0) return null;
            Int32.TryParse(x, out int r);
            return r;
        }

        protected enum WebApiMethod { Get, Post, Put, Delete }

        protected async Task<string> WebApi(WebApiMethod method, string servicePath, Object content = null)
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
                response.StatusCode == System.Net.HttpStatusCode.NoContent ||
                response.StatusCode == System.Net.HttpStatusCode.Created)
                return response.Content.ReadAsStringAsync().Result;
            else
                throw new Exception("API call failed. " + response.StatusCode + " " + response.ReasonPhrase);
        }
    }
}