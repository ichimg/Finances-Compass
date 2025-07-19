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

        public async Task<PagedResponse<User>> GetUserFriendsById(string userId, PagedParameters pagedParameters)
        {
            return await dbContext.Friendships
                .Include(f => f.RequesterUser)
                .ThenInclude(u => u.UserInfo)
                .Include(f => f.SelectedUser)
                .ThenInclude(u => u.UserInfo)
                .Where(f => (f.RequesterUserId == userId || f.SelectedUserId == userId) && (f.Status == Status.Accepted))
                .Select(f => f.RequesterUserId == userId ? f.SelectedUser : f.RequesterUser)
                .AsNoTracking()
                .ToPagedListAsync(pagedParameters.PageNumber, pagedParameters.PageSize);
        }

        public async Task<List<User>> GetAllUserFriendsById(string userId)
        {
            return await dbContext.Friendships
              .Include(f => f.RequesterUser)
              .ThenInclude(u => u.UserInfo)
              .Include(f => f.SelectedUser)
              .ThenInclude(u => u.UserInfo)
              .Where(f => (f.RequesterUserId == userId || f.SelectedUserId == userId) && (f.Status == Status.Accepted))
              .Select(f => f.RequesterUserId == userId ? f.SelectedUser : f.RequesterUser)
              .AsNoTracking()
              .ToListAsync();
        }

        public async Task<PagedResponse<User>> GetUserFriendRequestsById(string userId, PagedParameters pagedParameters)
        {
            return await dbContext.Friendships
                .Include(f => f.RequesterUser)
                .ThenInclude(u => u.UserInfo)
                .Include(f => f.SelectedUser)
                .ThenInclude(u => u.UserInfo)
                .Where(f => (f.SelectedUserId == userId) && (f.Status == Status.Pending))
                .Select(f => f.RequesterUser)
                .AsNoTracking()
                .ToPagedListAsync(pagedParameters.PageNumber, pagedParameters.PageSize);
        }

        public async Task<Friendship> GetUsersFriendship(string requesterUserId, string receiverUserId)
        {
            var friendshipFromDb = await dbContext.Friendships
                .Where(f => f.RequesterUserId.Equals(requesterUserId) && f.SelectedUserId.Equals(receiverUserId)).FirstOrDefaultAsync();

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

        public async Task AcceptFriendRequest(Friendship friendshipFromDb)
        {
            friendshipFromDb.Status = Status.Accepted;
            await dbContext.SaveChangesAsync();
        }

        public async Task RejectFriendRequest(Friendship friendshipFromDb)
        {
            friendshipFromDb.Status = Status.Rejected;
            await dbContext.SaveChangesAsync();
        }
    }
}
