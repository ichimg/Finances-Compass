using DebtsCompass.Domain.Entities.Dtos;

namespace DebtsCompass.Domain.Interfaces
{
    public interface ICategoriesService
    {
        Task<List<CategoryDto>> GetAllExpenseCategoriesByEmail(string userEmail);
        Task<List<CategoryDto>> GetAllIncomeCategoriesByEmail(string userEmail);
    }
}