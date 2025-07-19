using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Interfaces;

namespace DebtsCompass.DataAccess.Repositories
{
    public class DebtRepository : IDebtRepository
    {
        private readonly DebtsCompassDbContext dbContext;
        public DebtRepository(DebtsCompassDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task DeleteDebt(Debt debt)
        {
            dbContext.Debts.Remove(debt);
            await dbContext.SaveChangesAsync();
        }
    }
}
