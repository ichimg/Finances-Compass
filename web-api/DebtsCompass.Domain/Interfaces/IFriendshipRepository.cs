using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Enums;
using DebtsCompass.Domain.Pagination;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IFriendshipRepository
    {
        Task<PagedResponse<User>> GetUserFriendsById(string userId, PagedParameters pagedParameters);
        Task<Friendship> GetUsersFriendship(string requesterUserId, string receiverUserId);
        Task Add(Friendship friendship);
        Task Delete(Friendship friendship);
        Task AcceptFriendRequest(Friendship friendshipFromDb);
        Task RejectFriendRequest(Friendship friendshipFromDb);
        Task<PagedResponse<User>> GetUserFriendRequestsById(string userId, PagedParameters pagedParameters);
        Task<List<User>> GetAllUserFriendsById(string id);
    }
}
