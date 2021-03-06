﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using UIKit;
using CoreLocation;
using UserNotifications;
using PopditMobileApi;
using System.Diagnostics;
using Foundation;

namespace PopditiOS
{
    public class LocationManager : IDisposable
    {
        protected static CLLocationManager LocMgr;
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
                localEvent.BubbleId = bubbleId;
                localEvent.TimestampJson = DateTime.Now.ToLongTimeString();
                string json = await WebApiPost("api/Event", localEvent); 

                if (json != null && json.Length > 0)
                {
                    EventMobile serverEvent = (EventMobile)JsonConvert.DeserializeObject(json, typeof(EventMobile));
                    // If the event has not been suppressed and the server hit didn't fail, process it.
                    if (!serverEvent.Suppressed)
                    {
                        Debug.WriteLine(">>>>> Processing event: Bubble #" + bubbleId.ToString());
                        // Display a notification.
                        DisplayNotification(serverEvent.ProviderName, serverEvent.MsgTitle, serverEvent.Msg, "Popdit" + " " + e.Region.Identifier);
                        // If the pops page is displayed, refresh it.
                        UIWebView webView = (UIWebView)UIApplication.SharedApplication.KeyWindow.RootViewController.View;
                        if (webView.Request.Url.AbsoluteString.Contains("Event"))
                            webView.LoadRequest(new NSUrlRequest(PopditServer.WebRoot));
                    }
                    else Debug.WriteLine(">>>>> Event suppressed: Bubble #" + bubbleId.ToString());
                }
            }
        }

        public void DisplayNotification(string title, string subtitle, string body, string requestId)
        {
            var content = new UNMutableNotificationContent();
          
            // Compose the notification
            if (title != null && title.Length > 0) content.Title = title;
            else content.Title = "-";
            if (subtitle != null && subtitle.Length > 0) content.Subtitle = subtitle;
            if (body != null && body.Length > 0) content.Body = body;
            else content.Body = "See the Pops page for details";
            content.Sound = UNNotificationSound.GetSound("bubblepop.wav");

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false); // 1 second delay, do not repeat.

            var request = UNNotificationRequest.FromIdentifier(requestId, content, trigger);

            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            {
                if (err != null) { Debug.WriteLine(">>>>> Message error: " + err.DebugDescription); }
            });
        }

        // The user left the refresh zone.  Get a new catalog of bubbles from the server.
        async void Refresh(object sender, CLRegionEventArgs e)
        {
            if (credentials.BasicAuthString.Length > 0 && (e == null || e.Region.Identifier.Equals("RefreshZone"))) // Ignore location bubbles.
            {
                // Get the user's location and code it as the bubble zone.
                CLLocation location = LocMgr.Location;
                if (location != null)
                {
                    double latitude = location.Coordinate.Latitude;
                    double longitude = location.Coordinate.Longitude;
                    Debug.WriteLine(">>>>> Refreshing at " + latitude.ToString() + ", " + longitude.ToString());
                    Location loc = new Location(latitude, longitude);

                    // At the last possible moment, delete all the existing bubbles.
                    foreach (CLCircularRegion r in LocMgr.MonitoredRegions) LocMgr.StopMonitoring(r);

                    // Get all the bubbles in the zone.
                    string json = await WebApiPost("api/Bubble", loc);

                    // If you got a connection, process the data ...
                    if (json != null && json.Length > 0)
                    {
                        List<BubbleMobile> bubbleCatalog = (List<BubbleMobile>)JsonConvert.DeserializeObject(json, typeof(List<BubbleMobile>));

                        // Set an event handler for each bubble in the new list.
                        foreach (BubbleMobile bubble in bubbleCatalog)
                        {
                            if (bubble.Id == 0) // Refresh zone bubble.
                            {
                                Debug.WriteLine(">>>>> Adding refresh zone");
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
                                    Debug.WriteLine(">>>>> Inside bubble #" + bubble.Id.ToString() + " before pop");
                                    Pop(null, new CLRegionEventArgs(bubbleRegion));
                                    Debug.WriteLine(">>>>> Inside bubble #" + bubble.Id.ToString() + " after pop");
                                }
                            }
                        }
                    }
                    else HandleRefreshError();
                }                
                else HandleRefreshError();
            }
        }

        async void HandleRefreshError()
        {
            Debug.WriteLine(">>>>> Waiting for a connection");
            await Task.Delay(60000); // 1 minute
            // Refresh the location.
            Refresh(null, null);
        }

        protected async Task<string> WebApiPost(string servicePath, Object content)
        {
            string securityToken = credentials.BasicAuthString;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(PopditServer.PopRoot.AbsoluteString);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", credentials.BasicAuthString);
                    string json = JsonConvert.SerializeObject(content);
                    StringContent sc = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(servicePath, sc).ConfigureAwait(false);
                    return response.Content.ReadAsStringAsync().Result;
                }
                catch (Exception e)
                {
                    // TBD - Handle error
                    Debug.WriteLine(">>>>> Error in WebApiPost: " + e.Message);
                    return null;
                }
            }
        }

        // Poll for the security token until it's available, then kick off the first refresh.
        async void AwaitCredentials()
        {
            int count = 0;
            while (credentials.BasicAuthString.Length == 0)
            {
                Debug.WriteLine(">>>>> Awaiting credentials");
                await Task.Delay(500);
                count++;
                if (count == 1200)  // 10 minutes.
                    DisplayNotification("Please sign in", null, "Popdit cannot notify you about cool places until you sign in.", "Sign in");
            }
            Refresh(null, null);
        }

        public void Start() // All the async errors end up here.
        {
            Debug.WriteLine(">>>>> Start");
            LocMgr.RegionLeft += Refresh; // async void
            LocMgr.RegionEntered += Pop; // async void
            // Kick off the first refresh.
            AwaitCredentials(); // async void
        }

        public LocationManager()
        {
            LocMgr = new CLLocationManager();
            LocMgr.PausesLocationUpdatesAutomatically = false;
            LocMgr.DesiredAccuracy = CLLocation.AccuracyNearestTenMeters;  // TBD - Move to config?

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

        void IDisposable.Dispose() { if (!LocMgr.Equals(null)) LocMgr.Dispose(); }
    }
}