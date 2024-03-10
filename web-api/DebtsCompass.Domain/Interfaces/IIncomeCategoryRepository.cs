using DebtsCompass.Domain.Entities.Models;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IIncomeCategoryRepository
    {
        Task<List<IncomeCategory>> GetAllByEmail(string email);
        Task<IncomeCategory> GetByName(string name);
    }
}