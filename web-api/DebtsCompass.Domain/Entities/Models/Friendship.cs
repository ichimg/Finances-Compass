using DebtsCompass.Domain.Enums;

namespace DebtsCompass.Domain.Entities.Models
{
    public class Friendship
    {
        public string UserOneId { get; set; }
        public string UserTwoId { get; set; }
        public User UserOne { get; set; }
        public User UserTwo { get; set; }
        public Status Status { get; set; }
    }
}
