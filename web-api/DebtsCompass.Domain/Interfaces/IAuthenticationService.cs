using DebtsCompass.Domain.Requests;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> IsValidLogin(LoginRequest loginRequest);
    }
}