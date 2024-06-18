using DebtsCompass.Application.Exceptions;
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
    public class FriendshipsControllerTests
    {
        private Mock<IFriendshipsService> mockFriendshipService;
        private Mock<HttpContext> mockContext;
        private Mock<ClaimsPrincipal> mockUser;

        private FriendshipsController sut;

        [TestInitialize]
        public void Initialize()
        {
            mockFriendshipService = new Mock<IFriendshipsService>();
            mockUser = new Mock<ClaimsPrincipal>();
            mockContext = new Mock<HttpContext>();

            sut = new FriendshipsController(mockFriendshipService.Object);
        }

        [TestMethod]
        public async Task GetFriends_UnauthorizedEmail_ThrowException()
        {
            SetupUser("unauthorized");

            Assert.ThrowsExceptionAsync<ForbiddenRequestException>(() => sut.GetFriends(It.IsAny<string>(), It.IsAny<PagedParameters>()));
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
