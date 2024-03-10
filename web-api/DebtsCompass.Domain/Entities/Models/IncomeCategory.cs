namespace DebtsCompass.Domain.Entities.Models
{
    public class IncomeCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? UserId { get; set; }
        public User User { get; set; }
        public ICollection<Income> Incomes { get; set; }
    }
}