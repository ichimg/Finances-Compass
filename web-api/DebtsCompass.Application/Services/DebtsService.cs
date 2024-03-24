using DebtsCompass.Domain;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain.Entities.Dtos;
using DebtsCompass.Domain.Entities.EmailDtos;
using EmailSender;
using DebtsCompass.Domain.Enums;
using Hangfire;

namespace DebtsCompass.Application.Services
{
    public class DebtsService : IDebtsService
    {
        private readonly IDebtAssignmentRepository debtAssignmentRepository;
        private readonly IDebtRepository debtRepository;
        private readonly IUserRepository userRepository;
        private readonly INonUserRepository nonUserRepository;
        private readonly IEmailService emailService;
        private readonly ICurrencyRatesJob currencyRatesJob;
        private readonly IExpenseCategoryRepository expenseCategoryRepository;
        private readonly IIncomeCategoryRepository incomeCategoryRepository;
        private readonly IExpensesService expensesService;
        private readonly IIncomesService incomesService;

        private readonly IBackgroundJobClient backgroundJobClient;

        public DebtsService(IDebtAssignmentRepository debtAssignmentRepository,
            IUserRepository userRepository,
            INonUserRepository nonUserRepository,
            IEmailService emailService,
            ICurrencyRatesJob currencyRatesJob,
            IDebtRepository debtRepository,
            IExpenseCategoryRepository expenseCategoryRepository,
            IIncomeCategoryRepository incomeCategoryRepository,
            IExpensesService expensesService,
            IIncomesService incomesService,
            IBackgroundJobClient backgroundJobClient)
        {
            this.debtAssignmentRepository = debtAssignmentRepository;
            this.userRepository = userRepository;
            this.nonUserRepository = nonUserRepository;
            this.emailService = emailService;
            this.currencyRatesJob = currencyRatesJob;
            this.debtRepository = debtRepository;
            this.expenseCategoryRepository = expenseCategoryRepository;
            this.incomeCategoryRepository = incomeCategoryRepository;
            this.expensesService = expensesService;
            this.incomesService = incomesService;
            this.backgroundJobClient = backgroundJobClient;
        }

        public async Task<List<DebtDto>> GetAllReceivingDebts(string email)
        {
            var debtsFromDb = await debtAssignmentRepository.GetAllReceivingDebtsByEmail(email);

            User user = await userRepository.GetUserByEmail(email);

            if (user.CurrencyPreference == CurrencyPreference.EUR)
            {
                debtsFromDb.ForEach(d => d.Debt.Amount = (decimal)(d.Debt.Amount * d.Debt.EurExchangeRate));
            }
            else if (user.CurrencyPreference == CurrencyPreference.USD)
            {
                debtsFromDb.ForEach(d => d.Debt.Amount = (decimal)(d.Debt.Amount * d.Debt.UsdExchangeRate));
            }

            List<DebtDto> debts = debtsFromDb
                    .Select(Mapper.ReceivingDebtAssignmentDbModelToDebtDto)
                    .ToList();

            return debts;
        }

        public async Task<List<DebtDto>> GetAllUserDebts(string email)
        {
            var debtsFromDb = await debtAssignmentRepository.GetAllUserDebtsByEmail(email);

            User user = await userRepository.GetUserByEmail(email);

            if (user.CurrencyPreference == CurrencyPreference.EUR)
            {
                debtsFromDb.ForEach(d => d.Debt.Amount *= (decimal)d.Debt.EurExchangeRate);
            }
            else if (user.CurrencyPreference == CurrencyPreference.USD)
            {
                debtsFromDb.ForEach(d => d.Debt.Amount *= (decimal)d.Debt.UsdExchangeRate);
            }

            List<DebtDto> debts = debtsFromDb.Select(Mapper.UserDebtAssignmentDbModelToDebtDto).ToList();

            return debts;
        }

