namespace DebtsCompass.Domain.Entities.Requests
{
    public class RefreshTokenRequest
    {
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
