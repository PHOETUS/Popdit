﻿using System;
using System.Diagnostics;
using UIKit;
using Foundation;
using UserNotifications;

namespace PopditiOS
{
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public UserNotificationCenterDelegate() { }

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            Debug.WriteLine(">>>>> Notification: " + notification.Request.Identifier.ToString());
            // Displaythe notification.
            completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
        }

        // When a notification is clicked, load 
        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            UIWebView webView = (UIWebView)UIApplication.SharedApplication.KeyWindow.RootViewController.View;
            webView.LoadRequest(new NSUrlRequest(PopditServer.WebRoot));
        }
    }

}