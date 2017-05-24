using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;

namespace PopditTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Port: ");
            String port = Console.ReadLine();

            var request = (HttpWebRequest)WebRequest.Create("http://localhost:" + port + "/Fix/Create");
            request.ContentType = "application/json; charset=utf-8";
            request.Method = "POST";

            JObject j = JObject.FromObject(new 
            {
                    ProfileId = 1,
                    Latitude = 37.3,
                    Longitude = -121.92
            }
            );

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(Newtonsoft.Json.JsonConvert.SerializeObject(j));
            }
            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                Console.WriteLine(responseText);
            }
        }
    }
}
