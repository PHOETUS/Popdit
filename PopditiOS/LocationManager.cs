﻿using System;
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
    public class LocationManager : IDisposable
    {
        protected CLLocationManager LocMgr;
        List<BubbleMobile> BubbleCatalog;
        CredentialsManager credentials = new CredentialsManager();

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
                localEvent.ProfileId = 8;
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
                content.Sound = UNNotificationSound.GetSound("bubblepop.wav");

                var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false); // 1 second delay, do not repeat.

                var requestID = "Popdit" + " " + e.Region.Identifier;
                var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

                UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
                {
                    if (err != null) { Debug.WriteLine(">>>>> Message error: " + err.DebugDescription);  }
                });                
            }
        }

        // The user left the refresh zone.  Get a new catalog of bubbles from the server.
        async void Refresh(object sender, CLRegionEventArgs e)
        {
            if (credentials.SecurityToken.Length > 0 && (e == null || e.Region.Identifier.Equals("RefreshZone"))) // Ignore location bubbles.
            {
                // Get the user's location and code it as the bubble zone.
                CLLocation location = LocMgr.Location;
                double latitude = location.Coordinate.Latitude; // TBD double
                double longitude = location.Coordinate.Longitude; // TBD double
                Debug.WriteLine(">>>>> Refreshing at " + latitude.ToString() + ", " + longitude.ToString());
                Location loc = new Location((decimal)latitude, (decimal)longitude);
                // Get all the bubbles in the zone.
                Stream json = await WebApiPost("api/Bubble", loc);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<BubbleMobile>));
                BubbleCatalog = (List<BubbleMobile>)serializer.ReadObject(json);
                // Set an event handler for each bubble in the new list.
                foreach (BubbleMobile bubble in BubbleCatalog)
                {
                    if (bubble.Id == 0) // Refresh zone bubble.
                    {
                        // Redefine the refresh zone and give it an event handler.
                        CLCircularRegion refreshRegion = new CLCircularRegion(new CLLocationCoordinate2D(latitude, longitude), bubble.Radius, "RefreshZone");
                        LocMgr.StartMonitoring(refreshRegion);
                    }
                    else // Poppable bubble.
                    {
                        CLCircularRegion bubbleRegion = new CLCircularRegion(new CLLocationCoordinate2D((double)bubble.Latitude, (double)bubble.Longitude), bubble.Radius, bubble.Id.ToString());
                        LocMgr.StartMonitoring(bubbleRegion);
                        Debug.WriteLine(">>>>> Adding bubble #" + bubble.Id.ToString());
                        // Check to see if we're already inside the bubble.  This will not raise a "RegionEntered" event.
                        if (bubbleRegion.Contains(new CLLocationCoordinate2D(latitude, longitude)))
                        {
                            Pop(null, new CLRegionEventArgs(bubbleRegion));
                            Debug.WriteLine(">>>>> Inside bubble #" + bubble.Id.ToString());
                        }
                    }
                }
            }
        }

        protected async Task<Stream> WebApiPost(string servicePath, Object content)
        {
            string securityToken = credentials.SecurityToken;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://192.168.1.107:83/"); // TBD - move to config
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authentication", credentials.SecurityToken);
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

        public void Start()
        {
            Debug.WriteLine(">>>>> Start");
            LocMgr.RegionLeft += Refresh;
            LocMgr.RegionEntered += Pop;
            // Kick off the first refresh.
            AwaitCredentials();
        }

        // Poll for the security token until it's available, then kick off the first refresh.
        async void AwaitCredentials()
        {
            while (credentials.SecurityToken.Length == 0) await Task.Delay(500);
            Refresh(null, null);
        }
        
        public LocationManager()
        {
            this.LocMgr = new CLLocationManager();
            this.LocMgr.PausesLocationUpdatesAutomatically = false;
            this.LocMgr.DesiredAccuracy = CLLocation.AccuracyNearestTenMeters;  // TBD - Move to config?

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

        void IDisposable.Dispose()  { if (!LocMgr.Equals(null)) LocMgr.Dispose(); }
    }
}