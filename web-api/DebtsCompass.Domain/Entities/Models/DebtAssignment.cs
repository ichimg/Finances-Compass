namespace DebtsCompass.Domain.Entities.Models
{
    public class DebtAssignment
    {
        public Guid Id { get; set; }
        public string CreatorUserId { get; set; }
        public User CreatorUser { get; set; }
        public string? SelectedUserId { get; set; } // key for user with an account
        public User SelectedUser { get; set; }
        public Guid? NonUserDebtAssignmentId { get; set; } // key for user without an account
        public NonUser NonUser { get; set; }
        public Guid DebtId { get; set; }
        public Debt Debt { get; set; }
    }
}
