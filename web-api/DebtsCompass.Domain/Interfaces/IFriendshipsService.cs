using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Pagination;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IFriendshipsService
    {
        Task AddFriend(FriendRequest friendRequest);
        Task<PagedList<UserDto>> GetUserFriendsById(string email, PagedParameters pagedParameters);
        Task DeleteFriendRequest(FriendRequestDto friendRequestDto);
        Task AcceptFriendRequest(FriendRequestDto friendRequestDto);
        Task RejectFriendRequest(FriendRequestDto friendRequestDto);
        Task<PagedList<UserDto>> GetUserFriendRequestsById(string email, PagedParameters pagedParameters);
    }
}