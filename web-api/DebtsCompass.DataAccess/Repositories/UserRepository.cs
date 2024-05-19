using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Enums;
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
                .Include(u => u.ReceivingFriendships)
                .Include(u => u.RequestedFriendships)
                .Include(u => u.Expenses)
                .ThenInclude(e => e.Category)
                .Include(u => u.Incomes)
                .Include(u => u.UserInfo)
                .ThenInclude(u => u.Address)
                .Where(u => u.Email.Equals(email)).FirstOrDefaultAsync();

            return userFromDb;
        }

        public async Task<List<User>> GetAllAllowedDataConsent(string targetUserEmail)
        {
            return await dbContext.Users
                .Include(u => u.ReceivingFriendships)
                .ThenInclude(rf => rf.RequesterUser)
                .Include(u => u.RequestedFriendships)
                .ThenInclude(rf => rf.SelectedUser)
                .Include(u => u.Expenses)
                .ThenInclude(e => e.Category)
                .Include(u => u.UserInfo)
                .ThenInclude(u => u.Address)
                .Where(u => u.IsDataConsent && u.Email != targetUserEmail 
                && !u.ReceivingFriendships.Any(rf => rf.RequesterUser.Email == targetUserEmail) && !u.RequestedFriendships.Any(rf => rf.SelectedUser.Email == targetUserEmail))
                .ToListAsync();
        }

        public async Task<User> GetUserByEmailWithExpensesByMonth(string email, YearMonthDto yearMonthDto)
        {
            var startOfMonth = new DateTime(Int32.Parse(yearMonthDto.Year), Int32.Parse(yearMonthDto.Month), 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            User userFromDb = await dbContext.Users
                    .Include(u => u.Expenses.Where(e => e.Date >= startOfMonth && e.Date <= endOfMonth))
                    .ThenInclude(e => e.Category)
                    .Include(u => u.Incomes.Where(i => i.Date >= startOfMonth && i.Date <= endOfMonth))
                    .ThenInclude(i => i.Category)
                    .Where(u => u.Email.Equals(email))
                    .FirstOrDefaultAsync();

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
            .Where(u => (u.UserName.ToUpper() != currentUser.UserName.ToUpper()
                        && u.EmailConfirmed == true
                        && u.UserName.ToUpper().Contains(query.ToUpper()))
                        || (u.UserName.ToUpper() != currentUser.UserName.ToUpper()
                        && u.EmailConfirmed == true
                        && u.UserInfo.FirstName.ToUpper().Contains(query.ToUpper()))
                        || (u.UserName.ToUpper() != currentUser.UserName.ToUpper()
                        && u.EmailConfirmed == true
                        && u.UserInfo.LastName.ToUpper().Contains(query.ToUpper())))
            .OrderBy(u => u.UserInfo.Address.City == currentUser.UserInfo.Address.City ? 0 : 1) 
            .ThenBy(u => u.UserInfo.Address.County == currentUser.UserInfo.Address.County ? 0 : 1) 
            .ThenBy(u => u.UserInfo.Address.Country == currentUser.UserInfo.Address.Country ? 0 : 1) 
            .ToPagedListAsync(pagedParameters.PageNumber, pagedParameters.PageSize);
        }

        public async Task ChangeDashboardYear(User user, int year)
        {
            user.DashboardSelectedYear = year;

            await dbContext.SaveChangesAsync();
        }

        public async Task ChangeCurrencyPreference(User user, CurrencyPreference currency)
        {
            user.CurrencyPreference = currency;

            await dbContext.SaveChangesAsync();
        }
    }
}
