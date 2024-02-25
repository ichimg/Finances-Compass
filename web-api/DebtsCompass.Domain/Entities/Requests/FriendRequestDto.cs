namespace DebtsCompass.Domain.Entities.Requests
{
    public class FriendRequestDto
    {
        public string RequesterUserEmail { get; set; }
        public string SelectedUserEmail { get; set; }
    }
}
