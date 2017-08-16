using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using PopditWebApi;
using System.Web;
using Newtonsoft.Json;

namespace PopditWeb.Controllers
{
    public class BubbleController : Controller
    {
        // UPLOAD/DOWNLOAD
        [HttpPost]
        public async Task<ActionResult> UpDown(HttpPostedFileBase file, string submit)
        {
            string download = "";
            if (submit == "download")
            {
                string json = await WebApi(WebApiMethod.Get, "api/Bubble");
                List<BubbleInterop> bubbles = JsonConvert.DeserializeObject<List<BubbleInterop>>(json);
                //DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<BubbleInterop>));
                //List<BubbleInterop> bubbles = (List<BubbleInterop>)serializer.ReadObject(json);

                // Set up the first row.
                download = "Change\tResult\tID\tInternalID\tName\tMessage\tPhone\tUrl\tAddress\tRadiusID\tActive\n";

                foreach (BubbleInterop b in bubbles)
                {
                    string line = "";
                    line =
                        "\t\t" +
                        b.Id.ToString() + "\t" +
                        b.InternalId.ToString() + "\t" +
                        b.Name + "\t" +
                        b.AlertMsg + "\t" +
                        b.Phone + "\t" +
                        b.Url + "\t" +
                        b.Address + "\t" +
                        b.RadiusId.ToString() + "\t" +
                        b.Active.ToString() + "\t" +
                        "\n";
                    download = download + line;
                }
            }
            if (submit == "upload")
            {
                if (file != null && file.ContentLength > 0)
                {
                    // Profile - flags
                    string jsonProfile = await WebApi(WebApiMethod.Get, "api/Profile");
                    List<ProfileInterop> profiles = JsonConvert.DeserializeObject<List<ProfileInterop>>(jsonProfile);
                    //DataContractJsonSerializer serializerProfile = new DataContractJsonSerializer(typeof(List<ProfileInterop>));
                    //List<ProfileInterop> profiles = (List<ProfileInterop>)serializerProfile.ReadObject(jsonProfile);
                    int userId = profiles[0].Id;

                    StreamReader reader = new StreamReader(file.InputStream, Encoding.UTF8);
                    bool firstLine = true;
                    while (!reader.EndOfStream)
                    {
                        // Splitthe line into fields.
                        string inputLine = reader.ReadLine();
                        string line = "";
                        string[] fields = inputLine.Split('\t');
                        // If it's the first line, or if no change is desired, or if the change was already made, just copy input to output.
                        if (firstLine || fields[0].Length == 0 || fields[1].Length > 0) line = inputLine + "\n";
                        // Otherwise, process the line.
                        else
                        {
                            // Set up bubble.
                            BubbleInterop b = new BubbleInterop();
                            if (fields[2].Length == 0) fields[2] = "0";
                            b.Id = Convert.ToInt32(fields[2]);
                            if (fields[3].Length == 0) fields[3] = "0";
                            b.InternalId = Convert.ToInt32(fields[3]);
                            b.Name = fields[4];
                            b.AlertMsg = fields[5];
                            b.Phone = fields[6];
                            b.Url = fields[7];
                            b.Address = fields[8];
                            if (fields[9].Length == 0) fields[9] = "1";
                            b.RadiusId = Convert.ToInt32(fields[9]);
                            b.Active = Convert.ToBoolean(fields[10]);
                            b.ProfileId = userId;

                            try
                            {
                                switch (fields[0])
                                {
                                    case "CREATE":
                                        await WebApi(WebApiMethod.Post, "api/Bubble", b);
                                        break;
                                    case "UPDATE":
                                        await WebApi(WebApiMethod.Put, "api/Bubble/" + b.Id.ToString(), b);
                                        break;
                                    case "DELETE":
                                        await WebApi(WebApiMethod.Delete, "api/Bubble/" + b.Id.ToString());
                                        break;
                                }
                                fields[1] = "DONE";
                            }
                            catch (Exception e) { fields[1] = "ERROR"; }
                            
                            for (int i = 0; i < fields.Length; i++) line = line + fields[i] + "\t";
                            line.TrimEnd('\t');
                            line = line + "\n";                            
                        }
                        firstLine = false;
                        download = download + line;
                    }
                }
            }

            // Download the result.
            byte[] array = Encoding.UTF8.GetBytes(download);
            return File(array, "text/plain", "PopditBubbles.txt");
        }

