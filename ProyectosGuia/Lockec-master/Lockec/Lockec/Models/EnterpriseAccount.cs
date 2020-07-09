namespace Lockec.Models
{
    using Lockec.ViewModels;
    using Newtonsoft.Json;
    public class EnterpriseAccount : BaseViewModel
    {
        [JsonProperty("ruc")]
        private string _ruc;
        [JsonProperty("enterprise_name")]
        private string _enterprise_name;
        [JsonProperty("legal_representative_name")]
        private string _legal_representative_name;
        [JsonProperty("legal_representative_lastname")]
        private string _legal_representative_lastname;
        [JsonProperty("contact_name")]
        private string _contact_name;
        [JsonProperty("contact_lastname")]
        private string _contact_lastname;
        [JsonProperty("contact_work_position")]
        private string _contact_work_position;
        [JsonProperty("account")]
        private string _account;

        public string Ruc
        {
            get { return this._ruc; }
            set { SetValue(ref this._ruc, value); }
        }

        public string EnterpriseName
        {
            get { return this._enterprise_name; }
            set { SetValue(ref this._enterprise_name, value); }
        }

        public string LegalRepresentativeName
        {
            get { return this._legal_representative_name; }
            set { SetValue(ref this._legal_representative_name, value); }
        }

        public string LegalRepresentativeLastname
        {
            get { return this._legal_representative_lastname; }
            set { SetValue(ref this._legal_representative_lastname, value); }
        }

        public string ContactName
        {
            get { return this._contact_name; }
            set { SetValue(ref this._contact_name, value); }
        }

        public string ContactLastname
        {
            get { return this._contact_lastname; }
            set { SetValue(ref this._contact_lastname, value); }
        }

        public string ContactWorkPosition
        {
            get { return this._contact_work_position; }
            set { SetValue(ref this._contact_work_position, value); }
        }

        public string Account
        {
            get { return this._account; }
            set { SetValue(ref this._account, value); }
        }

    }
}
