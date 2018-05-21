namespace dispatcher.Models
{
    public class ResponseBase
    {
        public int Code { get; set; }
        public string Message { get; set; } = "";
    }
}