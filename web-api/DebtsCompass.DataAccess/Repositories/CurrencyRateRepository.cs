using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DebtsCompass.DataAccess.Repositories
{
    public class CurrencyRateRepository : ICurrencyRateRepository
    {
        private readonly DebtsCompassDbContext dbContext;
        public CurrencyRateRepository(DebtsCompassDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task InsertCurrencyRate(CurrencyRate currencyRate)
        {
            if (currencyRate is null)
            {
                throw new ArgumentNullException(nameof(currencyRate));
            }

            await dbContext.CurrencyRates.AddAsync(currencyRate);
            await dbContext.SaveChangesAsync();
        }

        public async Task<CurrencyRate> GetLatestInsertedCurrencyRates()
        {
            return await dbContext.CurrencyRates.OrderByDescending(cr => cr.RequestDate).FirstOrDefaultAsync();
        }
    }
}
