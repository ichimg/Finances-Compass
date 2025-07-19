using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DebtsCompass.DataAccess.Repositories
{
    public class IncomeCategoryRepository : IIncomeCategoryRepository
    {
        private readonly DebtsCompassDbContext dbContext;

        public IncomeCategoryRepository(DebtsCompassDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<IncomeCategory>> GetAllByEmail(string email)
        {
            return await dbContext.IncomeCategories.Include(c => c.User)
                .Where(c => c.User.Email == email || c.User == null)
                .ToListAsync();
        }

        public async Task<IncomeCategory> GetByName(string name)
        {
            return await dbContext.IncomeCategories.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
