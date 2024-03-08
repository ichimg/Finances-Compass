using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Enums;
using DebtsCompass.Domain.Interfaces;

namespace DebtsCompass.Application.Services
{
    public class ExpensesService : IExpensesService
    {
        private readonly IExpenseRepository expenseRepository;
        private readonly IUserRepository userRepository;
        private readonly ICurrencyRatesJob currencyRatesJob;
        private readonly ICategoryRepository categoryRepository;

        public ExpensesService(IExpenseRepository expenseRepository, IUserRepository userRepository, ICurrencyRatesJob currencyRatesJob, ICategoryRepository categoryRepository)
        {
            this.expenseRepository = expenseRepository;
            this.userRepository = userRepository;
            this.currencyRatesJob = currencyRatesJob;
            this.categoryRepository = categoryRepository;
        }

        public async Task<Guid> CreateExpense(CreateExpenseRequest createExpenseRequest, string creatorEmail)
        {
            User user = await userRepository.GetUserByEmail(creatorEmail) ?? throw new UserNotFoundException(creatorEmail);

            CurrencyDto currentCurrencies = await currencyRatesJob.GetLatestCurrencyRates();

            if (user.CurrencyPreference == CurrencyPreference.EUR)
            {
                createExpenseRequest.Amount /= currentCurrencies.EurExchangeRate;
            }
            else if (user.CurrencyPreference == CurrencyPreference.USD)
            {
                createExpenseRequest.Amount /= currentCurrencies.UsdExchangeRate;
            }

            ExpenseCategory category = await categoryRepository.GetByName(createExpenseRequest.Category);

            Expense expense = Mapper.CreateExpenseRequestToExpense(createExpenseRequest, user, currentCurrencies, category);

            await expenseRepository.CreateExpense(expense);

            return expense.Id;
        }
    }
}
