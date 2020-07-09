namespace Lockec.Models
{
    using Lockec.ViewModels;
    using Newtonsoft.Json;

    public class Account : BaseViewModel
    {
        [JsonProperty("phone_number")]
        private string _phoneNumber;

        [JsonProperty("email")]
        private string _email;

        [JsonProperty("city")]
        private string _city;

        [JsonProperty("address")]
        private string _address;

        [JsonProperty("verification_code")]
        private string _verificationCode;

        
        public string PhoneNumber
        {
            get { return this._phoneNumber; }
            set { SetValue(ref this._phoneNumber, value); }
        }

       
        public string Email
        {
            get { return this._email; }
            set { SetValue(ref this._email, value); }
        }

        
        public string City
        {
            get { return this._city; }
            set { SetValue(ref this._city, value); }
        }

        
        public string Address
        {
            get { return this._address; }
            set { SetValue(ref this._address, value); }
        }

        
        public string VerificationCode
        {
            get { return this._verificationCode; }
            set { SetValue(ref this._verificationCode, value); }
        }

    }
}
