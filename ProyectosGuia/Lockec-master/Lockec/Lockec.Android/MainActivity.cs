using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Plugin.FirebasePushNotification;
using Plugin.Permissions;
using Xamarin.Forms;

namespace Lockec.Droid
{
    [Activity(Label = "Lockec", Icon = "@drawable/ic_launcher", 
        Theme = "@style/splashscreen", MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //static readonly string TAG = "MainActivity";
        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(savedInstanceState);

            //START FOR FCM
            IsPlayServicesAvailable(); //You can use this method to check if play services are available.
            CreateNotificationChannel();
            //END FOR FCM

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);

            //Start Plugin Initialization
            
            //GOOGLE MAPS
            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState);

            //PopUps
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            //PERMISSIONS PLUGIN
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

            //End Plugin Initialization

            //FOR DEVICE ID
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            //END FOR DEVICE ID

            //FOR QR/BAR SCANNER
            global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
            //END FOR QR/BAR SCANNER

            LoadApplication(new App());

            //FOR FCM PUSH NOTIFICATIONS
            FirebasePushNotificationManager.ProcessIntent(this, Intent);
            //END FCM PUSH NOTIFICATIONS

            //VERIFICO LA INFO QUE RECIBO DE LA PUSH NOTIFICATION DE FCM
            /*Bundle bundle = Intent.Extras;
            System.Diagnostics.Debug.WriteLine(bundle);
            if (bundle != null)
            {
                Xamarin.Forms.Application.Current.MainPage = new MenuPage();
                Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync(new PaymentPopUpView());
            }*/


        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            //FOR XAMARIN ESSENTIALS
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            //FOR PERMISSIONS PLUGIN
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            //FOR QR/BAR SCANNER
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    //msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                }              
                else
                {
                    //This device is not supported           
                    Finish(); // Kill the activity if you want.         
                }
                return false;
            }
            else
            {
                //Google Play Services is available.         
                return true;
            }
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification 
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID, "FCM Notifications", NotificationImportance.Default)
            {
                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

    }
}