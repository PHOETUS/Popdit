using System.Collections.Generic;
using PopditiOS.Models;
using CoreLocation;
using UIKit;
using Foundation;

namespace PopditiOS
{
    public class LocationManager
    {
        protected CLLocationManager LocMgr;
        string SecToken = "";
        Zone Zone = new Zone();
        List<BubbleMobile> BubbleCatalog = new List<BubbleMobile>();

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
        { }

        // The user moved.  Get a new catalog of bubbles from the server.
        void RefreshBubbleCatalog()
        {
            // Make sure the user's credentials are available.
            string token = SecurityToken;
            // Get the user's location.
            // Define a zone and set and event handler for it.
            // Get all the bubbles in the zone and set event handlers for them.
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