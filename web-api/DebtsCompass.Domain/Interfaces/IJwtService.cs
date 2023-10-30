using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Requests;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string email);
        string GenerateRefreshToken();
        Task<RefreshTokenResponse> GetRefreshToken(string email, RefreshTokenRequest refreshTokenRequest);
        Task UpdateRefreshToken(string email, string token);
    }
}