using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace PopditiOS
{
    class CredentialsManager
    {
        string SecToken = "";

        // Get the security token for this mobile device.  Try this object, then the cookie, then the server.
        // If this function fails, the user needs to sign in.
        // TBD - Add error handling to force sign in.
        public string SecurityToken
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
                        if (phone.Length > 0 && pwd.Length > 0)
                        {
                            SecToken = GetSecurityTokenFromServer(phone, pwd);
                            // Initialize security token in the cookie.
                            SetValueForPopditCookie("Phone", SecToken); // TBD s/b "SecurityToken".
                        }
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
            NSHttpCookie cookie = GetPopditCookie();
            if (cookie != null) cookie.SetValueForKey(NSObject.FromObject(value), (NSString)key);
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
            token = "8126502080";
            return token;
        }
    }
}