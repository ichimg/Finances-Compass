using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Pagination;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IFriendshipsService
    {
        Task<PagedList<UserDto>> GetUserFriendsByEmail(string email, PagedParameters pagedParameters);
    }
}