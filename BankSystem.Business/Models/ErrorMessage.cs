namespace BankSystem.Business.Models
{
    public class ErrorMessage
    {
        public string? Message { get; set; }
        public int ErrorCode { get; set; }
        public string? TraceId { get; set; }
        public string? RequestId { get; set; }
    }
}
