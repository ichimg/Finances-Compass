using DebtsCompass.Domain.Entities.Models;

namespace DebtsCompass.Domain.Interfaces
{
    public interface INonUserRepository
    {
        Task<NonUser> GetNonUserByEmail(string email);
        Task Delete(NonUser nonUser);
    }
}