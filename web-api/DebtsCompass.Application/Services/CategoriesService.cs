using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities.Dtos;
using DebtsCompass.Domain.Interfaces;

namespace DebtsCompass.Application.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<List<ExpenseCategoryDto>> GetAllByEmail(string userEmail)
        {
            var categoriesFromDb = await categoryRepository.GetAllByEmail(userEmail);

            List<ExpenseCategoryDto> categories = categoriesFromDb.Select(Mapper.ExpenseCategoryToExpenseCategoryDto).ToList();

            return categories;
        }
    }
}
