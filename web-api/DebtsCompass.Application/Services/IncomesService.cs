using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Enums;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain;

namespace DebtsCompass.Application.Services
{
    public class IncomesService : IIncomesService
    {
        private readonly IIncomeRepository incomeRepository;
        private readonly IUserRepository userRepository;
        private readonly ICurrencyRatesJob currencyRatesJob;
        private readonly IIncomeCategoryRepository categoryRepository;

        public IncomesService(IIncomeRepository incomeRepository, IUserRepository userRepository,
            ICurrencyRatesJob currencyRatesJob, IIncomeCategoryRepository categoryRepository)
        {
            this.incomeRepository = incomeRepository;
            this.userRepository = userRepository;
            this.currencyRatesJob = currencyRatesJob;
            this.categoryRepository = categoryRepository;
        }

        public async Task<Guid> CreateIncome(CreateIncomeRequest createIncomeRequest, string creatorEmail, bool isRonCurrency = false)
        {
            User user = await userRepository.GetUserByEmail(creatorEmail) ?? throw new UserNotFoundException(creatorEmail);

            CurrencyDto currentCurrencies = await currencyRatesJob.GetLatestCurrencyRates();

            if (!isRonCurrency)
            {
                if (user.CurrencyPreference == CurrencyPreference.EUR)
                {
                    createIncomeRequest.Amount /= currentCurrencies.EurExchangeRate;
                }
                else if (user.CurrencyPreference == CurrencyPreference.USD)
                {
                    createIncomeRequest.Amount /= currentCurrencies.UsdExchangeRate;
                }
            }

            IncomeCategory category = await categoryRepository.GetByName(createIncomeRequest.Category);

            Income income = Mapper.CreateIncomeRequestToIncome(createIncomeRequest, user, currentCurrencies, category);

            await incomeRepository.CreateIncome(income);

            return income.Id;
        }

        public async Task DeleteIncome(string id, string email)
        {
            var incomeFromDb = await incomeRepository.GetIncomeById(id);

            if (incomeFromDb is null)
            {
                throw new EntityNotFoundException();
            }

            if (!incomeFromDb.User.Email.Equals(email))
            {
                throw new ForbiddenRequestException();
            }

            await incomeRepository.DeleteIncome(incomeFromDb);
        }

        public async Task UpdateIncome(EditIncomeRequest editIncomeRequest, string email)
        {
            User user = await userRepository.GetUserByEmail(email) ?? throw new UserNotFoundException(email);
            Income incomeFromDb = await incomeRepository.GetIncomeById(editIncomeRequest.Guid) ?? throw new EntityNotFoundException();

            CurrencyDto currentCurrencies = await currencyRatesJob.GetLatestCurrencyRates();

            if (user.CurrencyPreference == CurrencyPreference.EUR)
            {
                editIncomeRequest.Amount /= currentCurrencies.EurExchangeRate;
            }
            else if (user.CurrencyPreference == CurrencyPreference.USD)
            {
                editIncomeRequest.Amount /= currentCurrencies.UsdExchangeRate;
            }

            IncomeCategory category = await categoryRepository.GetByName(editIncomeRequest.Category);
            Income updatedIncome = Mapper.EditIncomeRequestToIncome(editIncomeRequest, category);

            await incomeRepository.UpdateDebt(incomeFromDb, updatedIncome);
        }
    }
}
