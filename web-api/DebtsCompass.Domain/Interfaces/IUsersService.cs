using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Pagination;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IUsersService
    {
        Task<PagedList<UserDto>> SearchUsers(string query, string email, PagedParameters pagedParameters);
    }
}