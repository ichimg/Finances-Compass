using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Interfaces;

namespace DebtsCompass.DataAccess.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly DebtsCompassDbContext dbContext;
        public ExpenseRepository(DebtsCompassDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateExpense(Expense expense)
        {
            if (expense is null)
            {
                throw new ArgumentNullException(nameof(expense));
            }

            await dbContext.Expenses.AddAsync(expense);
            await dbContext.SaveChangesAsync();
        }
    }
}
