using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Pagination;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IUsersService
    {
        Task<PagedList<UserDto>> SearchUsers(string query, string email, PagedParameters pagedParameters);
        Task<YearsDto> GetDashboardYear(string email);
        Task ChangeDashboardYear(string email, int year);
        Task<string> GetUserCurrencyPreference(string email);
        Task ChangeUserCurrencyPreference(string email, string currency);
    }
}