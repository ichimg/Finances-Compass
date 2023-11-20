using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain.Pagination;
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
            User userFromDb = await dbContext.Users
                .Include(u => u.UserInfo)
                .ThenInclude(u => u.Address)
                .Where(u => u.Email.Equals(email)).FirstOrDefaultAsync();

            return userFromDb;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            User userFromDb = await dbContext.Users
                .Include(u => u.UserInfo).Where(u => u.UserName.Equals(username)).FirstOrDefaultAsync();

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

        public async Task Update(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<PagedList<User>> GetUsersBySearchQuery(string query, User currentUser, PagedParameters pagedParameters)
        {
            
            return await dbContext.Users
            .Include(u => u.UserInfo)
            .ThenInclude(u => u.Address)
            .Where(u => u.UserName.ToUpper().Contains(query.ToUpper())
                      || u.UserInfo.FirstName.ToUpper().Contains(query.ToUpper())
                      || u.UserInfo.LastName.ToUpper().Contains(query.ToUpper()))
            .OrderBy(u => u.UserInfo.Address.City == currentUser.UserInfo.Address.City ? 0 : 1) 
            .ThenBy(u => u.UserInfo.Address.County == currentUser.UserInfo.Address.County ? 0 : 1) 
            .ThenBy(u => u.UserInfo.Address.Country == currentUser.UserInfo.Address.Country ? 0 : 1) 
            .ToPagedListAsync(pagedParameters.PageNumber, pagedParameters.PageSize);
        }
    }
}