        public async Task<Guid> CreateDebt(CreateDebtRequest createDebtRequest, string creatorEmail)
        {
            User creatorUser = await userRepository.GetUserByEmail(creatorEmail) ?? throw new UserNotFoundException(creatorEmail);
            User existingAccount = await userRepository.GetUserByEmail(createDebtRequest.Email);

            bool isUserAccount = existingAccount is not null;

            CurrencyDto currentCurrencies = await currencyRatesJob.GetLatestCurrencyRates();

            if (creatorUser.CurrencyPreference == CurrencyPreference.EUR)
            {
                createDebtRequest.Amount /= currentCurrencies.EurExchangeRate;
            }
            else if (creatorUser.CurrencyPreference == CurrencyPreference.USD)
            {
                createDebtRequest.Amount /= currentCurrencies.UsdExchangeRate;
            }

            DebtAssignment debtAssignment;
            if (isUserAccount)
            {
                User selectedUser = existingAccount;

                debtAssignment = Mapper.CreateDebtRequestToDebtAssignment(createDebtRequest, creatorUser, selectedUser, currentCurrencies);
                await debtAssignmentRepository.CreateDebt(debtAssignment);

                ReceiverInfoDto receiverInfoDto = Mapper.UserToReceiverInfoDto(debtAssignment.SelectedUser);
                DebtEmailInfoDto createdDebtEmailInfoDto = Mapper.DebtAssignmentToCreatedDebtEmailInfoDto(debtAssignment);
                await emailService.SendDebtCreatedNotification(receiverInfoDto, createdDebtEmailInfoDto);
                backgroundJobClient.Schedule(() => Console.WriteLine($"[HANGFIRE] Debt created at {debtAssignment.Debt.DeadlineDate}"), 
                    TimeSpan.FromMinutes(1));
            }
            else
            {
                NonUser existingNonUser = await nonUserRepository.GetNonUserByEmail(createDebtRequest.Email);

                if (existingNonUser is not null)
                {
                    debtAssignment = Mapper.CreateDebtRequestToDebtAssignment(createDebtRequest, creatorUser, existingNonUser, currentCurrencies);
                }
                else
                {
                    debtAssignment = Mapper.CreateDebtRequestToDebtAssignment(createDebtRequest, creatorUser, currentCurrencies);
                }
                await debtAssignmentRepository.CreateDebt(debtAssignment);


                ReceiverInfoDto receiverInfoDto = Mapper.NonUserToReceiverInfoDto(debtAssignment.NonUser);
                DebtEmailInfoDto createdDebtEmailInfoDto = Mapper.UserToCreatedDebtEmailInfoDto(creatorUser);
                await emailService.SendNoAccountDebtCreatedNotification(receiverInfoDto, createdDebtEmailInfoDto);
            }
            return debtAssignment.Id;
        }

        public async Task DeleteDebt(string id, string email)
        {
            var debtFromDb = await debtAssignmentRepository.GetDebtById(id);

            if (debtFromDb is null)
            {
                throw new EntityNotFoundException();
            }

            if (!debtFromDb.CreatorUser.Email.Equals(email))
            {
                throw new ForbiddenRequestException();
            }

            if (debtFromDb.SelectedUser is not null)
            {
                ReceiverInfoDto receiverInfoDto = Mapper.UserToReceiverInfoDto(debtFromDb.SelectedUser);
                DebtEmailInfoDto deletedDebtEmailInfoDto = Mapper.DebtAssignmentToCreatedDebtEmailInfoDto(debtFromDb);

                await debtRepository.DeleteDebt(debtFromDb.Debt);
                await emailService.SendDebtDeletedNotification(receiverInfoDto, deletedDebtEmailInfoDto);
            }
            else if (debtFromDb.NonUser is not null)
            {
                ReceiverInfoDto receiverInfoDto = Mapper.NonUserToReceiverInfoDto(debtFromDb.NonUser);
                DebtEmailInfoDto deletedDebtEmailInfoDto = Mapper.DebtAssignmentToCreatedDebtEmailInfoDto(debtFromDb);

                await debtRepository.DeleteDebt(debtFromDb.Debt);
                await emailService.SendDebtDeletedNotification(receiverInfoDto, deletedDebtEmailInfoDto);
            }
        }

