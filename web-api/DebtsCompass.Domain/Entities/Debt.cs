
namespace DebtsCompass.Domain.Entities
{
    public class Debt
    {
        public int Id { get; set; }
        public string CreatorUserId { get; set; }
        public User CreatorUser { get; set; } = null!;
        public double Amount { get; set; }
        public string? PersonName { get; set; }
        public string? PersonEmail { get; set; }
        public string? SelectedUserId { get; set; }
        public User? SelectedUser { get; set; }
        public DateTime DateOfBorrowing { get; set; }
        public string BorrowReason { get; set; }
        public BorrowingType BorrowingType { get; set; }
        public DateTime DeadlineDate { get; set; }
        public bool IsPaid { get; set; }
    }
}
