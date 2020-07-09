using Android.App;
using Android.Util;
using Firebase.Iid;
using Newtonsoft.Json;
using Plugin.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lockec.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        public async override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            System.Diagnostics.Debug.WriteLine("AQUI ESTOY");
            System.Diagnostics.Debug.WriteLine(TAG, "Refreshed token: " + refreshedToken);
            await SendRegistrationToServer(refreshedToken);
            
        }

        public async Task<List<FCM>> GetDeviceIDFCM()
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync("https://lockec.pythonanywhere.com/api/devices/");
            return JsonConvert.DeserializeObject<List<FCM>>(response);
        }

        private async Task SendRegistrationToServer(string token)
        {
            try
            {
                var fcmList = await GetDeviceIDFCM();

                //string deviceID = Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
                string deviceID = CrossDeviceInfo.Current.Id;
                var fcmObject = new FCM(deviceID);
                var exists = fcmList.Contains(fcmObject);
                var dict = new Dictionary<string, string>();
                dict.Add("name", "");
                dict.Add("registration_id", token);
                dict.Add("device_id", deviceID);
                //dict.Add("acive", );
                dict.Add("type", "android");
                var client = new HttpClient();

                if (exists)
                {
                    var existingAccountIndex = fcmList.IndexOf(fcmObject);
                    var existingAccount = fcmList[existingAccountIndex];
                    var req = new HttpRequestMessage(HttpMethod.Put, "https://lockec.pythonanywhere.com/api/devices/" + existingAccount.registration_id + "/")
                    {
                        Content = new FormUrlEncodedContent(dict)
                    };
                    var response = await client.SendAsync(req).ConfigureAwait(false);
                }
                else
                {
                    var req = new HttpRequestMessage(HttpMethod.Post, "https://lockec.pythonanywhere.com/api/devices/")
                    {
                        Content = new FormUrlEncodedContent(dict)
                    };
                    var response = await client.SendAsync(req).ConfigureAwait(false);
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }


        }
    }

    public class FCM
    {
        public string name { get; set; }
        public string registration_id { get; set; }
        public string device_id { get; set; }
        public bool active { get; set; }
        public string type { get; set; }

        public FCM(string device_id)
        {
            this.device_id = device_id;
        }


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            FCM account = (FCM)obj;
            return this.device_id == account.device_id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}