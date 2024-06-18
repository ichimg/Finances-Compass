using DebtsCompass.Application.Exceptions;
using DebtsCompass.Application.Services;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Dtos;
using DebtsCompass.Domain.Entities.EmailDtos;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Enums;
using DebtsCompass.Domain.Interfaces;
using Moq;

namespace DebtsCompass.Tests.Services
{
    [TestClass]
    public class DebtsServiceTests
    {
        private Mock<IDebtAssignmentRepository> mockDebtAssignmentRepository;
        private Mock<IDebtRepository> mockDebtRepository;
        private Mock<IUserRepository> mockUserRepository;
        private Mock<INonUserRepository> mockNonUserRepository;
        private Mock<IEmailService> mockEmailService;
        private Mock<ICurrencyRateRepository> mockCurrencyRateRepository;
        private Mock<IExpenseCategoryRepository> mockExpenseCategoryRepository;
        private Mock<IIncomeCategoryRepository> mockIncomeCategoryRepository;
        private Mock<IExpensesService> mockExpensesService;
        private Mock<IIncomesService> mockIncomesService;
        private Mock<IHangfireService> mockHangfireService;

        private DebtsService sut;

        [TestInitialize]
        public void Initialize()
        {
            mockDebtAssignmentRepository = new Mock<IDebtAssignmentRepository>();
            mockDebtRepository = new Mock<IDebtRepository>();
            mockUserRepository = new Mock<IUserRepository>();
            mockNonUserRepository = new Mock<INonUserRepository>();
            mockEmailService = new Mock<IEmailService>();
            mockCurrencyRateRepository = new Mock<ICurrencyRateRepository>();
            mockExpenseCategoryRepository = new Mock<IExpenseCategoryRepository>();
            mockIncomeCategoryRepository = new Mock<IIncomeCategoryRepository>();
            mockExpensesService = new Mock<IExpensesService>();
            mockIncomesService = new Mock<IIncomesService>();
            mockHangfireService = new Mock<IHangfireService>();

            sut = new DebtsService(mockDebtAssignmentRepository.Object, mockUserRepository.Object,
                                   mockNonUserRepository.Object, mockEmailService.Object, mockCurrencyRateRepository.Object,
                                   mockDebtRepository.Object, mockExpenseCategoryRepository.Object, mockIncomeCategoryRepository.Object,
                                   mockExpensesService.Object, mockIncomesService.Object, mockHangfireService.Object);
        }

        [TestMethod]
        public async Task GetAllReceivingDebts_CurrencyPreferenceRON_ReturnRONAmounts()
        {
            var expectedDebts = new List<DebtAssignment>
            {
                new DebtAssignment { Debt = new Debt { Amount = 500 } },
                new DebtAssignment { Debt = new Debt { Amount = 300 } }
            };

            var expectedSum = expectedDebts.Sum(d => d.Debt.Amount);

            mockDebtAssignmentRepository.Setup(r => r.GetAllReceivingDebtsByEmail(It.IsAny<string>())).ReturnsAsync(expectedDebts);
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(ReturnsUser(CurrencyPreference.RON));

            var result = await sut.GetAllReceivingDebts(It.IsAny<string>());
            var resultSum = result.Sum(d => d.Amount);

            Assert.AreEqual(expectedSum, resultSum);
        }

        [TestMethod]
        public async Task GetAllReceivingDebts_CurrencyPreferenceEUR_ReturnEURAmounts()
        {
            var expectedDebts = new List<DebtAssignment>
            {
                new DebtAssignment { Debt = new Debt { Amount = 500, CurrencyRate = new CurrencyRate{ EurExchangeRate = 0.20m } } },
                new DebtAssignment { Debt = new Debt { Amount = 300, CurrencyRate = new CurrencyRate{ EurExchangeRate = 0.25m } } }
            };

            var expectedSum = expectedDebts.Sum(d => d.Debt.Amount * d.Debt.CurrencyRate.EurExchangeRate);

            mockDebtAssignmentRepository.Setup(r => r.GetAllReceivingDebtsByEmail(It.IsAny<string>())).ReturnsAsync(expectedDebts);
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(ReturnsUser(CurrencyPreference.EUR));

            var result = await sut.GetAllReceivingDebts(It.IsAny<string>());
            var resultSum = result.Sum(d => d.Amount);

            Assert.AreEqual(expectedSum, resultSum);
        }

