using DebtsCompass.Domain.Entities.Models;
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

        public async Task<List<DebtAssignment>> GetAllReceivingDebtsByEmail(string email)
        {
            return await dbContext.DebtAssignments
                .Include(da => da.Debt)
                .Include(da => da.CreatorUser)
                .Include(da => da.NonUser)
                .Include(da => da.SelectedUser)
                .ThenInclude(u => u.UserInfo)
                .Where(da => da.CreatorUser.Email == email)
                .AsNoTracking()
                .ToListAsync();

        }

        public async Task<List<DebtAssignment>> GetAllUserDebtsByEmail(string email)
        {
            return await dbContext.DebtAssignments
                .Include(da => da.Debt)
                .Include(da => da.CreatorUser)
                .ThenInclude(u => u.UserInfo)
                .Include(da => da.SelectedUser)
                .ThenInclude(u => u.UserInfo)
                .Where(da => da.SelectedUser.Email == email)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<DebtAssignment>> GetAllNonUserDebtsByEmail(string email)
        {
            return await dbContext.DebtAssignments
                .Include(da => da.Debt)
                .Include(da => da.CreatorUser)
                .ThenInclude(u => u.UserInfo)
                .Include(da => da.NonUser)
                .Where(da => da.NonUser.PersonEmail == email)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task CreateDebt(DebtAssignment debtAssignment)
        {
            if(debtAssignment is null)
            {
                throw new ArgumentNullException(nameof(debtAssignment));
            }

            await dbContext.DebtAssignments.AddAsync(debtAssignment);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateRange(IEnumerable<DebtAssignment> debtAssignments)
        {
            dbContext.DebtAssignments.UpdateRange(debtAssignments);
            await dbContext.SaveChangesAsync();
        }
    }
}
