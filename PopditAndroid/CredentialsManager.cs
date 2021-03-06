﻿using Android.Webkit;
//using Java.Net;
using System;
using System.Text;
using Android.Net;


namespace PopditAndroid
{
    class CredentialsManager
    {
        string AuthString = "";

        // Get the security token for this mobile device.  Try this object, then the cookie, then the server.
        // If this function fails, the user needs to sign in.
        // TBD - Add error handling to force sign in.
        public string BasicAuthString
        {
            get
            {
                // If auth string is not initialized in this object ...
                if (AuthString.Length == 0)
                {
                    // Try to get the security token from the cookies anbd store it for next time.
                    string uid = GetValueForPopditCookieKey("Phone");
                    string pwd = GetValueForPopditCookieKey("Password");
                    if (uid.Length != 0 && pwd.Length != 0)
                        AuthString = "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(uid + ":" + pwd));                     
                }
                return AuthString;
            }
        }

        string GetPopditCookie()
        {
            // Get the cookies.
            CookieManager cookieMgr = CookieManager.Instance;
            var cookie = cookieMgr.GetCookie("http://prod.popdit.com")[0];
            return cookie.ToString();
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
    }
}