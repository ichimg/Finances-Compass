namespace DebtsCompass.Domain.Entities.DtoResponses
{
    public class LoginResponse
    {
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
