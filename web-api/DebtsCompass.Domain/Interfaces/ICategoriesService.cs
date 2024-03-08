using DebtsCompass.Domain.Entities.Dtos;

namespace DebtsCompass.Domain.Interfaces
{
    public interface ICategoriesService
    {
        Task<List<ExpenseCategoryDto>> GetAllByEmail(string userEmail);
    }
}