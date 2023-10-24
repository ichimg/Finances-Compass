namespace DebtsCompass.Domain.DtoResponses
{
    public class DebtDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public DateTime BorrowingDate { get; set; }
        public DateTime Deadline { get; set; }
        public string Reason { get; set; }
        public bool IsUserAccount { get; set; }
    }
}
