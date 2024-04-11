using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Enums;
using DebtsCompass.Domain.Pagination;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IFriendshipRepository
    {
        Task<PagedList<User>> GetUserFriendsById(string userId, PagedParameters pagedParameters);
        Task<Friendship> GetUsersFriendship(User userOne, User userTwo);
        Task Add(Friendship friendship);
        Task Delete(Friendship friendship);
        Task AcceptFriendRequest(Friendship friendshipFromDb);
        Task RejectFriendRequest(Friendship friendshipFromDb);
        Task<PagedList<User>> GetUserFriendRequestsById(string userId, PagedParameters pagedParameters);
        Task<List<User>> GetAllUserFriendsById(string id);
    }
}
