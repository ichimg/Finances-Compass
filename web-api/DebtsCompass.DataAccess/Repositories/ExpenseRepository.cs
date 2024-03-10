using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Expense> GetExpenseById(string id)
        {
            return await dbContext.Expenses
                        .Include(e => e.Category)
                        .Include(e => e.User)
                        .FirstOrDefaultAsync(e => e.Id.ToString().Equals(id));
        }

        public async Task DeleteExpense(Expense expense)
        {
            dbContext.Expenses.Remove(expense);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateDebt(Expense expenseFromDb, Expense updatedExpense)
        {
            expenseFromDb.Amount = updatedExpense.Amount;
            expenseFromDb.Category = updatedExpense.Category;
            expenseFromDb.Note = updatedExpense.Note;

            await dbContext.SaveChangesAsync();
        }
    }
}
