using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Pagination;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByUsername(string username);
        Task Add(User user);    
        Task Update(User user);
        Task<PagedList<User>> GetUsersBySearchQuery(string query, User currentUser, PagedParameters pagedParameters);
    }
}