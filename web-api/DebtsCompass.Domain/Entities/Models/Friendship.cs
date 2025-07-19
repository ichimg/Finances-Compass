using DebtsCompass.Domain.Enums;

namespace DebtsCompass.Domain.Entities.Models
{
    public class Friendship
    {
        public string RequesterUserId { get; set; }
        public string SelectedUserId { get; set; }
        public User RequesterUser { get; set; }
        public User SelectedUser { get; set; }
        public Status Status { get; set; }
    }
}