        [TestMethod]
        public async Task GetAllReceivingDebts_CurrencyPreferenceUSD_ReturnUSDAmounts()
        {
            var expectedDebts = new List<DebtAssignment>
            {
                new DebtAssignment { Debt = new Debt { Amount = 500, CurrencyRate = new CurrencyRate{ UsdExchangeRate = 0.218m } } },
                new DebtAssignment { Debt = new Debt { Amount = 300, CurrencyRate = new CurrencyRate{ UsdExchangeRate = 0.208m } } }
            };

            var expectedSum = expectedDebts.Sum(d => d.Debt.Amount * d.Debt.CurrencyRate.UsdExchangeRate);

            mockDebtAssignmentRepository.Setup(r => r.GetAllReceivingDebtsByEmail(It.IsAny<string>())).ReturnsAsync(expectedDebts);
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(ReturnsUser(CurrencyPreference.USD));

            var result = await sut.GetAllReceivingDebts(It.IsAny<string>());
            var resultSum = result.Sum(d => d.Amount);

            Assert.AreEqual(expectedSum, resultSum);
        }

        [TestMethod]
        public async Task GetAllUserDebts_CurrencyPreferenceRON_ReturnRONAmounts()
        {
            var expectedDebts = new List<DebtAssignment>
            {
                new DebtAssignment { Debt = new Debt { Amount = 500 } },
                new DebtAssignment { Debt = new Debt { Amount = 300 } }
            };

            var expectedSum = expectedDebts.Sum(d => d.Debt.Amount);

            mockDebtAssignmentRepository.Setup(r => r.GetAllUserDebtsByEmail(It.IsAny<string>())).ReturnsAsync(expectedDebts);
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(ReturnsUser(CurrencyPreference.RON));

            var result = await sut.GetAllUserDebts(It.IsAny<string>());
            var resultSum = result.Sum(d => d.Amount);

            Assert.AreEqual(expectedSum, resultSum);
        }

        [TestMethod]
        public async Task GetAllUserDebts_CurrencyPreferenceEUR_ReturnEURAmounts()
        {
            var expectedDebts = new List<DebtAssignment>
            {
                new DebtAssignment { Debt = new Debt { Amount = 500, CurrencyRate = new CurrencyRate{ EurExchangeRate = 0.20m } } },
                new DebtAssignment { Debt = new Debt { Amount = 300, CurrencyRate = new CurrencyRate{ EurExchangeRate = 0.25m } } }
            };

            var expectedSum = expectedDebts.Sum(d => d.Debt.Amount * d.Debt.CurrencyRate.EurExchangeRate);

            mockDebtAssignmentRepository.Setup(r => r.GetAllUserDebtsByEmail(It.IsAny<string>())).ReturnsAsync(expectedDebts);
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(ReturnsUser(CurrencyPreference.EUR));

            var result = await sut.GetAllUserDebts(It.IsAny<string>());
            var resultSum = result.Sum(d => d.Amount);

            Assert.AreEqual(expectedSum, resultSum);
        }

        [TestMethod]
        public async Task GetAllUserDebts_CurrencyPreferenceUSD_ReturnUSDAmounts()
        {
            var expectedDebts = new List<DebtAssignment>
            {
                new DebtAssignment { Debt = new Debt { Amount = 500, CurrencyRate = new CurrencyRate{ UsdExchangeRate = 0.218m } } },
                new DebtAssignment { Debt = new Debt { Amount = 300, CurrencyRate = new CurrencyRate{ UsdExchangeRate = 0.208m } } }
            };

            var expectedSum = expectedDebts.Sum(d => d.Debt.Amount * d.Debt.CurrencyRate.UsdExchangeRate);

            mockDebtAssignmentRepository.Setup(r => r.GetAllUserDebtsByEmail(It.IsAny<string>())).ReturnsAsync(expectedDebts);
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(ReturnsUser(CurrencyPreference.USD));

            var result = await sut.GetAllUserDebts(It.IsAny<string>());
            var resultSum = result.Sum(d => d.Amount);

            Assert.AreEqual(expectedSum, resultSum);
        }

        [TestMethod]
        public async Task CreateDebt_NullUser_ThrowException()
        {
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(value: null);

            Assert.ThrowsExceptionAsync<UserNotFoundException>(() => sut.CreateDebt(It.IsAny<CreateDebtRequest>(), It.IsAny<string>()));
        }

