namespace Lockec.Models
{
    using Lockec.ViewModels;
    using Newtonsoft.Json;
    public class ServiceProvided : BaseViewModel
    {

        public string id { get; set; }
        public object service_score { get; set; }
        public object employee_score { get; set; }
        public string comments { get; set; }
        public string anomalies { get; set; }
        public string client_name { get; set; }
        public string client_last_name { get; set; }
        public bool req_vehicle_service { get; set; }
        public bool req_guard_service { get; set; }
        public bool req_lock_service { get; set; }
        public object status { get; set; }
        public object date { get; set; }
        public object total_cost { get; set; }
        public bool is_payed { get; set; }
        public object client { get; set; }
        public object service { get; set; }
        public object service_details { get; set; }
        public object employee { get; set; }
        public object Armament { get; set; }
        public object Satellite_lock { get; set; }
        public object vehicle { get; set; }

        /*[JsonProperty("id")]
        private string _id;

        [JsonProperty("service_score")]
        private int _serviceScore;

        [JsonProperty("employee_score")]
        private int _employeeScore;

        [JsonProperty("comments")]
        private string _comments;

        [JsonProperty("anomalies")]
        private string _anomalies;

        [JsonProperty("status")]
        private string _status;

        [JsonProperty("service")]
        private string _service;

        [JsonProperty("client")]
        private string _client;

        [JsonProperty("service_details")]
        private string _service_details;

        [JsonProperty("total_cost")]
        private double _total_cost;

        [JsonProperty("client_name")]
        private string _client_name;

        *//*
        [JsonProperty("client")]
        private string _client;

        [JsonProperty("employee")]
        private string _employee;

        [JsonProperty("service")]
        private string _service;

        [JsonProperty("service_details")]
        private string _serviceDetails;
        *//*

        public string Id
        {
            get { return this._id; }
            set { SetValue(ref this._id, value); }
        }

        public int ServiceScore
        {
            get { return this._serviceScore; }
            set { SetValue(ref this._serviceScore, value); }
        }

        public int EmloyeeScore
        {
            get { return this._employeeScore; }
            set { SetValue(ref this._employeeScore, value); }
        }

        public double TotalCost
        {
            get { return this._total_cost; }
            set { SetValue(ref this._total_cost, value); }
        }


        public string Comments
        {
            get { return this._comments; }
            set { SetValue(ref this._comments, value); }
        }

        public string Anomalies
        {
            get { return this._anomalies; }
            set { SetValue(ref this._anomalies, value); }
        }

        public string Status
        {
            get { return this._status; }
            set { SetValue(ref this._status, value); }
        }

        public string Service
        {
            get { return this._service; }
            set { SetValue(ref this._service, value); }
        }

        public string Client
        {
            get { return this._client; }
            set { SetValue(ref this._client, value); }
        }

        public string ServiceDetails
        {
            get { return this._service_details; }
            set { SetValue(ref this._service_details, value); }
        }

        public string ClientName
        {
            get { return this._client_name; }
            set { SetValue(ref this._client_name, value); }
        }*/

    }
}
