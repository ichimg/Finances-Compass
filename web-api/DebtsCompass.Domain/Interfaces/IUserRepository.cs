using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Pagination;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByEmailWithExpenses(string email, YearMonthDto yearMonthDto);
        Task<User> GetUserByUsername(string username);
        Task Add(User user);    
        Task Update(User user);
        Task<PagedList<User>> GetUsersBySearchQuery(string query, User currentUser, PagedParameters pagedParameters);
    }
}