using DebtsCompass.Application.Exceptions;
using DebtsCompass.Application.Services;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Enums;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain.Pagination;
using Moq;

namespace DebtsCompass.Tests.Services
{
    [TestClass]
    public class UsersServiceTests
    {
        private Mock<IUserRepository> mockUserRepository;
        private Mock<IFriendshipRepository> mockFriendshipRepository;

        private UsersService sut;

        [TestInitialize]
        public void Initialize()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockFriendshipRepository = new Mock<IFriendshipRepository>();

            sut = new UsersService(mockUserRepository.Object, mockFriendshipRepository.Object);
        }

        [TestMethod]
        public async Task SearchUsers_RetrieveFriendshipStatus_ReturnStatus()
        {
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User());
            mockUserRepository.Setup(r => r.GetUsersBySearchQuery(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<PagedParameters>())).ReturnsAsync(new PagedResponse<User>(new List<User>{ new User() }, 1, 1, 10));
            mockFriendshipRepository.Setup(r => r.GetUsersFriendship(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Friendship { Status = Status.Accepted });

            var result = await sut.SearchUsers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PagedParameters>());

            Assert.AreEqual(Status.Accepted.ToString(), result.Items[0].FriendStatus);
        }

        [TestMethod]
        public async Task SearchUsers_IsPendingFriendStatus_ReturnIsPendingFriendRequestTrue()
        {
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User());
            mockUserRepository.Setup(r => r.GetUsersBySearchQuery(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<PagedParameters>())).ReturnsAsync(new PagedResponse<User>(new List<User> { new User() }, 1, 1, 10));
            mockFriendshipRepository.SetupSequence(r => r.GetUsersFriendship(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(value: null)
                .ReturnsAsync(new Friendship { Status = Status.Pending });

            var result = await sut.SearchUsers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PagedParameters>());

            Assert.AreEqual(true, result.Items[0].IsPendingFriendRequest);
        }

        [TestMethod]
        public async Task SearchUsers_NoFriendship_ReturnNoneStatus()
        {
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User());
            mockUserRepository.Setup(r => r.GetUsersBySearchQuery(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<PagedParameters>())).ReturnsAsync(new PagedResponse<User>(new List<User> { new User() }, 1, 1, 10));
            mockFriendshipRepository.SetupSequence(r => r.GetUsersFriendship(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(value: null)
                .ReturnsAsync(value: null);

            var result = await sut.SearchUsers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PagedParameters>());

            Assert.AreEqual(Status.None.ToString(), result.Items[0].FriendStatus);
        }

        [TestMethod]
        public async Task GetDashboardYear_NullUser_ThrowException()
        {
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(value: null);

            Assert.ThrowsExceptionAsync<UserNotFoundException>(() => sut.GetDashboardYear(It.IsAny<string>()));
        }

        [TestMethod]
        public async Task GetDashboardYear_NotNullUser_ReturnUserYears()
        {
            var expected = new YearsDto { DashboardSelectedYear = 2024, RegisteredYear = 2022 };

            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User { RegisteredDate = new DateTime(2022, 5, 2), DashboardSelectedYear = 2024 });

            var result = await sut.GetDashboardYear(It.IsAny<string>());

            Assert.AreEqual(expected.DashboardSelectedYear, result.DashboardSelectedYear);
            Assert.AreEqual(expected.RegisteredYear, result.RegisteredYear);
        }

        [TestMethod]
        public async Task ChangeDashboardYear_NullUser_ThrowException()
        {
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(value: null);

            Assert.ThrowsExceptionAsync<UserNotFoundException>(() => sut.ChangeDashboardYear(It.IsAny<string>(), It.IsAny<int>()));
        }

        [TestMethod]
        public async Task ChangeDashboardYear_NotNullUser_ChangeDashboardYearIsCalled()
        {
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User());

            await sut.ChangeDashboardYear(It.IsAny<string>(), It.IsAny<int>());

            mockUserRepository.Verify(r => r.ChangeDashboardYear(It.IsAny<User>() , It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task GetUserCurrencyPreference_NullUser_ThrowException()
        {
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(value: null);

            Assert.ThrowsExceptionAsync<UserNotFoundException>(() => sut.GetUserCurrencyPreference(It.IsAny<string>()));
        }

        [TestMethod]
        public async Task GetUserCurrencyPreference_NotNullUser_ReturnUserCurrencyPreference()
        {
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User { CurrencyPreference = CurrencyPreference.RON});

            var result = await sut.GetUserCurrencyPreference(It.IsAny<string>());

            Assert.AreEqual("RON", result);
        }

        [TestMethod]
        public async Task ChangeUserCurrencyPreference_NullUser_ThrowException()
        {
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(value: null);

            Assert.ThrowsExceptionAsync<UserNotFoundException>(() => sut.ChangeUserCurrencyPreference(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public async Task ChangeUserCurrencyPreference_NotNullUser_ChangeDashboardYearIsCalled()
        {
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User());

            await sut.ChangeUserCurrencyPreference(It.IsAny<string>(), "RON");

            mockUserRepository.Verify(r => r.ChangeCurrencyPreference(It.IsAny<User>(), It.IsAny<CurrencyPreference>()), Times.Once);
        }

        [TestMethod]
        public async Task ChangeUserCurrencyPreference_InvalidCurrency_ThrowException()
        {
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(new User());

            Assert.ThrowsExceptionAsync<InvalidCastException>(() => sut.ChangeUserCurrencyPreference(It.IsAny<string>(), "DKK"));
            mockUserRepository.Verify(r => r.ChangeCurrencyPreference(It.IsAny<User>(), It.IsAny<CurrencyPreference>()), Times.Never);

        }
    }
}
