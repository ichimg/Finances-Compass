using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Enums;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Presentation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace DebtsCompass.Tests.Controllers
{
    [TestClass]
    public class AuthenticationControllerTests
    {
        private Mock<UserManager<User>> mockUserManager;
        private Mock<IJwtService> mockJwtService;
        private Mock<IAuthenticationService> mockAuthenticationService;
        private Mock<IEmailService> mockEmailService;

        private AuthenticationController sut;

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

            mockJwtService = new Mock<IJwtService>();
            mockAuthenticationService = new Mock<IAuthenticationService>();
            mockEmailService = new Mock<IEmailService>();

            sut = new AuthenticationController(mockJwtService.Object, mockAuthenticationService.Object, mockUserManager.Object, mockEmailService.Object);
        }

        [TestMethod]
        public async Task Login_ValidLogin_ReturnStatus200()
        {
            var returned = await sut.Login(It.IsAny<LoginRequest>());
            OkObjectResult okResult = returned.Result as OkObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [TestMethod]
        public async Task Register_ValidRegister_ReturnStatus200()
        {
            mockAuthenticationService.Setup(s => s.Register(It.IsAny<RegisterRequest>())).ReturnsAsync(ReturnsUser(CurrencyPreference.RON));
            mockUserManager.Setup(u => u.GenerateEmailConfirmationTokenAsync(It.IsAny<User>())).ReturnsAsync("token");

            var returned = await sut.Login(It.IsAny<LoginRequest>());
            OkObjectResult okResult = returned.Result as OkObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [TestMethod]
        public async Task RefreshToken_NullRequest_ReturnStatus400()
        {
            var returned = await sut.RefreshToken(null);

            BadRequestObjectResult badRequestResult = returned.Result as BadRequestObjectResult;

            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task RefreshToken_ValidRefreshToken_ReturnStatus200()
        {
            mockJwtService.Setup(s => s.GetRefreshToken(It.IsAny<string>(), It.IsAny<RefreshTokenRequest>())).ReturnsAsync(new RefreshTokenResponse());

            var returned = await sut.RefreshToken(new RefreshTokenRequest());
            OkObjectResult okResult = returned.Result as OkObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [TestMethod]
        public async Task EmailConfirmation_ValidRequest_ReturnStatus200()
        {
            var returned = await sut.EmailConfirmation(It.IsAny<string>(), It.IsAny<string>());
            OkObjectResult okResult = returned.Result as OkObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        private User ReturnsUser(CurrencyPreference currencyPreference)
        {
            return new User { CurrencyPreference = currencyPreference, Email = "test", DashboardSelectedYear = 1 };
        }
    }
}
