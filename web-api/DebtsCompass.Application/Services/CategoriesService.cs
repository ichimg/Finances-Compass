using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities.Dtos;
using DebtsCompass.Domain.Interfaces;

namespace DebtsCompass.Application.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IExpenseCategoryRepository expenseCategoryRepository;
        private readonly IIncomeCategoryRepository incomeCategoryRepository;

        public CategoriesService(IExpenseCategoryRepository expenseCategoryRepository, IIncomeCategoryRepository incomeCategoryRepository)
        {
            this.expenseCategoryRepository = expenseCategoryRepository;
            this.incomeCategoryRepository = incomeCategoryRepository;
        }

        public async Task<List<CategoryDto>> GetAllExpenseCategoriesByEmail(string userEmail)
        {
            var categoriesFromDb = await expenseCategoryRepository.GetAllByEmail(userEmail);

            List<CategoryDto> categories = categoriesFromDb.Select(Mapper.ExpenseCategoryToCategoryDto).ToList();

            return categories;
        }

        public async Task<List<CategoryDto>> GetAllIncomeCategoriesByEmail(string userEmail)
        {
            var categoriesFromDb = await incomeCategoryRepository.GetAllByEmail(userEmail);

            List<CategoryDto> categories = categoriesFromDb.Select(Mapper.IncomeCategoryToCategoryDto).ToList();

            return categories;
        }
    }
}
