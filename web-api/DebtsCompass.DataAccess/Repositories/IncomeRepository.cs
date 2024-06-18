using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DebtsCompass.DataAccess.Repositories
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly DebtsCompassDbContext dbContext;
        public IncomeRepository(DebtsCompassDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateIncome(Income income)
        {
            if (income is null)
            {
                throw new ArgumentNullException(nameof(income));
            }

            await dbContext.Incomes.AddAsync(income);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Income> GetIncomeById(string id)
        {
            return await dbContext.Incomes
                        .Include(e => e.Category)
                        .Include(e => e.User)
                        .FirstOrDefaultAsync(e => e.Id.ToString().Equals(id));
        }

        public async Task DeleteIncome(Income income)
        {
            dbContext.Incomes.Remove(income);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateIncome(Income incomeFromDb, Income updatedIncome)
        {
            incomeFromDb.Amount = updatedIncome.Amount;
            incomeFromDb.Category = updatedIncome.Category;
            incomeFromDb.Note = updatedIncome.Note;
            incomeFromDb.CurrencyRate = updatedIncome.CurrencyRate;

            await dbContext.SaveChangesAsync();
        }
    }
}
