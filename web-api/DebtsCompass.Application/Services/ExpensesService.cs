using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Enums;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain.TotalList;

namespace DebtsCompass.Application.Services
{
    public class ExpensesService : IExpensesService
    {
        private readonly IExpenseRepository expenseRepository;
        private readonly IUserRepository userRepository;
        private readonly ICurrencyRatesJob currencyRatesJob;
        private readonly IExpenseCategoryRepository categoryRepository;

        public ExpensesService(IExpenseRepository expenseRepository, IUserRepository userRepository, ICurrencyRatesJob currencyRatesJob, IExpenseCategoryRepository categoryRepository)
        {
            this.expenseRepository = expenseRepository;
            this.userRepository = userRepository;
            this.currencyRatesJob = currencyRatesJob;
            this.categoryRepository = categoryRepository;
        }

        public async Task<Guid> CreateExpense(CreateExpenseRequest createExpenseRequest, string creatorEmail, bool isRonCurrency = false)
        {
            User user = await userRepository.GetUserByEmail(creatorEmail) ?? throw new UserNotFoundException(creatorEmail);

            CurrencyDto currentCurrencies = await currencyRatesJob.GetLatestCurrencyRates();

            if (!isRonCurrency)
            {
                if (user.CurrencyPreference == CurrencyPreference.EUR)
                {
                    createExpenseRequest.Amount /= currentCurrencies.EurExchangeRate;
                }
                else if (user.CurrencyPreference == CurrencyPreference.USD)
                {
                    createExpenseRequest.Amount /= currentCurrencies.UsdExchangeRate;
                }
            }

            ExpenseCategory category = await categoryRepository.GetByName(createExpenseRequest.Category);

            Expense expense = Mapper.CreateExpenseRequestToExpense(createExpenseRequest, user, currentCurrencies, category);

            await expenseRepository.CreateExpense(expense);

            return expense.Id;
        }

        public async Task DeleteExpense(string id, string email)
        {
            var expenseFromDb = await expenseRepository.GetExpenseById(id);

            if (expenseFromDb is null)
            {
                throw new EntityNotFoundException();
            }

            if (!expenseFromDb.User.Email.Equals(email))
            {
                throw new ForbiddenRequestException();
            }

            await expenseRepository.DeleteExpense(expenseFromDb);
        }

        public async Task EditExpense(EditExpenseRequest editExpenseRequest, string email)
        {
            User user = await userRepository.GetUserByEmail(email) ?? throw new UserNotFoundException(email);
            Expense expenseFromDb = await expenseRepository.GetExpenseById(editExpenseRequest.Guid) ?? throw new EntityNotFoundException();

            CurrencyDto currentCurrencies = await currencyRatesJob.GetLatestCurrencyRates();

            if (user.CurrencyPreference == CurrencyPreference.EUR)
            {
                editExpenseRequest.Amount /= currentCurrencies.EurExchangeRate;
            }
            else if (user.CurrencyPreference == CurrencyPreference.USD)
            {
                editExpenseRequest.Amount /= currentCurrencies.UsdExchangeRate;
            }

            ExpenseCategory category = await categoryRepository.GetByName(editExpenseRequest.Category);
            Expense updatedExpense = Mapper.EditExpenseRequestToExpense(editExpenseRequest, category);

            await expenseRepository.UpdateDebt(expenseFromDb, updatedExpense);
        }

        public async Task<TotalList<ExpenseOrIncomeDto>> GetAllByEmail(string email, YearMonthDto yearMonthDto)
        {
            User user = await userRepository.GetUserByEmailWithExpensesByMonth(email, yearMonthDto);
            var expensesFromDb = user.Expenses.ToList();
            var incomesFromDb = user.Incomes.ToList();

            if (user.CurrencyPreference == CurrencyPreference.EUR)
            {
                expensesFromDb.ForEach(e => e.Amount *= (decimal)e.EurExchangeRate);
            }
            else if (user.CurrencyPreference == CurrencyPreference.USD)
            {
                expensesFromDb.ForEach(e => e.Amount *= (decimal)e.UsdExchangeRate);
            }
            List<ExpenseOrIncomeDto> expenses = expensesFromDb.Select(Mapper.ExpenseToExpenseOrIncomeDto).ToList();
            decimal totalExpenses = expenses.Sum(e => e.Amount);  

            if (user.CurrencyPreference == CurrencyPreference.EUR)
            {
                incomesFromDb.ForEach(i => i.Amount *= (decimal)i.EurExchangeRate);
            }
            else if (user.CurrencyPreference == CurrencyPreference.USD)
            {
                incomesFromDb.ForEach(i => i.Amount *= (decimal)i.UsdExchangeRate);
            }
            List<ExpenseOrIncomeDto> incomes = incomesFromDb.Select(Mapper.IncomeToExpenseOrIncomeDto).ToList();
            decimal totalIncomes = incomes.Sum(i => i.Amount);
            var expenseOrIncomes = expenses.Concat(incomes);


            return new TotalList<ExpenseOrIncomeDto>(expenseOrIncomes.ToList(), totalExpenses, totalIncomes);
        }

        public async Task<TotalExpensesAndIncomesDto> GetExpensesAndIncomesTotalCount(string email)
        {
            User user = await userRepository.GetUserByEmail(email);
            var expensesFromDb = user.Expenses.ToList();
            var incomesFromDb = user.Incomes.ToList();

            if (user.CurrencyPreference == CurrencyPreference.EUR)
            {
                expensesFromDb.ForEach(e => e.Amount *= (decimal)e.EurExchangeRate);
                incomesFromDb.ForEach(i => i.Amount *= (decimal)i.EurExchangeRate);
            }
            else if (user.CurrencyPreference == CurrencyPreference.USD)
            {
                expensesFromDb.ForEach(e => e.Amount *= (decimal)e.UsdExchangeRate);
                incomesFromDb.ForEach(i => i.Amount *= (decimal)i.EurExchangeRate);
            }

            decimal totalExpenses = expensesFromDb.Sum(e => e.Amount);
            decimal totalIncomes = incomesFromDb.Sum(e => e.Amount);

            return new TotalExpensesAndIncomesDto
            {
                TotalExpenses = totalExpenses,
                TotalIncomes = totalIncomes
            };
        }
    }
}
