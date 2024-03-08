using DebtsCompass.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using DebtsCompass.Domain.Interfaces;

namespace DebtsCompass.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DebtsCompassDbContext dbContext;

        public CategoryRepository(DebtsCompassDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<ExpenseCategory>> GetAllByEmail(string email)
        {
            return await dbContext.ExpenseCategories.Include(c => c.User)
                .Where(c => c.User.Email == email || c.User == null)
                .ToListAsync();
        }

        public async Task<ExpenseCategory> GetByName(string name)
        {
            return await dbContext.ExpenseCategories.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
