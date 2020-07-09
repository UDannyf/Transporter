namespace Lockec.Models
{
    public class Status
    {
        public string status { get; set; }
        public string reason { get; set; }
        public string message { get; set; }
        public string date { get; set; }
    }

    public class PaymentStatus
    {
        public Status status { get; set; }
        public int requestId { get; set; }
        public string processUrl { get; set; }
    }
}
