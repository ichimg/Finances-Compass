using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Enums;
using DebtsCompass.Domain.Pagination;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IFriendshipRepository
    {
        Task<PagedList<User>> GetUserFriendsById(string userId, PagedParameters pagedParameters);
        Task<Friendship> GetUsersFriendStatus(User userOne, User userTwo);
        Task Add(Friendship friendship);
        Task Delete(Friendship friendship);
    }
}
