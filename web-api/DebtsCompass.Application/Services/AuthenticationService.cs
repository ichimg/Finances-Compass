using BCrypt.Net;
using DebtsCompass.Application.Exceptions;
using DebtsCompass.Application.Validators;
using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace DebtsCompass.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly EmailValidator emailValidator;
        private readonly PasswordValidator passwordValidator;
        private readonly IUserRepository userRepository;
        private readonly UserManager<User> userManager;
        private readonly INonUserRepository nonUserRepository;
        private readonly IDebtAssignmentRepository debtAssignmentRepository;
        public AuthenticationService(IUserRepository userRepository,
            EmailValidator emailValidator,
            PasswordValidator passwordValidator,
            UserManager<User> userManager,
            INonUserRepository nonUserRepository,
            IDebtAssignmentRepository debtAssignmentRepository)
        {
            this.userRepository = userRepository;
            this.emailValidator = emailValidator;
            this.passwordValidator = passwordValidator;
            this.userManager = userManager;
            this.nonUserRepository = nonUserRepository;
            this.debtAssignmentRepository = debtAssignmentRepository;
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

            User userFromDb = await userRepository.GetUserByEmail(loginRequest.Email) ?? 
                              throw new UserNotFoundException(loginRequest.Email);

            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, userFromDb.PasswordHash))
            {
                return false;
            }

            if (userFromDb.EmailConfirmed is false)
            {
                throw new EmailNotConfirmedException(userFromDb.Email);
            }

            return true;
        }

        public async Task<User> Register(RegisterRequest registerRequest)
        {
            if (!emailValidator.IsValid(registerRequest.Email))
            {
                throw new InvalidEmailException(registerRequest.Email);
            }

            if (!passwordValidator.IsValid(registerRequest.Password))
            {
                throw new InvalidPasswordException();
            }

            if (!registerRequest.Password.Equals(registerRequest.ConfirmPassword))
            {
                throw new PasswordMismatchException();
            }

            User existingUser = await userRepository.GetUserByEmail(registerRequest.Email);

            if (existingUser is not null)
            {
                throw new EmailAlreadyExistsException();
            }

            existingUser = await userRepository.GetUserByUsername(registerRequest.Username);

            if (existingUser is not null)
            {
                throw new UsernameAlreadyExistsException();
            }

            User user = Mapper.RegisterRequestToUserDbModel(registerRequest);

            await userRepository.Add(user);

            NonUser nonUser = await nonUserRepository.GetNonUserByEmail(user.Email);

            if (nonUser is not null)
            {
                await MoveExistingNonAccountDebts(user);
                await nonUserRepository.Delete(nonUser);
            }

            return user;
        }

        public async Task ConfirmEmail(string email, string token)
        {
            User user = await userRepository.GetUserByEmail(email);

            if (user.EmailConfirmed == true)
            {
                throw new EmailAlreadyConfirmedException(email);
            }

            var confirmResult = await userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
            {
                throw new ForbiddenRequestException();
            }
        }

        private async Task MoveExistingNonAccountDebts(User user)
        {
            var existingNonUserDebts = await debtAssignmentRepository.GetAllNonUserDebtsByEmail(user.Email);

            if (existingNonUserDebts.Any())
            {
                List<DebtAssignment> movedToActualUserDebts = existingNonUserDebts.Select(d =>
                {
                    d.SelectedUser = user;
                    d.NonUser = null;
                    return d;
                }).ToList();

                await debtAssignmentRepository.UpdateRange(movedToActualUserDebts);
            }
        }
    }
}
