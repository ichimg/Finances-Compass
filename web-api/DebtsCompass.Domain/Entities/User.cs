using Microsoft.AspNetCore.Identity;

namespace DebtsCompass.Domain.Entities
{
    public class User : IdentityUser
    {
        public Guid UserInfoId { get; set; }
        public UserInfo UserInfo { get; set; }
        public CurrencyPreference CurrencyPreference { get; set; }
        public ICollection<DebtAssignment> CreatedDebts { get; set; }
        public ICollection<DebtAssignment> DebtsAssigned { get; set; }
    }
}
