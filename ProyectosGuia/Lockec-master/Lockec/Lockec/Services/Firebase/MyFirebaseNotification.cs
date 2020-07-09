namespace Lockec.Services.Firebase
{
    using Lockec.Extras;
    using Lockec.Models;
    using Lockec.Services.Api_rest;
    using Lockec.ViewModels;
    using Lockec.Views.Menu;
    using Lockec.Views.PopUps;
    using Newtonsoft.Json;
    using Plugin.FirebasePushNotification;
    using Rg.Plugins.Popup.Services;
    using System;
    using Xamarin.Forms;

    public class MyFirebaseNotification
    {

        public static void MyNotificationOpened()
        {
            //CrossFirebasePushNotification.Current.Subscribe("general");
            
            CrossFirebasePushNotification.Current.OnNotificationOpened += async (s, p) =>
            {
                if (!string.IsNullOrEmpty(p.Identifier))
                {
                }
                else if (p.Data.ContainsKey("service_name"))
                {
                    var api = new ApiService();
                    ServiceSummary serviceSummary = ServiceDetails.ToServiceSummary<ServiceSummary>(p.Data);
                    var notvm = MainViewModel.GetInstance().NotVM = new NotificationSummaryViewModel
                    {
                        Date = serviceSummary.service_date,
                        DriverID = serviceSummary.employee_id_number,
                        DriverLastName = serviceSummary.employee_last_name,
                        DriverName = serviceSummary.employee_name,
                        PaymentMethod = serviceSummary.service_payment,
                        ServiceName = serviceSummary.service_name,
                        Hour = serviceSummary.service_time,
                        TotalAmount = serviceSummary.service_cost,
                        DriverWorkPosition = serviceSummary.employee_work_position
                    };
                    var accountSettings = Settings.AccountSettings;
                    var account = JsonConvert.DeserializeObject<Account>(accountSettings);
                    serviceSummary.user_notification = account.PhoneNumber;
                    await api.Post<ServiceSummary>(Constants.NOTIFICATION, serviceSummary);

                    MainViewModel.GetInstance().PayVM = new PaymentViewModel
                    {
                        ServiceSummaryInstance = serviceSummary
                    };


                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await Application.Current.MainPage.Navigation.PushModalAsync(new MenuPage());
                        if ("Efectivo".Equals(serviceSummary.service_payment)) 
                        {
                            await PopupNavigation.Instance.PushAsync(new EfectivePaymentPopUpView());
                        }
                        else
                        {
                            await PopupNavigation.Instance.PushAsync(new PaymentPopUpView());
                        }
                        
                    });

                }

            };

        }

        /*public static void MyNotificationReceived()
        {

            CrossFirebasePushNotification.Current.OnNotificationReceived += async (s, p) =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Received");
                    if (p.Data.ContainsKey("service_name"))
                    {
                        var api = new ApiService();
                        ServiceSummary serviceSummary = ServiceDetails.ToServiceSummary<ServiceSummary>(p.Data);
                        var notvm = MainViewModel.GetInstance().NotVM = new NotificationSummaryViewModel
                        {
                            Date = serviceSummary.service_date,
                            IDNumber = serviceSummary.employee_id_number,
                            Lastname = serviceSummary.employee_last_name,
                            Name = serviceSummary.employee_name,
                            PaymentMethod = serviceSummary.service_payment,
                            ReqService = serviceSummary.service_name,
                            StartTime = serviceSummary.service_time,
                            TotalCost = serviceSummary.service_cost,
                            WorkPosition = serviceSummary.employee_work_position
                        };
                        var accountSettings = Settings.AccountSettings;
                        var account = JsonConvert.DeserializeObject<Account>(accountSettings);
                        serviceSummary.user_notification = account.PhoneNumber;
                        await api.Post<ServiceSummary>(Constants.NOTIFICATION, serviceSummary);

                        *//*Device.BeginInvokeOnMainThread(() =>
                        {
                            //mPage.Message = $"{p.Data["body"]}";
                        });*//*

                    }
                }
                catch (Exception ex)
                {

                }
            };
        }*/

    }
}
