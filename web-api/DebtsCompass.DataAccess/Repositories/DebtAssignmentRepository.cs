using DebtsCompass.Domain.Entities;
using DebtsCompass.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DebtsCompass.DataAccess.Repositories
{
    public class DebtAssignmentRepository : IDebtAssignmentRepository
    {
        private readonly DebtsCompassDbContext dbContext;
        public DebtAssignmentRepository(DebtsCompassDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<DebtAssignment>> GetAllByEmailForExistingUsers(string email)
        {
            return await dbContext.DebtAssignments
                .Include(da => da.Debt)
                .Include(da => da.CreatorUser)
                .Include(da => da.SelectedUser)
                .ThenInclude(u => u.UserInfo)
                .Where(da => da.CreatorUser.Email == email)
                .ToListAsync();
        }

        public async Task<List<DebtAssignment>> GetAllByEmailForNotExistingUsers(string email)
        {
            return await dbContext.DebtAssignments
               .Include(da => da.Debt)
               .Include(da => da.CreatorUser)
               .Include(da => da.NonUserDebtAssignment)
               .Where(da => da.CreatorUser.Email == email && da.NonUserDebtAssignment != null)
               .ToListAsync();
        }

    }
}
