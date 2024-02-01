namespace DebtsCompass.Domain.Entities.Requests
{
    public class CompletePaypalOrderRequest
    {
        public string Intent { get; set; }
        public string OrderId { get; set; }
    }
}