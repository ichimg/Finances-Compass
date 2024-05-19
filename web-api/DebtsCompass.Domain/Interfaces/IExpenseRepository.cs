using DebtsCompass.Domain.Entities.Models;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IExpenseRepository
    {
        Task CreateExpense(Expense expense);
        Task DeleteExpense(Expense expense);
        Task<Expense> GetExpenseById(string id);
        Task UpdateExpense(Expense expenseFromDb, Expense updatedExpense);
    }
}