using System.ComponentModel.DataAnnotations;

namespace DebtsCompass.Domain.Entities.Requests
{
    public class EditIncomeRequest
    {
        [MaxLength(70)]
        [Required]
        public string Guid { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [MaxLength(30)]
        [Required]
        public string Category { get; set; }
        [MaxLength(500)]
        [Required]
        public string Note { get; set; }
    }
}
