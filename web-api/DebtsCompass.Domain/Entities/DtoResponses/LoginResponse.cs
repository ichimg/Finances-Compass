﻿using DebtsCompass.Domain.Enums;

namespace DebtsCompass.Domain.Entities.DtoResponses
{
    public class LoginResponse
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string CurrencyPreference { get; set; }
        public bool IsDataConsent { get; set; }
    }
}
