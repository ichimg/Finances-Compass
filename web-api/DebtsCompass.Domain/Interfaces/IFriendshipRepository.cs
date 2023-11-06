using DebtsCompass.Domain.Entities.Models;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IFriendshipRepository
    {
        Task<IEnumerable<User>> GetUserFriendsById(string userId);
    }
}