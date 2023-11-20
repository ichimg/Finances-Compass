using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Pagination;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IFriendshipRepository
    {
        Task<PagedList<User>> GetUserFriendsById(string userId, PagedParameters pagedParameters);
    }
}
