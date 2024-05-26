using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Pagination;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IFriendshipsService
    {
        Task AddFriend(FriendRequest friendRequest);
        Task<PagedResponse<UserDto>> GetUserFriendsByEmail(string email, PagedParameters pagedParameters);
        Task<List<UserDto>> GetAllUserFriends(string email);
        Task DeleteFriendRequest(FriendRequestDto friendRequestDto);
        Task AcceptFriendRequest(FriendRequestDto friendRequestDto);
        Task RejectFriendRequest(FriendRequestDto friendRequestDto);
        Task<PagedResponse<UserDto>> GetUserFriendRequestsById(string email, PagedParameters pagedParameters);
    }
}