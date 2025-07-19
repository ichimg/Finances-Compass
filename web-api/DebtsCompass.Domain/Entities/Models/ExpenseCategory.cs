namespace DebtsCompass.Domain.Entities.Models
{
    public class ExpenseCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? UserId { get; set; }
        public User User { get; set; }
        public ICollection<Expense> Expenses { get; set; }
    }
}
