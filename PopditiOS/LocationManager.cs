using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Foundation;
using UIKit;
using CoreLocation;
using UserNotifications;
using PopditMobile.Models;
using System.Diagnostics;

namespace PopditiOS
{
    public class LocationManager
    {
        protected CLLocationManager LocMgr;
        string SecToken = "8126502080";
        const int bubbleZoneRadius = 10000; // Radius in meters, minimum 3300m (> 2 miles) to ensure working algorithm.  TBD - move to config
        const int refreshZoneRadius = 9000; // Radius in meters.  TBD - move to config
        List<BubbleMobile> BubbleCatalog;

        protected async Task<Stream> WebApiPost(string servicePath, Object content)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://192.168.1.106:83/"); // TBD - move to config
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", SecurityToken);
                    string json = JsonConvert.SerializeObject(content);
                    StringContent sc = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(servicePath, sc).ConfigureAwait(false);
                    return response.Content.ReadAsStreamAsync().Result;
                }
                catch (Exception e)
                {
                    // TBD - Handle error
                    Debug.WriteLine(">>>>> Error in WebApiPost: " + e.Message);
                    return null;
                }
            }
        }

        // A location bubble was popped.  Notify the server and display the notification string to the user.
        async void Pop(object sender, CLRegionEventArgs e)
        {
            if (!e.Region.Identifier.Equals("RefreshZone")) // Ignore the refresh zone.
            {
                // Get the ID of the bubble that was popped.
                int bubbleId = Int32.Parse(e.Region.Identifier);
                Debug.WriteLine(">>>>> Popped bubble #" + bubbleId.ToString());
                // Notify the server and get notification message.
                EventMobile localEvent = new EventMobile();
                localEvent.BubbleId = bubbleId;
                localEvent.TimestampJson = DateTime.Now.ToShortTimeString();
                Stream json = await WebApiPost("api/Event", localEvent);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(EventMobile));
                EventMobile serverEvent = (EventMobile)serializer.ReadObject(json);
                // Display the notification.
                var content = new UNMutableNotificationContent();
                content.Title = serverEvent.ProviderName;  // Name of provider.
                                                           // content.Subtitle = "Your prescription is ready";  // Not implemented.
                content.Body = serverEvent.Msg;
                //content.Badge = 1; // Not implemented.

                var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false); // 1 second delay, do not repeat.

                var requestID = "Popdit" + " " + e.Region.Identifier;
                var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

                UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
                {
                    if (err != null)
                    {
                            // Do something with error...
                        }
                });
            }
        }

        // The user left the refresh zone.  Get a new catalog of bubbles from the server.
        async void Refresh(object sender, CLRegionEventArgs e)
        {
            if (e == null || e.Region.Identifier.Equals("RefreshZone")) // Ignore location bubbles.
            {
                // Get the user's location and code it as the bubble zone.
                CLLocation location = LocMgr.Location;
                double latitude = location.Coordinate.Latitude; // TBD double
                double longitude = location.Coordinate.Longitude; // TBD double
                Debug.WriteLine(">>>>> Refreshing at " + latitude.ToString() + ", " + longitude.ToString());
                Zone bubbleZone = new Zone((decimal)latitude, (decimal)longitude, bubbleZoneRadius);
                // Get all the bubbles in the zone.
                Stream json = await WebApiPost("api/Bubble", bubbleZone);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<BubbleMobile>));
                BubbleCatalog = (List<BubbleMobile>)serializer.ReadObject(json);
                // Set an event handler for each bubble in the new list.
                foreach (BubbleMobile bubble in BubbleCatalog)
                {
                    CLCircularRegion bubbleRegion = new CLCircularRegion(new CLLocationCoordinate2D((double)bubble.Latitude, (double)bubble.Longitude), bubble.Radius, bubble.Id.ToString());
                    LocationMgr.StartMonitoring(bubbleRegion);
                    bubble.Region = bubbleRegion;
                    Debug.WriteLine(">>>>> Adding bubble #" + bubble.Id.ToString());
                }
                // Redefine the refresh zone and give it this event handler.
                CLCircularRegion refreshRegion = new CLCircularRegion(new CLLocationCoordinate2D(latitude, longitude), refreshZoneRadius, "RefreshZone");
                LocationMgr.StartMonitoring(refreshRegion);
            }
        }

        // Get the security token for this mobile device.  Try this object, then the cookie, then the server.
        // If this function fails, the user needs to sign in.
        // TBD - Add error handling to force sign in.
        string SecurityToken
        {
            get
            {
                // If security token is not initialized in this object ...
                if (SecToken.Length == 0)
                {
                    // Try to get the security token from the cookie.
                    SecToken = GetValueForPopditCookieKey("Phone"); // TBD s/b "SecurityToken".
                                                                    // If security token is not found in the cookie ...
                    if (SecToken.Length == 0)
                    {
                        // Get the security token from the server.
                        string phone = GetValueForPopditCookieKey("Phone");
                        string pwd = GetValueForPopditCookieKey("Password");
                        // Initialize security token in this object.
                        SecToken = GetSecurityTokenFromServer(phone, pwd);
                        // Initialize security token in the cookie.
                        SetValueForPopditCookie("Phone", SecToken); // TBD s/b "SecurityToken".
                    }
                }
                return SecToken;
            }
        }

        NSHttpCookie GetPopditCookie()
        {
            // Get the cookies.
            NSHttpCookie[] cookies = NSHttpCookieStorage.SharedStorage.Cookies;
            // Find the Popdit cookie.
            NSHttpCookie cookie = null;
            foreach (NSHttpCookie c in cookies)
            {
                if (c.Name == "Popdit") cookie = c;
                break;
            }
            return cookie;
        }

        void SetValueForPopditCookie(string key, string value)
        {
            GetPopditCookie().SetValueForKey(NSObject.FromObject(value), (NSString)key);
        }

        string GetValueForPopditCookieKey(string key)
        {
            string value = "";
            try
            {
                NSHttpCookie cookie = GetPopditCookie();
                // Get the "value" string from the cookie, something like "key1=xyz&key2=abc&key3=pdq".
                string cookieValueString = (NSString)cookie.ValueForKey(new NSString("value")).ToString();
                // Split the value string into key-value substrings, like "key1=xyz".
                string[] cookieKeyValuePairs = cookieValueString.Split('&');
                // Find the key-value pair for the given key, like "SecurityToken=JWDCh*34fd-3h42".
                string desiredKeyValuePair = "";
                int i = 0;
                do { desiredKeyValuePair = cookieKeyValuePairs[i++]; }
                while (!desiredKeyValuePair.Contains(key));
                // Parse out the value from the SecurityToken key-value pair and initialize the security token in this object.
                value = desiredKeyValuePair.Split('=')[1];
            }
            catch { } // No cookie, no Popdit cookie, no value string, no instance of desired key, no value for desired key - whatever ...
            return value;
        }

        // Initialize token from server.
        string GetSecurityTokenFromServer(string phone, string pwd)
        {
            string token = "";
            // Get token TBD add code here ...
            return token;
        }

        public void Start()
        {
            Debug.WriteLine(">>>>> Start");
            LocationMgr.RegionLeft += Refresh;
            LocationMgr.RegionEntered += Pop;
            // Kick off the first refresh.
            Refresh(null, null);
        }

        public LocationManager()
        {
            this.LocMgr = new CLLocationManager();
            this.LocMgr.PausesLocationUpdatesAutomatically = false;
            this.LocMgr.DesiredAccuracy = CLLocation.AccuracyNearestTenMeters;

            // iOS 8 has additional permissions requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                LocMgr.RequestAlwaysAuthorization(); // works in background
                                                     // locMgr.RequestWhenInUseAuthorization (); // only in foreground
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                LocMgr.AllowsBackgroundLocationUpdates = true;
            }
        }

        public CLLocationManager LocationMgr { get { return this.LocMgr; } }
    }
}