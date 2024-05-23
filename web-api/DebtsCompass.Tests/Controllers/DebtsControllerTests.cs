using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Presentation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Security.Claims;

namespace DebtsCompass.Tests.Controllers
{
    [TestClass]
    public class DebtsControllerTests
    {
        private Mock<IDebtsService> mockDebtsService;
        private Mock<HttpContext> mockContext;
        private Mock<ClaimsPrincipal> mockUser;

        private DebtsController sut;
        [TestInitialize]
        public void Initialize()
        {
            mockDebtsService = new Mock<IDebtsService>();
            mockUser = new Mock<ClaimsPrincipal>();
            mockContext = new Mock<HttpContext>();

            sut = new DebtsController(mockDebtsService.Object);
        }

        [TestMethod]
        public async Task GetReceivingDebts_UnauthorizedEmail_ThrowException()
        {
            SetupUser("email");

            await Assert.ThrowsExceptionAsync<ForbiddenRequestException>(() => sut.GetReceivingDebts(It.IsAny<string>()));
        }

        [TestMethod]
        public async Task GetReceivingDebts_AuthorizedEmail_ReturnStatus200()
        {
            string authorizedEmail = "email";
            SetupUser(authorizedEmail);

            var returned = await sut.GetReceivingDebts(authorizedEmail);
            OkObjectResult okResult = returned.Result as OkObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetReceivingDebts_AuthorizedEmail_ReturnExpectedDebts()
        {
            string authorizedEmail = "email";
            SetupUser(authorizedEmail);
            var expectedDebts = new List<DebtDto>
            {
                new DebtDto { Email = "test1email@fake.com" },
                new DebtDto { Email = "test2email@fake.com" }
            };

            mockDebtsService.Setup(s => s.GetAllReceivingDebts(It.IsAny<string>())).ReturnsAsync(expectedDebts);

            var returned = await sut.GetReceivingDebts(authorizedEmail);
            OkObjectResult okResult = returned.Result as OkObjectResult;
            var result = ((dynamic)okResult.Value).Payload;

            CollectionAssert.AreEqual(expectedDebts, result);
            Assert.AreEqual(expectedDebts.Count, result.Count);
        }

        [TestMethod]
        public async Task GetUserDebts_UnauthorizedEmail_ThrowException()
        {
            SetupUser("email");

            await Assert.ThrowsExceptionAsync<ForbiddenRequestException>(() => sut.GetUserDebts(It.IsAny<string>()));
        }

        [TestMethod]
        public async Task GetUserDebts_AuthorizedEmail_ReturnStatus200()
        {
            string authorizedEmail = "email";
            SetupUser(authorizedEmail);

            var returned = await sut.GetUserDebts(authorizedEmail);
            OkObjectResult okResult = returned.Result as OkObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetUserDebts_AuthorizedEmail_ReturnExpectedDebts()
        {
            string authorizedEmail = "email";
            SetupUser(authorizedEmail);
            var expectedDebts = new List<DebtDto>
            {
                new DebtDto { Email = "test1email@fake.com" },
                new DebtDto { Email = "test2email@fake.com" }
            };

            mockDebtsService.Setup(s => s.GetAllUserDebts(It.IsAny<string>())).ReturnsAsync(expectedDebts);

            var returned = await sut.GetUserDebts(authorizedEmail);
            OkObjectResult okResult = returned.Result as OkObjectResult;
            var result = ((dynamic)okResult.Value).Payload;

            CollectionAssert.AreEqual(expectedDebts, result);
            Assert.AreEqual(expectedDebts.Count, result.Count);
        }

        [TestMethod]
        public async Task CreateDebt_UnauthorizedEmail_ThrowException()
        {
            SetupUser("email");

            await Assert.ThrowsExceptionAsync<ForbiddenRequestException>(() => sut.CreateDebt(It.IsAny<CreateDebtRequest>(), It.IsAny<string>()));
        }

        [TestMethod]
        public async Task CreateDebt_AuthorizedEmail_ReturnStatus201()
        {
            string authorizedEmail = "email";
            SetupUser(authorizedEmail);

            var returned = await sut.CreateDebt(It.IsAny<CreateDebtRequest>(), authorizedEmail);
            OkObjectResult okResult = returned.Result as OkObjectResult;
            var statusCode = ((dynamic)okResult.Value).StatusCode;

            Assert.AreEqual(HttpStatusCode.Created, statusCode);
        }

        [TestMethod]
        public async Task CreateDebt_AuthorizedEmail_ReturnExpectedDebtGuid()
        {
            string authorizedEmail = "email";
            SetupUser(authorizedEmail);
            var expectedGuid = Guid.NewGuid();

            mockDebtsService.Setup(s => s.CreateDebt(It.IsAny<CreateDebtRequest>(), authorizedEmail)).ReturnsAsync(expectedGuid);

            var returned = await sut.CreateDebt(It.IsAny<CreateDebtRequest>(), authorizedEmail);
            OkObjectResult okResult = returned.Result as OkObjectResult;
            var result = ((dynamic)okResult.Value).Payload;
            var statusCode = ((dynamic)okResult.Value).StatusCode;

            Assert.AreEqual(expectedGuid, result);
            Assert.AreEqual(HttpStatusCode.Created, statusCode);
        }

        [TestMethod]
        public async Task DeleteDebt_UnauthorizedEmail_ThrowException()
        {
            SetupUser("email");

            await Assert.ThrowsExceptionAsync<ForbiddenRequestException>(() => sut.DeleteDebt(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public async Task DeleteDebt_AuthorizedEmail_ReturnStatus200()
        {
            string authorizedEmail = "email";
            SetupUser(authorizedEmail);

            var returned = await sut.DeleteDebt(It.IsAny<string>(), authorizedEmail);
            OkObjectResult okResult = returned.Result as OkObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [TestMethod]
        public async Task EditDebt_UnauthorizedEmail_ThrowException()
        {
            SetupUser("email");

            await Assert.ThrowsExceptionAsync<ForbiddenRequestException>(() => sut.EditDebt(It.IsAny<EditDebtRequest>(), It.IsAny<string>()));
        }

        [TestMethod]
        public async Task EditDebtt_AuthorizedEmail_ReturnStatus201()
        {
            string authorizedEmail = "email";
            SetupUser(authorizedEmail);

            var returned = await sut.EditDebt(It.IsAny<EditDebtRequest>(), authorizedEmail);
            OkObjectResult okResult = returned.Result as OkObjectResult;
            var statusCode = ((dynamic)okResult.Value).StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, statusCode);
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