        // GET: Bubble/Index
        public async Task<ActionResult> Index()
        {
            try
            {
                // Profile - flags
                string jsonProfile = await WebApi(WebApiMethod.Get, "api/Profile");
                List<ProfileInterop> profiles = JsonConvert.DeserializeObject<List<ProfileInterop>>(jsonProfile);
                string flags = profiles[0].Flags;
                ViewData["UpDown"] = false;
                if (flags != null) ViewData["UpDown"] = (flags.Contains("UD")); // Upload/Download flag.

                // Radii
                string jsonRadii = await WebApi(WebApiMethod.Get, "api/Radius");
                List<RadiusInterop> radii = JsonConvert.DeserializeObject<List<RadiusInterop>>(jsonRadii);
                ViewData["Radii"] = radii;

                string json = await WebApi(WebApiMethod.Get, "api/Bubble");
                List<BubbleInterop> bubbles = JsonConvert.DeserializeObject<List<BubbleInterop>>(json);
                return View(bubbles);
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("Index", "Home"); }
        }

        // POST: Bubble/Create
        [HttpPost]
        public async Task<ActionResult> Create(FormCollection collection)
        {
            try
            {
                // Get RadiusId key.
                int j = 0;
                while (j < collection.Keys.Count && !collection.Keys[j].Contains("RadiusId")) j++;

                // Create new bubble from FormCollection
                BubbleInterop b = new BubbleInterop();
                b.Name = collection["Name"].ToString();
                b.AlertMsg = collection["AlertMsg"].ToString();
                b.RadiusId = ConvertToInt(collection[j]);
                b.Active = collection["Active"].Contains("true");
                b.Address = collection["Address"].ToString();
                b.Phone = collection["Phone"];
                b.Url = collection["Url"];

                /*
                string lat = collection["Latitude"].ToString();
                if (lat.Length == 0) lat = "0";
                b.Latitude = double.Parse(lat);
                string lng = collection["Latitude"].ToString();
                if (lng.Length == 0) lng = "0";
                b.Longitude = double.Parse(lng);
                */

                await WebApi(WebApiMethod.Post, "api/Bubble", b);
                return RedirectToAction("Index", "Bubble");
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("Index", "Home"); }
        }

        // POST: Bubble/Update/5
        [HttpPost]
        public async Task<ActionResult> Update(int id, FormCollection collection)
        {
            try
            {
                // UPDATE
                if (collection["command"].Equals("Update"))
                {
                    // Get RadiusId key.
                    int j = 0;
                    while (j < collection.Keys.Count && !collection.Keys[j].Contains("RadiusId")) j++;

                    // TBD - this is a hack for testing.
                    BubbleInterop b = new BubbleInterop();
                    b.Id = id;
                    b.ProfileId = ConvertToInt(collection["ProfileId"]);
                    b.Name = collection["Name"].ToString();
                    b.AlertMsg = collection["AlertMsg"].ToString();
                    b.RadiusId = ConvertToInt(collection[j]);
                    b.Active = collection["Active"].Contains("true");
                    b.Address = collection["Address"].ToString();
                    b.Phone = collection["Phone"];
                    b.Url = collection["Url"];

                    /*
                    string lat = collection["Latitude"].ToString();
                    if (lat.Length == 0) lat = "0";
                    b.Latitude = double.Parse(lat);
                    string lng = collection["Latitude"].ToString();
                    if (lng.Length == 0) lng = "0";
                    b.Longitude = double.Parse(lng);
                    */

                    await WebApi(WebApiMethod.Put, "api/Bubble/" + id.ToString(), b);
                }
                // DELETE
                if (collection["command"].Equals("Confirm deletion"))
                    await WebApi(WebApiMethod.Delete, "api/Bubble/" + id.ToString());

                return RedirectToAction("Index");
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("Index", "Home"); }
        }
    }
}
