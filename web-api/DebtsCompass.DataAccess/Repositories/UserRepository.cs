using DebtsCompass.Domain.Entities;
using DebtsCompass.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DebtsCompass.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DebtsCompassDbContext dbContext;
        public UserRepository(DebtsCompassDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User userFromDb = await dbContext.Users.Where(u => u.Email.Equals(email)).FirstOrDefaultAsync();

            return userFromDb;
        }

        public async Task Add(User user)
        {
            if(user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }
    }
}
