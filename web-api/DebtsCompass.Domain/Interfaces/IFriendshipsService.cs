using DebtsCompass.Domain.Entities.DtoResponses;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IFriendshipsService
    {
        Task<List<UserDto>> GetUserFriendsByEmail(string email);
    }
}