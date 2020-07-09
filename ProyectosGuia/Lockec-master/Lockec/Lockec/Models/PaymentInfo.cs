namespace Lockec.Models
{
    public class Buyer
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string document { get; set; }
        public string documentType { get; set; }
        public string mobile { get; set; }
    }

    public class Amount
    {
        public string currency { get; set; }
        public string total { get; set; }
    }

    public class Payment
    {
        public string reference { get; set; }
        public string description { get; set; }
        public Amount amount { get; set; }
    }

    public class Auth
    {
        public string login { get; set; }
        public string tranKey { get; set; }
        public string nonce { get; set; }
        public string seed { get; set; }
    }


    public class PaymentInfo
    {
        public Buyer buyer { get; set; }
        public Payment payment { get; set; }
        public string expiration { get; set; }
        public string ipAddress { get; set; }
        public string returnUrl { get; set; }
        public string userAgent { get; set; }
        public string paymentMethod { get; set; }
        public Auth auth { get; set; }
    }

    
}
