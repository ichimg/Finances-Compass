namespace DebtsCompass.Domain.Entities.Requests
{
    public class DeleteFriendRequest
    {
        public string RequesterUserEmail { get; set; }
        public string ReceiverUserEmail { get; set; }
    }
}
