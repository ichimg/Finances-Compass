using DebtsCompass.Domain.Entities.Models;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IExpenseRepository
    {
        Task CreateExpense(Expense expense);
    }
}