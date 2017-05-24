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

namespace PopditWeb.Controllers
{
    public class Controller : System.Web.Mvc.Controller
    {
        protected List<JToken> mObjectList;

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