using System.ComponentModel.DataAnnotations;

namespace DebtsCompass.Domain.Entities.Requests
{
    public class CreateIncomeRequest
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string Category { get; set; }
        [MaxLength(500)]
        public string? Note { get; set; }
    }
}
