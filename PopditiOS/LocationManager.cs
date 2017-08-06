using System;
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
                localEvent.BubbleId = bubbleId;
                localEvent.TimestampJson = DateTime.Now.ToShortTimeString();
                string json = await WebApiPost("api/Event", localEvent);
                EventMobile serverEvent = (EventMobile)JsonConvert.DeserializeObject(json, typeof(EventMobile));

                // Display the notification.
                DisplayNotification(serverEvent.ProviderName, serverEvent.MsgTitle, serverEvent.Msg, "Popdit" + " " + e.Region.Identifier);

                // If the pops page is displayed, refresh it.
                UIWebView webView = (UIWebView)UIApplication.SharedApplication.KeyWindow.RootViewController.View;
                //if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active && webView.Request.Url.AbsoluteString.Contains("Event"))
                if (webView.Request.Url.AbsoluteString.Contains("Event"))
                    webView.LoadRequest(new NSUrlRequest(new NSUrl("http://192.168.1.107:82/")));
                    //webView.LoadRequest(new NSUrlRequest(new NSUrl("http://stage.popdit.com/")));
            }
        }

        void DisplayNotification(string title, string subtitle, string body, string requestId)
        {
            var content = new UNMutableNotificationContent();
            if (title != null) content.Title = title;
            if (subtitle != null) content.Subtitle = subtitle;
            if (body != null) content.Body = body;
            //content.Badge = 1; // Not implemented.
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
                double latitude = location.Coordinate.Latitude; // TBD double
                double longitude = location.Coordinate.Longitude; // TBD double
                Debug.WriteLine(">>>>> Refreshing at " + latitude.ToString() + ", " + longitude.ToString());
                Location loc = new Location(latitude, longitude);
                // Get all the bubbles in the zone.
                string json = await WebApiPost("api/Bubble", loc);
               
                BubbleCatalog = (List<BubbleMobile>)JsonConvert.DeserializeObject(json, typeof(List<BubbleMobile>));

                // Set an event handler for each bubble in the new list.
                foreach (BubbleMobile bubble in BubbleCatalog)
                {
                    if (bubble.Id == 0) // Refresh zone bubble.
                    {
                        Debug.WriteLine(">>>>> Refresh zone");
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
        }

        protected async Task<string> WebApiPost(string servicePath, Object content)
        {
            string securityToken = credentials.BasicAuthString;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://192.168.1.107:83/"); // TBD - move to config
                    //client.BaseAddress = new Uri("https://pop-stage.popdit.com/"); // TBD - move to config
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
            int count = 0;
            while (credentials.BasicAuthString.Length == 0)
            {
                await Task.Delay(500);
                count++;
                if (count == 1200)  // 10 minutes.
                    DisplayNotification("Please sign in", null, "Popdit cannot notify you about cool places until you sign in.", "Sign in");
            }
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