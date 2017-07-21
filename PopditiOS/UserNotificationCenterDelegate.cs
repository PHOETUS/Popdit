using System;
using UserNotifications;
using System.Diagnostics;

namespace PopditiOS
{
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public UserNotificationCenterDelegate() { }

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            Debug.WriteLine(">>>>> Notification: " + notification.Request.Identifier.ToString());
            // Displaythe notification.
            completionHandler(UNNotificationPresentationOptions.Alert);
        }
    }

}