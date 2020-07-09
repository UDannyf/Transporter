namespace Lockec.Models
{
    using Lockec.ViewModels;
    using Newtonsoft.Json;

    public class PersonalAccount : BaseViewModel
    {
        [JsonProperty("dob")]
        private string _dob;

        [JsonProperty("id_number")]
        private string _id_number;

        [JsonProperty("name")]
        private string _name;

        [JsonProperty("last_name")]
        private string _last_name;

        [JsonProperty("account")]
        private string _account;

        public string DoB
        {
            get { return this._dob; }
            set { SetValue(ref this._dob, value); }
        }

        public string IdNumber
        {
            get { return this._id_number; }
            set { SetValue(ref this._id_number, value); }
        }

        public string Name
        {
            get { return this._name; }
            set { SetValue(ref this._name, value); }
        }

        public string Lastname
        {
            get { return this._last_name; }
            set { SetValue(ref this._last_name, value); }
        }

        public string Account
        {
            get { return this._account; }
            set { SetValue(ref this._account, value); }
        }

    }
}
