﻿using DebtsCompass.Domain.Enums;

namespace DebtsCompass.Domain.Entities.DtoResponses
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FriendStatus { get; set; }
        public bool IsPendingFriendRequest { get; set; }
    }
}
