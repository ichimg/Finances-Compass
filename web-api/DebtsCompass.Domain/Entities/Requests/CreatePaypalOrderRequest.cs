namespace DebtsCompass.Domain.Entities.Requests
{
    public class CreatePaypalOrderRequest
    {
        public string Intent { get; set; }
        public string PayeeEmail { get; set; }
        public string Value { get; set; }
        public string EurExchangeRate { get; set; }
    }
}
