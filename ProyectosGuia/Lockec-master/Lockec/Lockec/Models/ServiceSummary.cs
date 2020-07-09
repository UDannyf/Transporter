namespace Lockec.Models
{

    using Lockec.ViewModels;
    using Newtonsoft.Json;

    public class ServiceSummary : BaseViewModel
    {

        [JsonProperty("service")]
        private string _serviceName;

        [JsonProperty("date")]
        private string _date;
        
        [JsonProperty("hour")]
        private string _hour;

        [JsonProperty("total_amount")]
        private string _totalAmount;

        [JsonProperty("payment_method")]
        private string _paymentMethod;

        [JsonProperty("driver_name")]
        private string _driverName;

        [JsonProperty("driver_lastname")]
        private string _driverLastName;

        [JsonProperty("driver_id")]
        private string _driverID;

        [JsonProperty("driver_position")]
        private string _driverWorkPosition;

        private string _userNotification;

        public string service_name
        {
            get { return this._serviceName; }
            set { SetValue(ref this._serviceName, value); }
        }

        public string service_date
        {
            get { return this._date; }
            set { SetValue(ref this._date, value); }
        }

        public string service_time
        {
            get { return this._hour; }
            set { SetValue(ref this._hour, value); }
        }

        public string service_cost
        {
            get { return this._totalAmount; }
            set { SetValue(ref this._totalAmount, value); }
        }

        public string employee_id_number
        {
            get { return this._driverID; }
            set { SetValue(ref this._driverID, value); }
        }
        public string service_payment
        {
            get { return this._paymentMethod; }
            set { SetValue(ref this._paymentMethod, value); }
        }
        public string employee_name
        {
            get { return this._driverName; }
            set { SetValue(ref this._driverName, value); }
        }
        public string employee_last_name
        {
            get { return this._driverLastName; }
            set { SetValue(ref this._driverLastName, value); }
        }
        public string employee_work_position
        {
            get { return this._driverWorkPosition; }
            set { SetValue(ref this._driverWorkPosition, value); }
        }
        public string user_notification
        {
            get { return this._userNotification; }
            set { SetValue(ref this._userNotification, value); }
        }
    }
}
