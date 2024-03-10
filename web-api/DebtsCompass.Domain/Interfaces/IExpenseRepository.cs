using DebtsCompass.Domain.Entities.Models;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IExpenseRepository
    {
        Task CreateExpense(Expense expense);
        Task DeleteExpense(Expense expense);
        Task<Expense> GetExpenseById(string id);
        Task UpdateDebt(Expense expenseFromDb, Expense updatedExpense);
    }
}