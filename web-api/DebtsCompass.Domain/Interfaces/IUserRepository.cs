using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Enums;
using DebtsCompass.Domain.Pagination;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
        Task<List<User>> GetAllAllowedDataConsent(string targetUserEmail);
        Task<User> GetUserByEmailWithExpensesByMonth(string email, YearMonthDto yearMonthDto);
        Task<User> GetUserByUsername(string username);
        Task Add(User user);    
        Task Update(User user);
        Task<PagedResponse<User>> GetUsersBySearchQuery(string query, User currentUser, PagedParameters pagedParameters);
        Task ChangeDashboardYear(User user, int year);
        Task ChangeCurrencyPreference(User user, CurrencyPreference currency);
    }
}