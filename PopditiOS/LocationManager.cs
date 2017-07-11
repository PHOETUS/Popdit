using System.Collections.Generic;
using PopditiOS.Models;
using CoreLocation;
using UIKit;
using Foundation;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace PopditiOS
{
    public class LocationManager
    {
        protected CLLocationManager LocMgr;
        string SecToken = "8126502080";
        const int bubbleZoneRadius = 3000; // Radius in meters.  TBD - move to config
        const int refreshZoneRadius = 2000; // Radius in meters.  TBD - move to config
        List<BubbleMobile> BubbleCatalog;

        public event System.EventHandler<BubblePoppedEventArgs> BubblePopped = delegate { };

        // Handle the event throws when a bubble is popped.
        public void HandleBubblePopped(BubblePoppedEventArgs e)
        {
            // If the zone was popped, refresh the catalog.
            RefreshBubbleCatalog();
            // else if a bubble was popped, notify the server and display the notification string to the user.
            Pop();
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

        // A bubble was popped.  Notify the server and display the notification string to the user.
        void Pop()
        {
            // If the user left the refresh zone, then refresh the bubble catalog.
            // Otherwise, if a location bubble was popped, notify the server and display a notification message.
        }

        // The user left the refresh zone.  Get a new catalog of bubbles from the server.
        async void RefreshBubbleCatalog()
        {
            // Get the user's location and code it as the bubble zone.
            CLLocation location = LocMgr.Location;
            Zone bubbleZone = new Zone();
            bubbleZone.Latitude = (decimal)location.Coordinate.Latitude; // TBD double
            bubbleZone.Longitude = (decimal)location.Coordinate.Longitude; // TBD double
            bubbleZone.Radius = bubbleZoneRadius;
            // Define a zone and set and event handler for it.
            // Get all the bubbles in the zone and set event handlers for them.
            Stream json = await WebApiPost("api/Bubble", bubbleZone);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<BubbleMobile>));
            BubbleCatalog = (List<BubbleMobile>)serializer.ReadObject(json);
            // Create a new bubble to represent the new refresh zone and add it to the bubble catalog.
            // When the user pops it from inside, we will refresh again.
        }

        protected async Task<Stream> WebApiPost(string servicePath, Object content)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:83/"); // TBD - move to config
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", SecurityToken);                    
                    string json = JsonConvert.SerializeObject(content);
                    StringContent sc = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(servicePath, sc).ConfigureAwait(false);
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
                    client.BaseAddress = new Uri("http://localhost:83/"); // TBD - make configurable
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", SecurityToken);  // TBD - this is just a hack for testing

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

        public void StartLocationUpdates() { RefreshBubbleCatalog(); } // Kick off the first refresh.

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