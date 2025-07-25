﻿using DebtsCompass.Domain;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain.Entities.Dtos;
using DebtsCompass.Domain.Entities.EmailDtos;
using DebtsCompass.Domain.Enums;

namespace DebtsCompass.Application.Services
{
    public class DebtsService : IDebtsService
    {
        private readonly IDebtAssignmentRepository debtAssignmentRepository;
        private readonly IDebtRepository debtRepository;
        private readonly IUserRepository userRepository;
        private readonly INonUserRepository nonUserRepository;
        private readonly IEmailService emailService;
        private readonly ICurrencyRateRepository currencyRateRepository;
        private readonly IExpenseCategoryRepository expenseCategoryRepository;
        private readonly IIncomeCategoryRepository incomeCategoryRepository;
        private readonly IExpensesService expensesService;
        private readonly IIncomesService incomesService;
        private readonly IHangfireService hangfireService;

        public DebtsService(IDebtAssignmentRepository debtAssignmentRepository,
            IUserRepository userRepository,
            INonUserRepository nonUserRepository,
            IEmailService emailService,
            ICurrencyRateRepository currencyRateRepository,
            IDebtRepository debtRepository,
            IExpenseCategoryRepository expenseCategoryRepository,
            IIncomeCategoryRepository incomeCategoryRepository,
            IExpensesService expensesService,
            IIncomesService incomesService,
            IHangfireService hangfireService)
        {
            this.debtAssignmentRepository = debtAssignmentRepository;
            this.userRepository = userRepository;
            this.nonUserRepository = nonUserRepository;
            this.emailService = emailService;
            this.currencyRateRepository = currencyRateRepository;
            this.debtRepository = debtRepository;
            this.expenseCategoryRepository = expenseCategoryRepository;
            this.incomeCategoryRepository = incomeCategoryRepository;
            this.expensesService = expensesService;
            this.incomesService = incomesService;
            this.hangfireService = hangfireService;
        }

        public async Task<List<DebtDto>> GetAllReceivingDebts(string email)
        {
            var debtsFromDb = await debtAssignmentRepository.GetAllReceivingDebtsByEmail(email);

            User user = await userRepository.GetUserByEmail(email);

            if (user.CurrencyPreference == CurrencyPreference.EUR)
            {
                debtsFromDb.ForEach(d => d.Debt.Amount = (decimal)(d.Debt.Amount * d.Debt.CurrencyRate.EurExchangeRate));
            }
            else if (user.CurrencyPreference == CurrencyPreference.USD)
            {
                debtsFromDb.ForEach(d => d.Debt.Amount = (decimal)(d.Debt.Amount * d.Debt.CurrencyRate.UsdExchangeRate));
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
                debtsFromDb.ForEach(d => d.Debt.Amount *= (decimal)d.Debt.CurrencyRate.EurExchangeRate);
            }
            else if (user.CurrencyPreference == CurrencyPreference.USD)
            {
                debtsFromDb.ForEach(d => d.Debt.Amount *= (decimal)d.Debt.CurrencyRate.UsdExchangeRate);
            }

            List<DebtDto> debts = debtsFromDb.Select(Mapper.UserDebtAssignmentDbModelToDebtDto).ToList();

            return debts;
        }

        public async Task<Guid> CreateDebt(CreateDebtRequest createDebtRequest, string creatorEmail)
        {
            User creatorUser = await userRepository.GetUserByEmail(creatorEmail) ?? throw new UserNotFoundException(creatorEmail);
            User existingAccount = await userRepository.GetUserByEmail(createDebtRequest.Email);

            bool isUserAccount = existingAccount is not null;

            CurrencyRate currentCurrencyRate = await currencyRateRepository.GetLatestInsertedCurrencyRates();

            if (creatorUser.CurrencyPreference == CurrencyPreference.EUR)
            {
                createDebtRequest.Amount /= currentCurrencyRate.EurExchangeRate;
            }
            else if (creatorUser.CurrencyPreference == CurrencyPreference.USD)
            {
                createDebtRequest.Amount /= currentCurrencyRate.UsdExchangeRate;
            }

            DebtAssignment debtAssignment;
            if (isUserAccount)
            {
                User selectedUser = existingAccount;

                debtAssignment = Mapper.CreateDebtRequestToDebtAssignment(createDebtRequest, creatorUser, selectedUser, currentCurrencyRate);
                await debtAssignmentRepository.CreateDebt(debtAssignment);

                ReceiverInfoDto receiverInfoDto = Mapper.UserToReceiverInfoDto(debtAssignment.SelectedUser);
                DebtEmailInfoDto createdDebtEmailInfoDto = Mapper.DebtAssignmentToDebtEmailInfoDto(debtAssignment);
                await emailService.SendDebtCreatedNotification(receiverInfoDto, createdDebtEmailInfoDto);

                string jobId = await hangfireService.ScheduleDeadlineEmails(debtAssignment, receiverInfoDto);
                await debtAssignmentRepository.UpdateDeadlineReminderJobId(debtAssignment, jobId);
            }
            else
            {
                NonUser existingNonUser = await nonUserRepository.GetNonUserByEmail(createDebtRequest.Email);

                if (existingNonUser is not null)
                {
                    debtAssignment = Mapper.CreateDebtRequestToDebtAssignment(createDebtRequest, creatorUser, existingNonUser, currentCurrencyRate);
                }
                else
                {
                    debtAssignment = Mapper.CreateDebtRequestToDebtAssignment(createDebtRequest, creatorUser, currentCurrencyRate);
                }
                await debtAssignmentRepository.CreateDebt(debtAssignment);

                ReceiverInfoDto receiverInfoDto = Mapper.NonUserToReceiverInfoDto(debtAssignment.NonUser);
                DebtEmailInfoDto createdDebtEmailInfoDto = Mapper.UserToDebtEmailInfoDto(creatorUser);
                await emailService.SendNoAccountDebtCreatedNotification(receiverInfoDto, createdDebtEmailInfoDto);
            }

            return debtAssignment.Id;
        }

