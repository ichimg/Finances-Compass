using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DebtsCompass.Domain.Entities.Models
{
    public class Debt
    {
        public Guid Id { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        public string? BorrowReason { get; set; }
        public DateTime DateOfBorrowing { get; set; }
        public DateTime DeadlineDate { get; set; }
        public Status Status { get; set; }
        public bool IsPaid { get; set; }
        public int? CurrencyRateId { get; set; }
        public CurrencyRate CurrencyRate { get; set; }
        public ICollection<DebtAssignment> DebtAssignments { get; set; }
    }
}
