using BCrypt.Net;
using DebtsCompass.Application.Validators;
using DebtsCompass.Domain.Entities;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain.Requests;

namespace DebtsCompass.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly EmailValidator emailValidator;
        private readonly PasswordValidator passwordValidator;
        private readonly IUserRepository userRepository;
        public AuthenticationService(IUserRepository userRepository, EmailValidator emailValidator, PasswordValidator passwordValidator)
        {
            this.userRepository = userRepository;
            this.emailValidator = emailValidator;
            this.passwordValidator = passwordValidator;
        }

        public async Task<bool> IsValidLogin(LoginRequest loginRequest)
        { 
            if (!emailValidator.IsValid(loginRequest.Email))
            {
                return false;
            }

            if (!passwordValidator.IsValid(loginRequest.Password))
            {
                return false;
            }

            User userFromDb = await userRepository.GetUserByEmail(loginRequest.Email);

            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, userFromDb.PasswordHash))
            {
                return false;
            }

            return true;
        }
    }
}
