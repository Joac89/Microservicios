
using dispatcher.Models;

namespace dispatcher.Models
{
    public class Deal
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string InvoiceKey { get; set; }
        public bool State { get; set; }
        public string Url { get; set; }
        public string Template { get; set; }
        public string Type { get; set; }
        public string RequestTemplate { get; set; }
        public int NumRequest { get; set; }
    }

    public class ResponseDeal : ResponseBase
    {
        public Deal Data { get; set; }
    }
}