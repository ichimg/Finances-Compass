using DebtsCompass.Application.Exceptions;
using DebtsCompass.Application.Services;
using DebtsCompass.Application.Validators;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Enums;
using DebtsCompass.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace DebtsCompass.Tests.Services
{
    [TestClass]
    public class AuthenticationServiceTests
    {
        private Mock<IUserRepository> mockUserRepository;
        private Mock<UserManager<User>> mockUserManager;
        private Mock<INonUserRepository> mockNonUserRepository;
        private Mock<IDebtAssignmentRepository> mockDebtAssignmentRepository;
        private Mock<IJwtService> mockJwtService;

        private AuthenticationService sut;

        [TestInitialize]
        public void Initialize()
        {
            mockUserManager = new Mock<UserManager<User>>(
               new Mock<IUserStore<User>>().Object,
               new Mock<IOptions<IdentityOptions>>().Object,
               new Mock<IPasswordHasher<User>>().Object,
               new IUserValidator<User>[0],
               new IPasswordValidator<User>[0],
               new Mock<ILookupNormalizer>().Object,
               new Mock<IdentityErrorDescriber>().Object,
               new Mock<IServiceProvider>().Object,
               new Mock<ILogger<UserManager<User>>>().Object);

            mockUserRepository = new Mock<IUserRepository>();
            mockNonUserRepository = new Mock<INonUserRepository>();
            mockDebtAssignmentRepository = new Mock<IDebtAssignmentRepository>();
            mockJwtService = new Mock<IJwtService>();

            sut = new AuthenticationService(mockUserRepository.Object, new EmailValidator(), new PasswordValidator(),
                mockUserManager.Object, mockNonUserRepository.Object, mockDebtAssignmentRepository.Object, mockJwtService.Object);
        }

        [TestMethod]
        public async Task IsValidLogin_InvalidEmail_ReturnFalse()
        {
            LoginRequest loginRequest = new LoginRequest { Email = "invalid" };

            var result = await sut.IsValidLogin(loginRequest);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task IsValidLogin_InvalidPassword_ReturnFalse()
        {
            LoginRequest loginRequest = new LoginRequest { Email = "valid@email.com", Password = "invalid" };

            var result = await sut.IsValidLogin(loginRequest);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task IsValidLogin_UserNotFound_ThrowException()
        {
            LoginRequest loginRequest = new LoginRequest { Email = "valid@email.com", Password = "ValidPass123" };
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(value: null);

            Assert.ThrowsExceptionAsync<UserNotFoundException>(() => sut.IsValidLogin(loginRequest));
        }

        [TestMethod]
        public async Task IsValidLogin_WrongPassword_ReturnFalse()
        {
            var wrongHash = BCrypt.Net.BCrypt.HashPassword("wrongPassword");
            LoginRequest loginRequest = new LoginRequest { Email = "valid@email.com", Password = "ValidPass123" };
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User { PasswordHash = wrongHash });

            var result = await sut.IsValidLogin(loginRequest);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task IsValidLogin_EmailNotConfirmed_ThrowException()
        {
            var correctHash = BCrypt.Net.BCrypt.HashPassword("ValidPass123");
            LoginRequest loginRequest = new LoginRequest { Email = "valid@email.com", Password = "ValidPass123" };
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User { PasswordHash = correctHash, EmailConfirmed = false });

            Assert.ThrowsExceptionAsync<EmailNotConfirmedException>(() => sut.IsValidLogin(loginRequest));
        }

        [TestMethod]
        public async Task IsValidLogin_CorrectLogin_ReturnTrue()
        {
            var correctHash = BCrypt.Net.BCrypt.HashPassword("ValidPass123");
            LoginRequest loginRequest = new LoginRequest { Email = "valid@email.com", Password = "ValidPass123" };
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User { PasswordHash = correctHash, EmailConfirmed = true });

            var result = await sut.IsValidLogin(loginRequest);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task GetLoginResponse_IsNotValidLogin_ThrowException()
        {
            Assert.ThrowsExceptionAsync<InvalidCredentialsException>(() => sut.GetLoginResponse(It.IsAny<LoginRequest>()));
        }

        [TestMethod]
        public async Task GetLoginResponse_IsValidLogin_ReturnExpectedLoginResponse()
        {
            var expected = new LoginResponse
            {
                Email = "valid@email.com",
                FirstName = "firstName",
                AccessToken = "aToken",
                RefreshToken = "rToken",
                CurrencyPreference = CurrencyPreference.RON.ToString(),
                IsDataConsent = false
            };

            var correctHash = BCrypt.Net.BCrypt.HashPassword("ValidPass123");
            LoginRequest loginRequest = new LoginRequest { Email = "valid@email.com", Password = "ValidPass123" };
            mockUserRepository.SetupSequence(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User { PasswordHash = correctHash, EmailConfirmed = true }).ReturnsAsync(
                new User { Email = expected.Email, UserInfo = new UserInfo { FirstName = expected.FirstName }, CurrencyPreference = CurrencyPreference.RON }); ;
            mockJwtService.Setup(s => s.GenerateRefreshToken()).Returns(expected.RefreshToken);
            mockJwtService.Setup(s => s.GenerateToken(It.IsAny<string>())).Returns(expected.AccessToken);

            var result = await sut.GetLoginResponse(loginRequest);

            Assert.AreEqual(expected.Email, result.Email);
            Assert.AreEqual(expected.FirstName, result.FirstName);
            Assert.AreEqual(expected.AccessToken, result.AccessToken);
            Assert.AreEqual(expected.RefreshToken, result.RefreshToken);
            Assert.AreEqual(expected.CurrencyPreference, result.CurrencyPreference);
            Assert.AreEqual(expected.IsDataConsent, result.IsDataConsent);
        }

        [TestMethod]
        public async Task Register_InvalidEmail_ThrowException()
        {
            var registerRequest = new RegisterRequest { Email = "invalid" };
            Assert.ThrowsExceptionAsync<InvalidEmailException>(() => sut.Register(registerRequest));
        }

        [TestMethod]
        public async Task Register_InvalidPassword_ThrowException()
        {
            var registerRequest = new RegisterRequest { Email = "valid@email.com", Password = "invalid" };
            Assert.ThrowsExceptionAsync<InvalidPasswordException>(() => sut.Register(registerRequest));
        }

        [TestMethod]
        public async Task Register_PasswordsMismatch_ThrowException()
        {
            var registerRequest = new RegisterRequest { Email = "valid@email.com", Password = "ValidPass123", ConfirmPassword = "NotEqual123" };
            Assert.ThrowsExceptionAsync<PasswordMismatchException>(() => sut.Register(registerRequest));
        }

        [TestMethod]
        public async Task Register_ExistingEmail_ThrowException()
        {
            var registerRequest = new RegisterRequest { Email = "valid@email.com", Password = "ValidPass123", ConfirmPassword = "ValidPass123" };
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User());
            Assert.ThrowsExceptionAsync<EmailAlreadyExistsException>(() => sut.Register(registerRequest));
        }

        [TestMethod]
        public async Task Register_ExistingUsername_ThrowException()
        {
            var registerRequest = new RegisterRequest { Email = "valid@email.com", Password = "ValidPass123", ConfirmPassword = "ValidPass123" };
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(value: null);
            mockUserRepository.Setup(r => r.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(new User());

            Assert.ThrowsExceptionAsync<UsernameAlreadyExistsException>(() => sut.Register(registerRequest));
        }

        [TestMethod]
        public async Task Register_CorrectRegister_ReturnExpectedUser()
        {
            var registerRequest = new RegisterRequest { Email = "valid@email.com", Password = "ValidPass123", ConfirmPassword = "ValidPass123", CurrencyPreference = "RON" };
            User expected = new User { Email = registerRequest.Email };
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(value: null);
            mockUserRepository.Setup(r => r.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(value: null);
            mockNonUserRepository.Setup(r => r.GetNonUserByEmail(It.IsAny<string>())).ReturnsAsync(value: null);

            var result = await sut.Register(registerRequest);

            Assert.AreEqual(expected.Email, result.Email);
        }

        [TestMethod]
        public async Task Register_ExistingNonUser_DeleteIsCalled()
        {
            var registerRequest = new RegisterRequest { Email = "valid@email.com", Password = "ValidPass123", ConfirmPassword = "ValidPass123", CurrencyPreference = "RON" };
            User expected = new User { Email = registerRequest.Email };
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(value: null);
            mockUserRepository.Setup(r => r.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(value: null);
            mockNonUserRepository.Setup(r => r.GetNonUserByEmail(It.IsAny<string>())).ReturnsAsync(new NonUser());
            mockDebtAssignmentRepository.Setup(r => r.GetAllNonUserDebtsByEmail(It.IsAny<string>())).ReturnsAsync(new List<DebtAssignment>());

            var result = await sut.Register(registerRequest);

            mockNonUserRepository.Verify(r => r.Delete(It.IsAny<NonUser>()), Times.Once);
        }

        [TestMethod]
        public async Task Register_ExistingNonUserHasDebts_UpdateRangeIsCalled()
        {
            var registerRequest = new RegisterRequest { Email = "valid@email.com", Password = "ValidPass123", ConfirmPassword = "ValidPass123", CurrencyPreference = "RON" };
            User expected = new User { Email = registerRequest.Email };
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(value: null);
            mockUserRepository.Setup(r => r.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(value: null);
            mockNonUserRepository.Setup(r => r.GetNonUserByEmail(It.IsAny<string>())).ReturnsAsync(new NonUser());
            mockDebtAssignmentRepository.Setup(r => r.GetAllNonUserDebtsByEmail(It.IsAny<string>())).ReturnsAsync(new List<DebtAssignment> { new DebtAssignment()});

            var result = await sut.Register(registerRequest);

            mockDebtAssignmentRepository.Verify(r => r.UpdateRange(It.IsAny<IEnumerable<DebtAssignment>>()), Times.Once);
        }

        [TestMethod]
        public async Task ConfirmEmail_EmailAlreadyConfirmed_ThrowException()
        {
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User { EmailConfirmed = true });

            Assert.ThrowsExceptionAsync<EmailAlreadyConfirmedException>(() => sut.ConfirmEmail(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public async Task ConfirmEmail_NotSucceeded_ThrowException()
        {
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User { EmailConfirmed = false });
            mockUserManager.Setup(u => u.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(new IdentityResult());

            Assert.ThrowsExceptionAsync<ForbiddenRequestException>(() => sut.ConfirmEmail(It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}
