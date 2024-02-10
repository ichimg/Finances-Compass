using DebtsCompass.Domain.Entities.DtoResponses;

namespace DebtsCompass.Domain.Interfaces
{
    public interface ICurrencyRatesJob
    {
        Task<CurrencyDto> GetLatestCurrencyRates();
    }
}