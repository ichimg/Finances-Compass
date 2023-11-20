using DebtsCompass.Domain.Enums;

namespace DebtsCompass.Domain.Entities.DtoResponses
{
    public class UserFriendDto
    {
        UserDto UserDto { get; set; }
        Status FriendStatus { get; set; }
    }
}
