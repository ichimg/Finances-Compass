using System.ComponentModel.DataAnnotations.Schema;

namespace DebtsCompass.Domain.Entities.Models
{
    public class Income
    {
        public Guid Id { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Guid? CategoryId { get; set; }
        public IncomeCategory Category { get; set; }
        public string Note { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? EurExchangeRate { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? UsdExchangeRate { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
