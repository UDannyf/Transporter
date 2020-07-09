namespace Lockec.Models
{
    using Lockec.ViewModels;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ServiceDetails : BaseViewModel
    {
        [JsonProperty("id")]
        private string _id;

        [JsonProperty("start_place")]
        private string _startPlace;

        [JsonProperty("end_place")]
        private string _endPlace;

        [JsonProperty("start_time")]
        private string _startTime;

        [JsonProperty("end_time")]
        private string _endTime;

        [JsonProperty("date")]
        private string _date;

        [JsonProperty("status")]
        private string _status;

        [JsonProperty("payment_method")]
        private string _paymentMethod;

        [JsonProperty("total_distance")]
        private double _totalDistance;

        [JsonProperty("total_cost")]
        private double _totalCost;

        [JsonProperty("req_no_unit")]
        private int _unitNumber;

        [JsonProperty("req_vehicle")]
        private bool _reqVehicle;

        [JsonProperty("req_guard")]
        private bool _reqGuard;

        [JsonProperty("req_lock")]
        private bool _reqLock;

        [JsonProperty("device_id")]
        private string _deviceID;

        [JsonProperty("req_service")]
        private string _reqService;

        [JsonProperty("user_id")]
        private string _userID;

       
        public string Id
        {
            get { return this._id; }
            set { SetValue(ref this._id, value); }
        }
        public string StartPlace
        {
            get { return this._startPlace; }
            set { SetValue(ref this._startPlace, value); }
        }

        public string EndPlace
        {
            get { return this._endPlace; }
            set { SetValue(ref this._endPlace, value); }
        }

        public string StartTime
        {
            get { return this._startTime; }
            set { SetValue(ref this._startTime, value); }
        }

        public string EndTime
        {
            get { return this._endTime; }
            set { SetValue(ref this._endTime, value); }
        }

        public string Date
        {
            get { return this._date; }
            set { SetValue(ref this._date, value); }
        }

        public int UnitNumber
        {
            get { return this._unitNumber; }
            set { SetValue(ref this._unitNumber, value); }
        }

        public string PaymentMethod
        {
            get { return this._paymentMethod; }
            set { SetValue(ref this._paymentMethod, value); }
        }

        public double TotalDistance
        {
            get { return this._totalDistance; }
            set { SetValue(ref this._totalDistance, value); }
        }

        public double TotalCost
        {
            get { return this._totalCost; }
            set { SetValue(ref this._totalCost, value); }
        }

        public string ReqService
        {
            get { return this._reqService; }
            set { SetValue(ref this._reqService, value); }
        }

        public string UserID
        {
            get { return this._userID; }
            set { SetValue(ref this._userID, value); }
        }

        public string DeviceID
        {
            get { return this._deviceID; }
            set { SetValue(ref this._deviceID, value); }
        }

        public string Status
        {
            get { return this._status; }
            set { SetValue(ref this._status, value); }
        }

        public bool ReqVehicle
        {
            get { return this._reqVehicle; }
            set { SetValue(ref this._reqVehicle, value); }
        }

        public bool ReqGuard
        {
            get { return this._reqGuard; }
            set { SetValue(ref this._reqGuard, value); }
        }

        public bool ReqLock
        {
            get { return this._reqLock; }
            set { SetValue(ref this._reqLock, value); }
        }


        public static T ToServiceSummary<T>(IDictionary<string, object> source)
        where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                if (item.Key == "service_name" ||
                   item.Key == "service_date" ||
                   item.Key == "service_time" ||
                   item.Key == "service_cost" ||
                   item.Key == "service_payment" ||
                   item.Key == "employee_name" ||
                   item.Key == "employee_last_name" ||
                   item.Key == "employee_id_number" ||
                   item.Key == "employee_work_position")
                    someObjectType
                             .GetProperty(item.Key)
                             .SetValue(someObject, item.Value, null);
            }

            return someObject;
        }

    }
}
