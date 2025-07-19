using DebtsCompass.Domain.Entities.Models;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IExpenseCategoryRepository
    {
        Task<List<ExpenseCategory>> GetAllByEmail(string email);
        Task<ExpenseCategory> GetByName(string name);
    }
}