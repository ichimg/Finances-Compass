using DebtsCompass.Domain.Entities.Models;

namespace DebtsCompass.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<ExpenseCategory>> GetAllByEmail(string email);
        Task<ExpenseCategory> GetByName(string name);
    }
}