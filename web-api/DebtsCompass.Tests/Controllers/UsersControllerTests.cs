using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain.Pagination;
using DebtsCompass.Presentation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace DebtsCompass.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        private Mock<IUsersService> mockUsersService;
        private Mock<IUserRecommandationService> mockUserRecommandationService;
        private Mock<HttpContext> mockContext;
        private Mock<ClaimsPrincipal> mockUser;

        private UsersController sut;

        [TestInitialize]
        public void Initialize()
        {
            mockUsersService = new Mock<IUsersService>();
            mockUserRecommandationService = new Mock<IUserRecommandationService>();
            mockUser = new Mock<ClaimsPrincipal>();
            mockContext = new Mock<HttpContext>();

            sut = new UsersController(mockUsersService.Object, mockUserRecommandationService.Object);
        }

        [TestMethod]
        public async Task SearchUsers_UnauthorizedEmail_ThrowException()
        {
            SetupUser("email");

            Assert.ThrowsExceptionAsync<ForbiddenRequestException>(() => sut.SearchUsers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PagedParameters>()));
        }

        [TestMethod]
        public async Task SearchUsers_AuthorizedEmail_ReturnStatus200()
        {
            var authorizedEmail = "email";
            SetupUser(authorizedEmail);
            mockUsersService.Setup(s => s.SearchUsers(It.IsAny<string>(), authorizedEmail, It.IsAny<PagedParameters>())).ReturnsAsync(new PagedResponse<UserDto>(null, 0, 1, 10));

            var returned = await sut.SearchUsers(authorizedEmail, It.IsAny<string>(), It.IsAny<PagedParameters>());
            OkObjectResult okResult = returned.Result as OkObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [TestMethod]
        public async Task SearchUsers_AuthorizedEmail_ReturnExpectedUsers()
        {
            var expectedUsers = new PagedResponse<UserDto>(new List<UserDto> { new UserDto() }, 1, 1, 10);
            var authorizedEmail = "email";
            SetupUser(authorizedEmail);
            mockUsersService.Setup(s => s.SearchUsers(It.IsAny<string>(), authorizedEmail, It.IsAny<PagedParameters>())).ReturnsAsync(expectedUsers);

            var returned = await sut.SearchUsers(authorizedEmail, It.IsAny<string>(), It.IsAny<PagedParameters>());
            OkObjectResult okResult = returned.Result as OkObjectResult;
            var result = ((dynamic)okResult.Value).Payload;

            CollectionAssert.AreEqual(expectedUsers.Items, result.Items);
        }

        [TestMethod]
        public async Task GetDashboardYear_UnauthorizedEmail_ThrowException()
        {
            SetupUser("email");

            Assert.ThrowsExceptionAsync<ForbiddenRequestException>(() => sut.GetDashboardYear(It.IsAny<string>()));
        }

        [TestMethod]
        public async Task GetDashboardYear_AuthorizedEmail_ReturnStatus200()
        {
            var authorizedEmail = "email";
            SetupUser(authorizedEmail);
            mockUsersService.Setup(s => s.GetDashboardYear(authorizedEmail)).ReturnsAsync(new YearsDto());

            var returned = await sut.SearchUsers(authorizedEmail, It.IsAny<string>(), It.IsAny<PagedParameters>());
            OkObjectResult okResult = returned.Result as OkObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetDashboardYear_AuthorizedEmail_ReturnExpectedDashboardYear()
        {
            var expected = new YearsDto { RegisteredYear = 2023, DashboardSelectedYear = 2024 };
            var authorizedEmail = "email";
            SetupUser(authorizedEmail);
            mockUsersService.Setup(s => s.GetDashboardYear(authorizedEmail)).ReturnsAsync(expected);

            var returned = await sut.GetDashboardYear(authorizedEmail);
            OkObjectResult okResult = returned.Result as OkObjectResult;
            var result = ((dynamic)okResult.Value).Payload;

            Assert.AreEqual(expected.RegisteredYear, result.RegisteredYear);
            Assert.AreEqual(expected.DashboardSelectedYear, result.DashboardSelectedYear);
        }

        [TestMethod]
        public async Task ChangeDashboardYear_UnauthorizedEmail_ThrowException()
        {
            SetupUser("email");

            Assert.ThrowsExceptionAsync<ForbiddenRequestException>(() => sut.ChangeDashboardYear(It.IsAny<string>(), It.IsAny<int>()));
        }

        [TestMethod]
        public async Task ChangeDashboardYear_AuthorizedEmail_ReturnStatus200()
        {
            var authorizedEmail = "email";
            SetupUser(authorizedEmail);

            var returned = await sut.ChangeDashboardYear(authorizedEmail, It.IsAny<int>());
            OkObjectResult okResult = returned.Result as OkObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetCurrencyPreference_UnauthorizedEmail_ThrowException()
        {
            SetupUser("email");

            Assert.ThrowsExceptionAsync<ForbiddenRequestException>(() => sut.GetCurrencyPreference(It.IsAny<string>()));
        }

        [TestMethod]
        public async Task GetCurrencyPreference_AuthorizedEmail_ReturnStatus200()
        {
            var authorizedEmail = "email";
            SetupUser(authorizedEmail);
            mockUsersService.Setup(s => s.GetUserCurrencyPreference(authorizedEmail)).ReturnsAsync("RON");

            var returned = await sut.GetCurrencyPreference(authorizedEmail);
            OkObjectResult okResult = returned.Result as OkObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetCurrencyPreference_AuthorizedEmail_ReturnExpectedDashboardYear()
        {
            var expected = "RON";
            var authorizedEmail = "email";
            SetupUser(authorizedEmail);
            mockUsersService.Setup(s => s.GetUserCurrencyPreference(authorizedEmail)).ReturnsAsync("RON");

            var returned = await sut.GetCurrencyPreference(authorizedEmail);
            OkObjectResult okResult = returned.Result as OkObjectResult;
            var result = ((dynamic)okResult.Value).Payload;

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task ChangeCurrencyPreference_UnauthorizedEmail_ThrowException()
        {
            SetupUser("email");

            Assert.ThrowsExceptionAsync<ForbiddenRequestException>(() => sut.ChangeCurrencyPreference(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public async Task ChangeCurrencyPreference_AuthorizedEmail_ReturnStatus200()
        {
            var authorizedEmail = "email";
            SetupUser(authorizedEmail);

            var returned = await sut.ChangeCurrencyPreference(authorizedEmail, It.IsAny<string>());
            OkObjectResult okResult = returned.Result as OkObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetSimilarUsers_UnauthorizedEmail_ThrowException()
        {
            SetupUser("email");

            Assert.ThrowsExceptionAsync<ForbiddenRequestException>(() => sut.GetSimilarUsers(It.IsAny<string>(), It.IsAny<int>()));
        }

        [TestMethod]
        public async Task GetSimilarUsers_AuthorizedEmail_ReturnStatus200()
        {
            var authorizedEmail = "email";
            SetupUser(authorizedEmail);
            mockUserRecommandationService.Setup(s => s.RecommendSimilarUsers(authorizedEmail, It.IsAny<int>())).ReturnsAsync(new List<UserDto>());

            var returned = await sut.SearchUsers(authorizedEmail, It.IsAny<string>(), It.IsAny<PagedParameters>());
            OkObjectResult okResult = returned.Result as OkObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetSimilarUsers_AuthorizedEmail_ReturnExpectedUsers()
        {
            var expectedUsers = new List<UserDto> { new UserDto() };
            var authorizedEmail = "email";
            SetupUser(authorizedEmail);
            mockUserRecommandationService.Setup(s => s.RecommendSimilarUsers(authorizedEmail, It.IsAny<int>())).ReturnsAsync(expectedUsers);

            var returned = await sut.GetSimilarUsers(authorizedEmail, It.IsAny<int>());
            OkObjectResult okResult = returned.Result as OkObjectResult;
            var result = ((dynamic)okResult.Value).Payload;

            CollectionAssert.AreEqual(expectedUsers, result);
        }

        private void SetupUser(string email)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email)
        };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);

            mockUser.Setup(u => u.Identity).Returns(identity);
            mockUser.Setup(u => u.FindFirst(ClaimTypes.Email)).Returns(claims.First(c => c.Type == ClaimTypes.Email));

            mockContext.SetupGet(hc => hc.User).Returns(mockUser.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = mockContext.Object
            };
        }

    }
}
