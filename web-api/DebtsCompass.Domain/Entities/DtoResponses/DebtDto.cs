using DebtsCompass.Domain.Enums;

namespace DebtsCompass.Domain.Entities.DtoResponses
{
    public class DebtDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public DateTime BorrowingDate { get; set; }
        public DateTime Deadline { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public bool IsPaid { get; set; }
        public bool IsUserAccount { get; set; }

    }
}