        public async Task EditDebt(EditDebtRequest editDebtRequest, string email)
        {
            DebtAssignment debtAssignmentFromDb = await debtAssignmentRepository.GetDebtById(editDebtRequest.Guid) ?? throw new EntityNotFoundException();
            User user = await userRepository.GetUserByEmail(email) ?? throw new UserNotFoundException(email);

            CurrencyDto currentCurrencies = await currencyRatesJob.GetLatestCurrencyRates();

            if (user.CurrencyPreference == CurrencyPreference.EUR)
            {
                editDebtRequest.Amount /= currentCurrencies.EurExchangeRate;
            }
            else if (user.CurrencyPreference == CurrencyPreference.USD)
            {
                editDebtRequest.Amount /= currentCurrencies.UsdExchangeRate;
            }

            bool isUserAccount = editDebtRequest.IsUserAccount;
            DebtAssignment updatedDebt;
            if (isUserAccount)
            {
                User selectedUser = await userRepository.GetUserByEmail(editDebtRequest.Email);
                updatedDebt = Mapper.EditDebtRequestToDebtAssignment(editDebtRequest, selectedUser);
            }
            else
            {
                NonUser nonUser = await nonUserRepository.GetNonUserByEmail(editDebtRequest.Email);
                updatedDebt = Mapper.EditDebtRequestToDebtAssignment(editDebtRequest, nonUser);
            }

            await debtAssignmentRepository.UpdateDebt(debtAssignmentFromDb, updatedDebt);
        }

        public async Task ApproveDebt(string debtId, string email)
        {
            DebtAssignment debtAssignmentFromDb = await debtAssignmentRepository.GetDebtById(debtId) ?? throw new EntityNotFoundException();
            await debtAssignmentRepository.ApproveDebt(debtAssignmentFromDb);

            await CreateExpenseRelatedToDebt(debtAssignmentFromDb, debtAssignmentFromDb.Debt.DateOfBorrowing, debtAssignmentFromDb.CreatorUser,
                $"Loaned to {debtAssignmentFromDb.SelectedUser.UserInfo.FirstName} {debtAssignmentFromDb.SelectedUser.UserInfo.LastName}");
            await CreateIncomeRelatedToDebt(debtAssignmentFromDb, debtAssignmentFromDb.Debt.DateOfBorrowing, debtAssignmentFromDb.SelectedUser,
                $"Loaned from {debtAssignmentFromDb.CreatorUser.UserInfo.FirstName} {debtAssignmentFromDb.CreatorUser.UserInfo.LastName}.");
            // TODO: send e-mail notification
        }

        public async Task RejectDebt(string debtId, string email)
        {
            DebtAssignment debtAssignmentFromDb = await debtAssignmentRepository.GetDebtById(debtId) ?? throw new EntityNotFoundException();
            await debtAssignmentRepository.RejectDebt(debtAssignmentFromDb);
            // TODO: send e-mail notification
        }

        public async Task MarkPaid(string debtId)
        {
            DebtAssignment debtAssignmentFromDb = await debtAssignmentRepository.GetDebtById(debtId) ?? throw new EntityNotFoundException();
            await debtAssignmentRepository.PayDebt(debtAssignmentFromDb);

            await CreateExpenseRelatedToDebt(debtAssignmentFromDb, DateTime.UtcNow, debtAssignmentFromDb.SelectedUser,
                $"Debt paid to {debtAssignmentFromDb.CreatorUser.UserInfo.FirstName} {debtAssignmentFromDb.CreatorUser.UserInfo.LastName}");
            await CreateIncomeRelatedToDebt(debtAssignmentFromDb, DateTime.UtcNow, debtAssignmentFromDb.CreatorUser,
                $"Debt collected from {debtAssignmentFromDb.SelectedUser.UserInfo.FirstName} {debtAssignmentFromDb.SelectedUser.UserInfo.LastName}.");

            // TODO: send e-mail notification
        }

        private async Task CreateExpenseRelatedToDebt(DebtAssignment debtAssignment, DateTime date, User user, string message)
        {
            ExpenseCategory category = await expenseCategoryRepository.GetByName("Debts");
            CreateExpenseRequest createExpenseRequest = new CreateExpenseRequest
            {
                Amount = debtAssignment.Debt.Amount,
                Date = date.ToString(),
                Category = category.Name,
                Note = message
            };

            await expensesService.CreateExpense(createExpenseRequest, user.Email, true);
        }

        private async Task CreateIncomeRelatedToDebt(DebtAssignment debtAssignment, DateTime date, User user, string message)
        {
            IncomeCategory category = await incomeCategoryRepository.GetByName("Debts");
            CreateIncomeRequest createIncomeRequest = new CreateIncomeRequest
            {
                Amount = debtAssignment.Debt.Amount,
                Date = date.ToString(),
                Category = category.Name,
                Note = message
            };

            await incomesService.CreateIncome(createIncomeRequest, user.Email, true);
        }
    }
}
