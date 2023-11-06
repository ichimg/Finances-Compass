using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DebtsCompass.DataAccess.Repositories
{
    public class NonUserRepository : INonUserRepository
    {
        private readonly DebtsCompassDbContext dbContext;
        public NonUserRepository(DebtsCompassDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<NonUser> GetNonUserByEmail(string email)
        {
            return await dbContext.NonUsers.FirstOrDefaultAsync(n => n.PersonEmail == email);
        }

        public async Task Delete(NonUser nonUser)
        {
            dbContext.NonUsers.Remove(nonUser);
            await dbContext.SaveChangesAsync();
        }
    }
}
