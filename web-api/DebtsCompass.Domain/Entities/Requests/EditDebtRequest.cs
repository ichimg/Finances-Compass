using System.ComponentModel.DataAnnotations;

namespace DebtsCompass.Domain.Entities.Requests
{
    public class EditDebtRequest
    {
        [MaxLength(70)]
        public string Guid { get; set; }
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        [MaxLength(320)]
        [Required]
        public string Email { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string BorrowingDate { get; set; }
        [Required]
        public string Deadline { get; set; }
        [MaxLength(50)]
        public string? Reason { get; set; }
        [Required]
        public bool IsUserAccount { get; set; }
    }
}