        [TestMethod]
        public async Task CreateDebt_NotNullUser_ReturnExpectedCreatedDebtId()
        {
            var expectedId = Guid.NewGuid();
            mockUserRepository.SetupSequence(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(value: ReturnsUser(CurrencyPreference.RON))
                                                                                       .ReturnsAsync(value: ReturnsUser(CurrencyPreference.RON));
            mockCurrencyRateRepository.Setup(r => r.GetLatestInsertedCurrencyRates()).ReturnsAsync(new CurrencyRate());
            mockDebtAssignmentRepository.Setup(repo => repo.CreateDebt(It.IsAny<DebtAssignment>()))
                             .Callback<DebtAssignment>(entity => entity.Id = expectedId);

            var result = await sut.CreateDebt(InstantiateCreateDebtRequest(), It.IsAny<string>());

            Assert.AreEqual(expectedId, result);
        }

        [TestMethod]
        public async Task CreateDebt_IsUserAccount_SendDebtCreatedNotificationIsCalled()
        {
            mockUserRepository.SetupSequence(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(value: ReturnsUser(CurrencyPreference.RON))
                                                                                       .ReturnsAsync(value: ReturnsUser(CurrencyPreference.RON));
            mockCurrencyRateRepository.Setup(r => r.GetLatestInsertedCurrencyRates()).ReturnsAsync(new CurrencyRate());
            mockDebtAssignmentRepository.Setup(repo => repo.CreateDebt(It.IsAny<DebtAssignment>()));

            await sut.CreateDebt(InstantiateCreateDebtRequest(), It.IsAny<string>());

            mockEmailService.Verify(s => s.SendDebtCreatedNotification(It.IsAny<ReceiverInfoDto>(), It.IsAny<DebtEmailInfoDto>()), Times.Exactly(1));
        }

        [TestMethod]
        public async Task CreateDebt_IsUserAccount_ScheduleDeadlineRemindersIsCalled()
        {
            mockUserRepository.SetupSequence(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(value: ReturnsUser(CurrencyPreference.RON))
                                                                                       .ReturnsAsync(value: ReturnsUser(CurrencyPreference.RON));
            mockCurrencyRateRepository.Setup(r => r.GetLatestInsertedCurrencyRates()).ReturnsAsync(new CurrencyRate());
            mockDebtAssignmentRepository.Setup(repo => repo.CreateDebt(It.IsAny<DebtAssignment>()));

            await sut.CreateDebt(InstantiateCreateDebtRequest(), It.IsAny<string>());

            mockHangfireService.Verify(s => s.ScheduleDeadlineEmails(It.IsAny<DebtAssignment>(), It.IsAny<ReceiverInfoDto>()), Times.Exactly(1));
        }

        [TestMethod]
        public async Task CreateDebt_IsNotUserAccount_SendNoAccountDebtCreatedNotificationIsCalled()
        {
            mockUserRepository.SetupSequence(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(value: ReturnsUser(CurrencyPreference.RON))
                                                                                       .ReturnsAsync(value: null);

            mockNonUserRepository.Setup(s => s.GetNonUserByEmail(It.IsAny<string>())).ReturnsAsync(value: new NonUser());
            mockCurrencyRateRepository.Setup(r => r.GetLatestInsertedCurrencyRates()).ReturnsAsync(new CurrencyRate());
            mockDebtAssignmentRepository.Setup(r => r.CreateDebt(It.IsAny<DebtAssignment>()));

            await sut.CreateDebt(InstantiateCreateDebtRequest(), It.IsAny<string>());

            mockEmailService.Verify(s => s.SendNoAccountDebtCreatedNotification(It.IsAny<ReceiverInfoDto>(), It.IsAny<DebtEmailInfoDto>()), Times.Exactly(1));
        }

        [TestMethod]
        public async Task DeleteDebt_IsNotCreatorUser_ThrowException()
        {
            string notCreatorUserEmail = "differentUser";
            mockDebtAssignmentRepository.Setup(r => r.GetDebtById(It.IsAny<string>())).ReturnsAsync(InstantiateDebtAssignment());

            await Assert.ThrowsExceptionAsync<ForbiddenRequestException>(() => sut.DeleteDebt(It.IsAny<string>(), notCreatorUserEmail));
        }

        [TestMethod]
        public async Task DeleteDebt_IsUserAccount_DeleteScheduledJobIsCalled()
        {
            var debtAssignment = InstantiateDebtAssignment(true);
            mockDebtAssignmentRepository.Setup(r => r.GetDebtById(It.IsAny<string>())).ReturnsAsync(debtAssignment);

            await sut.DeleteDebt(It.IsAny<string>(), debtAssignment.CreatorUser.Email);

            mockHangfireService.Verify(s => s.DeleteScheduledJob(It.IsAny<string>()), Times.Exactly(1));
            mockEmailService.Verify(s => s.SendDebtDeletedNotification(It.IsAny<ReceiverInfoDto>(), It.IsAny<DebtEmailInfoDto>()), Times.Once());
        }

        [TestMethod]
        public async Task DeleteDebt_IsNotUserAccount_DeleteScheduledJobIsNotCalled()
        {
            var debtAssignment = InstantiateDebtAssignment();
            mockDebtAssignmentRepository.Setup(r => r.GetDebtById(It.IsAny<string>())).ReturnsAsync(debtAssignment);

            await sut.DeleteDebt(It.IsAny<string>(), debtAssignment.CreatorUser.Email);

            mockHangfireService.Verify(s => s.DeleteScheduledJob(It.IsAny<string>()), Times.Never());
            mockEmailService.Verify(s => s.SendDebtDeletedNotification(It.IsAny<ReceiverInfoDto>(), It.IsAny<DebtEmailInfoDto>()), Times.Once());
        }

        [TestMethod]
        public async Task ApproveDebt_DebtNotFound_ThrowException()
        {
            mockDebtAssignmentRepository.Setup(r => r.GetDebtById(It.IsAny<string>())).ReturnsAsync(value: null);

            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(() => sut.ApproveDebt(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public async Task RejectDebt_DebtFound_DeleteScheduledJobIsCalled()
        {
            var debtAssignment = InstantiateDebtAssignment(true);
            mockDebtAssignmentRepository.Setup(r => r.GetDebtById(It.IsAny<string>())).ReturnsAsync(debtAssignment);

            await sut.RejectDebt(It.IsAny<string>(), debtAssignment.CreatorUser.Email);

            mockHangfireService.Verify(s => s.DeleteScheduledJob(It.IsAny<string>()), Times.Exactly(1));
        }

        [TestMethod]
        public async Task RejectDebt_DebtNotFound_ThrowException()
        {
            mockDebtAssignmentRepository.Setup(r => r.GetDebtById(It.IsAny<string>())).ReturnsAsync(value: null);

            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(() => sut.RejectDebt(It.IsAny<string>(), It.IsAny<string>()));
        }
        
        [TestMethod]
        public async Task MarkPaid_DebtFound_DeleteScheduledJobIsCalled()
        {
            var debtAssignment = InstantiateDebtAssignment(true);
            mockDebtAssignmentRepository.Setup(r => r.GetDebtById(It.IsAny<string>())).ReturnsAsync(debtAssignment);

            await sut.MarkPaid(It.IsAny<string>());

            mockHangfireService.Verify(s => s.DeleteScheduledJob(It.IsAny<string>()), Times.Exactly(1));
        }

        [TestMethod]
        public async Task MarkPaid_DebtNotFound_ThrowException()
        {
            mockDebtAssignmentRepository.Setup(r => r.GetDebtById(It.IsAny<string>())).ReturnsAsync(value: null);

            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(() => sut.MarkPaid(It.IsAny<string>()));
        }

        [TestMethod]
        public async Task GetLoansAndDebtsTotalCount_CurrencyPreferenceRON_ReturnRONAmounts()
        {
            var expectedDebts = new List<DebtAssignment>
            {
                new DebtAssignment { Debt = new Debt { Amount = 500 } },
                new DebtAssignment { Debt = new Debt { Amount = 300 } }
            };

            var expectedLoans = new List<DebtAssignment>
            {
                new DebtAssignment { Debt = new Debt { Amount = 210, CurrencyRate = new CurrencyRate{ EurExchangeRate = 0.20m } } },
                new DebtAssignment { Debt = new Debt { Amount = 155, CurrencyRate = new CurrencyRate{ EurExchangeRate = 0.25m } } }
            };

            var expectedDebtsSum = expectedDebts.Sum(d => d.Debt.Amount);
            var expectedLoansSum = expectedLoans.Sum(d => d.Debt.Amount);


            mockDebtAssignmentRepository.Setup(r => r.GetAllUserDebtsByEmail(It.IsAny<string>())).ReturnsAsync(expectedDebts);
            mockDebtAssignmentRepository.Setup(r => r.GetAllReceivingDebtsByEmail(It.IsAny<string>())).ReturnsAsync(expectedLoans);
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(ReturnsUser(CurrencyPreference.RON));

            var result = await sut.GetLoansAndDebtsTotalCount(It.IsAny<string>());

            Assert.AreEqual(expectedDebtsSum, result.TotalDebts);
            Assert.AreEqual(expectedLoansSum, result.TotalLoans);
        }

        [TestMethod]
        public async Task GetLoansAndDebtsTotalCount_CurrencyPreferenceEUR_ReturnEURAmounts()
        {
            var expectedDebts = new List<DebtAssignment>
            {
                new DebtAssignment { Debt = new Debt { Amount = 500, CurrencyRate = new CurrencyRate{ EurExchangeRate = 0.20m } } },
                new DebtAssignment { Debt = new Debt { Amount = 300, CurrencyRate = new CurrencyRate{ EurExchangeRate = 0.25m } } }
            };

            var expectedLoans = new List<DebtAssignment>
            {
                new DebtAssignment { Debt = new Debt { Amount = 500, CurrencyRate = new CurrencyRate{ EurExchangeRate = 0.20m } } },
                new DebtAssignment { Debt = new Debt { Amount = 300, CurrencyRate = new CurrencyRate{ EurExchangeRate = 0.25m } } }
            };

            var expectedDebtsSum = expectedDebts.Sum(d => d.Debt.Amount * d.Debt.CurrencyRate.EurExchangeRate);
            var expectedLoansSum = expectedLoans.Sum(d => d.Debt.Amount * d.Debt.CurrencyRate.EurExchangeRate);

            mockDebtAssignmentRepository.Setup(r => r.GetAllUserDebtsByEmail(It.IsAny<string>())).ReturnsAsync(expectedDebts);
            mockDebtAssignmentRepository.Setup(r => r.GetAllReceivingDebtsByEmail(It.IsAny<string>())).ReturnsAsync(expectedLoans);
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(ReturnsUser(CurrencyPreference.EUR));

            var result = await sut.GetLoansAndDebtsTotalCount(It.IsAny<string>());

            Assert.AreEqual(expectedDebtsSum, result.TotalDebts);
            Assert.AreEqual(expectedLoansSum, result.TotalLoans);
        }

        [TestMethod]
        public async Task GetLoansAndDebtsTotalCount_CurrencyPreferenceUSD_ReturnUSDAmounts()
        {
            var expectedDebts = new List<DebtAssignment>
            {
                new DebtAssignment { Debt = new Debt { Amount = 500, CurrencyRate = new CurrencyRate{ UsdExchangeRate = 0.218m } } },
                new DebtAssignment { Debt = new Debt { Amount = 300, CurrencyRate = new CurrencyRate{ UsdExchangeRate = 0.208m } } }
            };

            var expectedLoans = new List<DebtAssignment>
            {
                new DebtAssignment { Debt = new Debt { Amount = 500, CurrencyRate = new CurrencyRate{ UsdExchangeRate = 0.218m } } },
                new DebtAssignment { Debt = new Debt { Amount = 300, CurrencyRate = new CurrencyRate{ UsdExchangeRate = 0.208m } } }
            };

            var expectedDebtsSum = expectedDebts.Sum(d => d.Debt.Amount * d.Debt.CurrencyRate.UsdExchangeRate);
            var expectedLoansSum = expectedLoans.Sum(d => d.Debt.Amount * d.Debt.CurrencyRate.UsdExchangeRate);

            mockDebtAssignmentRepository.Setup(r => r.GetAllUserDebtsByEmail(It.IsAny<string>())).ReturnsAsync(expectedDebts);
            mockDebtAssignmentRepository.Setup(r => r.GetAllReceivingDebtsByEmail(It.IsAny<string>())).ReturnsAsync(expectedLoans);
            mockUserRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(ReturnsUser(CurrencyPreference.USD));

            var result = await sut.GetLoansAndDebtsTotalCount(It.IsAny<string>());

            Assert.AreEqual(expectedDebtsSum, result.TotalDebts);
            Assert.AreEqual(expectedLoansSum, result.TotalLoans);
        }

        private User ReturnsUser(CurrencyPreference currencyPreference)
        {
            return new User { CurrencyPreference = currencyPreference, Email = "test", DashboardSelectedYear = 1 };
        }

        private CreateDebtRequest InstantiateCreateDebtRequest()
        {
            return new CreateDebtRequest
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
                Amount = 50m,
                BorrowingDate = DateTime.Today.ToString(),
                Deadline = DateTime.Today.ToString(),
                Reason = "Test",
                Status = "Pending"
            };
        }

        private DebtAssignment InstantiateDebtAssignment(bool isSelectedUser = false)
        {
            return new DebtAssignment
            {
                CreatorUser = ReturnsUser(CurrencyPreference.RON),
                SelectedUser = isSelectedUser ? ReturnsUser(CurrencyPreference.RON) : null,
                NonUser = !isSelectedUser ? new NonUser() : null,
                Debt = InstantiateDebt()
            }; 

        }

        private Debt InstantiateDebt()
        {
            return new Debt
            {
                Amount = 50m
            };
        }
    }
}
