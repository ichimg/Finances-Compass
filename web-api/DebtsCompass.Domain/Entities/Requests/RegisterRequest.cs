using System.ComponentModel.DataAnnotations;

namespace DebtsCompass.Domain.Entities.Requests
{
    public class RegisterRequest
    {
        [MaxLength(30)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(30)]
        [Required]
        public string LastName { get; set; }
        [MaxLength(100)]
        [Required]
        public string Country { get; set; }
        [MaxLength(100)]
        [Required]
        public string State { get; set; }
        [MaxLength(100)]
        [Required]
        public string City { get; set; }
        [MaxLength(20)]
        [Required]
        public string PostalCode { get; set; }
        [MaxLength(100)]
        [Required]
        public string StreetAddress { get; set; }
        [MaxLength(320)]
        [Required]
        public string Email { get; set; }
        [MaxLength(20)]
        [Required]
        public string PhoneNumber { get; set; }
        [MaxLength(100)]
        [Required]
        public string Password { get; set; }
        [MaxLength(100)]
        [Required]
        public string ConfirmPassword { get; set; }
        [MaxLength(100)]
        [Required]
        public string Iban { get; set; }
        public string ClientURI { get; set; }
    }
}