        public async Task DeleteDebt(string id, string email)
        {
            var debtFromDb = await debtAssignmentRepository.GetDebtById(id) ?? throw new EntityNotFoundException();
            if (!debtFromDb.CreatorUser.Email.Equals(email))
            {
                throw new ForbiddenRequestException();
            }

            if (debtFromDb.SelectedUser is not null)
            {
                ReceiverInfoDto receiverInfoDto = Mapper.UserToReceiverInfoDto(debtFromDb.SelectedUser);
                DebtEmailInfoDto deletedDebtEmailInfoDto = Mapper.DebtAssignmentToDebtEmailInfoDto(debtFromDb);

                await hangfireService.DeleteScheduledJob(debtFromDb.DeadlineReminderJobId!);
                await debtRepository.DeleteDebt(debtFromDb.Debt);

                await emailService.SendDebtDeletedNotification(receiverInfoDto, deletedDebtEmailInfoDto);
            }
            else if (debtFromDb.NonUser is not null)
            {
                ReceiverInfoDto receiverInfoDto = Mapper.NonUserToReceiverInfoDto(debtFromDb.NonUser);
                DebtEmailInfoDto deletedDebtEmailInfoDto = Mapper.NoAccountDebtAssignmentToDebtEmailInfoDto(debtFromDb);

                await debtRepository.DeleteDebt(debtFromDb.Debt);
                await emailService.SendDebtDeletedNotification(receiverInfoDto, deletedDebtEmailInfoDto);
            }
        }

        public async Task EditDebt(EditDebtRequest editDebtRequest, string email)
        {
            DebtAssignment debtAssignmentFromDb = await debtAssignmentRepository.GetDebtById(editDebtRequest.Guid) ?? throw new EntityNotFoundException();
            User user = await userRepository.GetUserByEmail(email) ?? throw new UserNotFoundException(email);

            CurrencyRate currentCurrencyRate = await currencyRateRepository.GetLatestInsertedCurrencyRates();

            // convert to base currency RON
            if (user.CurrencyPreference == CurrencyPreference.EUR)
            {
                editDebtRequest.Amount /= currentCurrencyRate.EurExchangeRate;
            }
            else if (user.CurrencyPreference == CurrencyPreference.USD)
            {
                editDebtRequest.Amount /= currentCurrencyRate.UsdExchangeRate;
            }

            bool isUserAccount = editDebtRequest.IsUserAccount;
            DebtAssignment updatedDebt;
            if (isUserAccount)
            {
                User selectedUser = await userRepository.GetUserByEmail(editDebtRequest.Email);
                updatedDebt = Mapper.EditDebtRequestToDebtAssignment(editDebtRequest, selectedUser, currentCurrencyRate);
                await debtAssignmentRepository.UpdateDebt(debtAssignmentFromDb, updatedDebt);  

                await hangfireService.DeleteScheduledJob(debtAssignmentFromDb.DeadlineReminderJobId!);

                ReceiverInfoDto receiverInfoDto = Mapper.UserToReceiverInfoDto(debtAssignmentFromDb.SelectedUser);
                string jobId = await hangfireService.ScheduleDeadlineEmails(debtAssignmentFromDb, receiverInfoDto);
                await debtAssignmentRepository.UpdateDeadlineReminderJobId(debtAssignmentFromDb, jobId);
            }
            else
            {
                NonUser nonUser = await nonUserRepository.GetNonUserByEmail(editDebtRequest.Email);
                updatedDebt = Mapper.EditDebtRequestToDebtAssignment(editDebtRequest, nonUser, currentCurrencyRate);


                await debtAssignmentRepository.UpdateDebt(debtAssignmentFromDb, updatedDebt);
            }

        }

