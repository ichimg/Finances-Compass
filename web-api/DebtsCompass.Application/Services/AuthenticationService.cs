using BCrypt.Net;
using DebtsCompass.Application.Exceptions;
using DebtsCompass.Application.Validators;
using DebtsCompass.Domain;
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

            if(userFromDb is null)
            {
                throw new UserNotFoundException(loginRequest.Email);
            }

            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, userFromDb.PasswordHash))
            {
                return false;
            }

            return true;
        }

        public async Task Register(RegisterRequest registerRequest)
        {
            if (!emailValidator.IsValid(registerRequest.Email))
            {
                throw new InvalidEmailException(registerRequest.Email);
            }

            if(!passwordValidator.IsValid(registerRequest.Password))
            {
                throw new InvalidPasswordException();
            }

            if(!registerRequest.Password.Equals(registerRequest.ConfirmPassword))
            {
                throw new PasswordMismatchException();
            }

            User existingUser = await userRepository.GetUserByEmail(registerRequest.Email);

            if (existingUser is not null)
            {
                throw new EmailAlreadyExistsException(registerRequest.Email);
            }

            User user = Mapper.RegisterRequestToUserDbModel(registerRequest);

            await userRepository.Add(user);
        }


    }
}
