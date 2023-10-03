using Microsoft.AspNetCore.Identity;

namespace DebtsCompass.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public ICollection<Debt> Debts { get; } = new List<Debt>();
    }
}