        public async Task ApproveDebt(string debtId, string email)
        {
            DebtAssignment debtAssignmentFromDb = await debtAssignmentRepository.GetDebtById(debtId) ?? throw new EntityNotFoundException();
            await debtAssignmentRepository.ApproveDebt(debtAssignmentFromDb);

            await CreateExpenseRelatedToDebt(debtAssignmentFromDb, debtAssignmentFromDb.Debt.DateOfBorrowing, debtAssignmentFromDb.CreatorUser,
                $"Loaned to {debtAssignmentFromDb.SelectedUser.UserInfo.FirstName} {debtAssignmentFromDb.SelectedUser.UserInfo.LastName}.");
            await CreateIncomeRelatedToDebt(debtAssignmentFromDb, debtAssignmentFromDb.Debt.DateOfBorrowing, debtAssignmentFromDb.SelectedUser,
                $"Loaned from {debtAssignmentFromDb.CreatorUser.UserInfo.FirstName} {debtAssignmentFromDb.CreatorUser.UserInfo.LastName}.");
            // TODO: send e-mail notification
        }

        public async Task RejectDebt(string debtId, string email)
        {
            DebtAssignment debtAssignmentFromDb = await debtAssignmentRepository.GetDebtById(debtId) ?? throw new EntityNotFoundException();

            await hangfireService.DeleteScheduledJob(debtAssignmentFromDb.DeadlineReminderJobId!);
            await debtAssignmentRepository.RejectDebt(debtAssignmentFromDb);

            // TODO: send e-mail notification
        }

        public async Task MarkPaid(string debtId)
        {
            DebtAssignment debtAssignmentFromDb = await debtAssignmentRepository.GetDebtById(debtId) ?? throw new EntityNotFoundException();
            await debtAssignmentRepository.PayDebt(debtAssignmentFromDb);

            await CreateExpenseRelatedToDebt(debtAssignmentFromDb, DateTime.UtcNow, debtAssignmentFromDb.SelectedUser,
                $"Debt paid to {debtAssignmentFromDb.CreatorUser.UserInfo?.FirstName} {debtAssignmentFromDb.CreatorUser.UserInfo?.LastName}.");
            await CreateIncomeRelatedToDebt(debtAssignmentFromDb, DateTime.UtcNow, debtAssignmentFromDb.CreatorUser,
                $"Debt collected from {debtAssignmentFromDb.SelectedUser.UserInfo?.FirstName} {debtAssignmentFromDb.SelectedUser.UserInfo?.LastName}.");

            await hangfireService.DeleteScheduledJob(debtAssignmentFromDb.DeadlineReminderJobId!);

            // TODO: send e-mail notification
        }

        public async Task<TotalLoansAndDebtsDto> GetLoansAndDebtsTotalCount(string email)
        {
            User user = await userRepository.GetUserByEmail(email);
            var loansFromDb = await debtAssignmentRepository.GetAllReceivingDebtsByEmail(email);
            var debtsFromDb = await debtAssignmentRepository.GetAllUserDebtsByEmail(email);

            loansFromDb = loansFromDb.Where(l => l.Debt.DateOfBorrowing.Year == user.DashboardSelectedYear).ToList();
            debtsFromDb = debtsFromDb.Where(d => d.Debt.DateOfBorrowing.Year == user.DashboardSelectedYear).ToList();

            if (user.CurrencyPreference == CurrencyPreference.EUR)
            {
                loansFromDb.ForEach(d => d.Debt.Amount *= (decimal)d.Debt.CurrencyRate.EurExchangeRate);
                debtsFromDb.ForEach(d => d.Debt.Amount *= (decimal)d.Debt.CurrencyRate.EurExchangeRate);
            }
            else if (user.CurrencyPreference == CurrencyPreference.USD)
            {
                loansFromDb.ForEach(d => d.Debt.Amount *= (decimal)d.Debt.CurrencyRate.UsdExchangeRate);
                debtsFromDb.ForEach(d => d.Debt.Amount *= (decimal)d.Debt.CurrencyRate.UsdExchangeRate);
            }

            decimal totalLoans = loansFromDb.Sum(e => e.Debt.Amount);
            decimal totalDebts = debtsFromDb.Sum(e => e.Debt.Amount);

            return new TotalLoansAndDebtsDto
            {
                TotalLoans = totalLoans,
                TotalDebts = totalDebts
            };
        }

        private async Task CreateExpenseRelatedToDebt(DebtAssignment debtAssignment, DateTime date, User user, string message)
        {
            ExpenseCategory category = await expenseCategoryRepository.GetByName("Debts");
            CreateExpenseRequest createExpenseRequest = new CreateExpenseRequest
            {
                Amount = debtAssignment.Debt.Amount,
                Date = date.ToString(),
                Category = category?.Name,
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
                Category = category?.Name,
                Note = message
            };

            await incomesService.CreateIncome(createIncomeRequest, user.Email, true);
        }
    }
}
