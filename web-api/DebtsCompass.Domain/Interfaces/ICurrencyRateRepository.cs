using DebtsCompass.Domain.Entities.Models;

namespace DebtsCompass.Domain.Interfaces
{
    public interface ICurrencyRateRepository
    {
        Task InsertCurrencyRate(CurrencyRate currencyRate);
        Task<CurrencyRate> GetLatestInsertedCurrencyRates();
    }
}