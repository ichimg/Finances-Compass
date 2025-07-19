using DebtsCompass.Domain.Enums;

namespace DebtsCompass.Domain.Entities.Requests
{
    public class FriendRequest
    {
        public string RequesterUserEmail { get; set; }
        public string ReceiverUserEmail { get; set; }
        public string Status { get; set; }
    }
}
