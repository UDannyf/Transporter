using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Firebase.Messaging;
using System.Collections.Generic;

namespace Lockec.Droid.Services
{

    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        // private string TAG = "MyFirebaseMsgService";
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            SendNotification(message.GetNotification().Body, message.Data);
        }
        private void SendNotification(string messageBody, IDictionary<string, string> data)
        {
            /*
            var intent = new Intent(this, typeof(MainActivity)); intent.AddFlags(ActivityFlags.ClearTop);
            foreach (var key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }
            var pendingIntent = PendingIntent.GetActivity(this, MainActivity.NOTIFICATION_ID, intent, PendingIntentFlags.OneShot);
            var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.CHANNEL_ID).SetSmallIcon(Resource.Drawable.icon).SetContentTitle("Lockec Notification").SetContentText(messageBody).SetAutoCancel(true).SetContentIntent(pendingIntent);
            var notificationManager = NotificationManagerCompat.From(this); notificationManager.Notify(MainActivity.NOTIFICATION_ID, notificationBuilder.Build());
            */
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);
            //string CHANNEL_ID = "my_channel_01";

            var notificationBuilder = new Notification.Builder(this, MainActivity.CHANNEL_ID)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetContentTitle("Lockec Notification")
                .SetContentText(messageBody)
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true);

            var notificationManager = NotificationManager.FromContext(this);

            NotificationChannel mChannel = new NotificationChannel(MainActivity.CHANNEL_ID, "FCM Notification", NotificationImportance.High);
            // Configure the notification channel.
            mChannel.Description = "My Notifications";

            //mChannel.EnableLights(true);

           
            //mChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });
            mChannel.EnableVibration(true);
            mChannel.CanShowBadge();

            notificationManager.CreateNotificationChannel(mChannel);
            notificationManager.Notify(1, notificationBuilder.Build());
        }
    }

}
