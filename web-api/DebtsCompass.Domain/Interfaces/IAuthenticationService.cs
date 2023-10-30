using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Entities.Requests;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> IsValidLogin(LoginRequest loginRequest);
        Task<User> Register(RegisterRequest registerRequest);
    }
}