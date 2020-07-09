namespace Lockec.ViewModels
{
    using Lockec.Extras;
    using Lockec.Models;
    using Lockec.Services.Api_rest;
    using Lockec.Views.PopUps;
    using Rg.Plugins.Popup.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class NotificationSummaryViewModel : BaseViewModel
    {
        private bool _isRunning;
        private string _service_name;
        private string _service_date;
        private string _service_time;
        private string _service_cost;
        private string _service_payment;
        private string _employee_name;
        private string _employee_last_name;
        private string _employee_id_number;
        private string _employee_work_position;

        private ApiService api;
        private Account AccountInstance;
        private ObservableCollection<ServiceSummary> _notifications;
        private ServiceSummary _notificationSelected;

        public string ServiceName
        {
            get { return this._service_name; }
            set { SetValue(ref this._service_name, value); }
        }

        public string Date
        {
            get { return this._service_date; }
            set { SetValue(ref this._service_date, value); }
        }

        public string Hour
        {
            get { return this._service_time; }
            set { SetValue(ref this._service_time, value); }
        }

        public string TotalAmount
        {
            get { return this._service_cost; }
            set { SetValue(ref this._service_cost, value); }
        }

        public string PaymentMethod
        {
            get { return this._service_payment; }
            set { SetValue(ref this._service_payment, value); }
        }

        public string DriverName
        {
            get { return this._employee_name; }
            set { SetValue(ref this._employee_name, value); }
        }

        public string DriverLastName
        {
            get { return this._employee_last_name; }
            set { SetValue(ref this._employee_last_name, value); }
        }

        public string DriverID
        {
            get { return this._employee_id_number; }
            set { SetValue(ref this._employee_id_number, value); }
        }

        public string DriverWorkPosition
        {
            get { return this._employee_work_position; }
            set { SetValue(ref this._employee_work_position, value); }
        }

        public bool IsRunning
        {
            get { return this._isRunning; }
            set { SetValue(ref this._isRunning, value); }
        }

        public ObservableCollection<ServiceSummary> Notifications
        {
            get { return this._notifications; }
            set { SetValue(ref this._notifications, value); }
        }
        public ServiceSummary NotificationSelected
        {
            get { return _notificationSelected; }
            set
            {
                SetValue(ref this._notificationSelected, value);
                if (_notifications != null)
                {
                    GetNotificationDetailCommand.Execute(_notificationSelected);
                }
            }
        }

        public ICommand GetNotificationDetailCommand { get; set; }
        public ICommand InitNotificationDetailsCommand { get; set; }


        public NotificationSummaryViewModel()
        {
            GetNotificationDetailCommand = new Command<ServiceSummary>(async (param) => await ShowNotificationDetailsSummary(param));
            api = new ApiService();
            IsRunning = true;
            AccountInstance = MainViewModel.GetInstance().MenuVM.AccountInstance;
            InitNotificationDetailsCommand = new Command(InitNotificationDetails);

        }

        private async void InitNotificationDetails()
        {

            await Task.Run(async () =>
            {
                
                var notificationResponse = await api.GetAllInfo<ServiceSummary>(Constants.NOTIFICATION, "?user_notification=" + AccountInstance.PhoneNumber + "&ordering=-date");
                if (notificationResponse.IsSuccess != false)
                {
                    var notifications = (List<ServiceSummary>)notificationResponse.Result;
                    Notifications = new ObservableCollection<ServiceSummary>();
                    //Services.OrderBy(s => s.Status)
                    foreach (var n in notifications)
                    {
                        Notifications.Add(n);
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        // UI interaction goes here
                        IsRunning = false;
                    });
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                  "¡Ups!",
                  "No posees ningún servicio asociado a tu cuenta.",
                  "Ok");
                    return;
                }

            });

        }

        /*var notvm = MainViewModel.GetInstance().NotVM = new NotificationSummaryViewModel
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
        };*/

        private async Task ShowNotificationDetailsSummary(ServiceSummary service)
        {
            //NotificationsLoad(service);
            MainViewModel.GetInstance().PayVM = new PaymentViewModel
            {
                ServiceSummaryInstance = service
            };
            
            if ("Efectivo".Equals(service.service_payment))
            {
                await PopupNavigation.Instance.PushAsync(new EfectivePaymentPopUpView());

            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new PaymentMenuOptionPopUpView());
                
                //MainViewModel.GetInstance().PayVM.MakeRequestCommand.Execute(null);

            }

            //await PopupNavigation.Instance.PushAsync(new ServiceHistoryPopUpView());
            /*await Application.Current.MainPage.DisplayAlert(
                    service.ReqService, service.Date,
                    "Ok");
            return;*/
        }


        private void NotificationsLoad(ServiceSummary service)
        {
            
            this.DriverID = service.employee_id_number;
            this.DriverLastName = service.employee_name;
            this.DriverName = service.employee_name;
            this.DriverWorkPosition = service.employee_work_position;

            this.Date = service.service_date;
            this.TotalAmount = service.service_cost;
            this.ServiceName = service.service_name;
            this.Hour = service.service_time;
            this.PaymentMethod = service.service_payment;
        }
    }
}
