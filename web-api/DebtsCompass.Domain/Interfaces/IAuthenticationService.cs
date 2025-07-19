using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Entities.Requests;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> IsValidLogin(LoginRequest loginRequest);
        Task<User> Register(RegisterRequest registerRequest);
        Task ConfirmEmail(string email, string token);
        Task<LoginResponse> GetLoginResponse(LoginRequest loginRequest);
    }
}