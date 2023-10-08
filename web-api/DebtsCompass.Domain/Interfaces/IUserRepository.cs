using DebtsCompass.Domain.Entities;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
        Task Add(User user);    
    }
}