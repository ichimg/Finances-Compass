using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Enums;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain.Pagination;
using Microsoft.EntityFrameworkCore;

namespace DebtsCompass.DataAccess.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly DebtsCompassDbContext dbContext;
        public FriendshipRepository(DebtsCompassDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<PagedList<User>> GetUserFriendsById(string userId, PagedParameters pagedParameters)
        {
            return await dbContext.Friendships
                .Include(f => f.UserOne)
                .ThenInclude(u => u.UserInfo)
                .Include(f => f.UserTwo)
                .ThenInclude(u => u.UserInfo)
                .Where(f => (f.UserOneId == userId || f.UserTwoId == userId) && (f.Status == Status.Accepted))
                .Select(f => f.UserOneId == userId ? f.UserTwo : f.UserOne)
                .AsNoTracking()
                .ToPagedListAsync(pagedParameters.PageNumber, pagedParameters.PageSize);
        }

        public async Task<Friendship> GetUsersFriendStatus(User userOne, User userTwo)
        {
            var friendshipFromDb = await dbContext.Friendships
                .Where(f => f.UserOneId.Equals(userOne.Id) && f.UserTwoId.Equals(userTwo.Id)).FirstOrDefaultAsync();

            return friendshipFromDb;

        }

        public async Task Add(Friendship friendship)
        {
            if (friendship is null)
            {
                throw new ArgumentNullException(nameof(friendship));
            }
            await dbContext.Friendships.AddAsync(friendship);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(Friendship friendship)
        {
            dbContext.Friendships.Remove(friendship);
            await dbContext.SaveChangesAsync();
        }
    }
}
