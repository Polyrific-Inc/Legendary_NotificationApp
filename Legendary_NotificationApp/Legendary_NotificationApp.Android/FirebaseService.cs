using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;
using System;
using System.Linq;
using WindowsAzure.Messaging;

namespace Legendary_NotificationApp.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            string messageBody = string.Empty;
            string url = string.Empty;
            string title = string.Empty;

            if (message.GetNotification() != null)
            {
                messageBody = message.GetNotification().Body;
            }

            // NOTE: test messages sent via the Azure portal will be received here
            else
            {
                messageBody = message.Data["message"];
                url = message.Data["url"];
                title = message.Data["title"];
            }

            // convert the incoming message to a local notification
            SendLocalNotification(messageBody, url, title);

        }

        public override void OnNewToken(string token)
        {
            // TODO: save token instance locally, or log if desired

            SendRegistrationToServer(token);
        }

        void SendLocalNotification(string body, string url, string title)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);

            intent.PutExtra("url", url);
            intent.PutExtra("message", body);
            intent.PutExtra("title", body);

            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this, AzureNotificationHub.NotificationChannelName)
                .SetContentTitle(title)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetContentText(body)
                .SetAutoCancel(true)
                .SetShowWhen(false)
                .SetContentIntent(pendingIntent);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                notificationBuilder.SetChannelId(AzureNotificationHub.NotificationChannelName);
            }

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());
        }


        void SendRegistrationToServer(string token)
        {
            try
            {
                NotificationHub hub = new NotificationHub(AzureNotificationHub.NotificationHubName, AzureNotificationHub.ListenConnectionString, this);

                // register device with Azure Notification Hub using the token from FCM
                Registration registration = hub.Register(token, AzureNotificationHub.SubscriptionTags);

            }
            catch (Exception e)
            {
                Log.Error(AzureNotificationHub.DebugTag, $"Error registering device: {e.Message}");
            }
        }
    }

}