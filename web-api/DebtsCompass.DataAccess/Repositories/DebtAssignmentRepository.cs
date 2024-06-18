using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Enums;
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
                .ThenInclude(d => d.CurrencyRate)
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
                .ThenInclude(d => d.CurrencyRate)
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
                .ThenInclude(d => d.CurrencyRate)
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

        public async Task<DebtAssignment> GetDebtById(string id)
        {
            var debtAssignmentFromDb = await dbContext.DebtAssignments.Where(d => d.Id.ToString().Equals(id))
                                                                      .Include(d => d.CreatorUser)
                                                                      .ThenInclude(u => u.UserInfo)
                                                                      .Include(d => d.SelectedUser)
                                                                      .ThenInclude(u => u.UserInfo)
                                                                      .Include(d => d.NonUser)
                                                                      .Include(d => d.Debt)
                                                                      .ThenInclude(d => d.CurrencyRate)
                                                                      .FirstOrDefaultAsync();

            return debtAssignmentFromDb;
        }

        public async Task UpdateDebt(DebtAssignment debtFromDb, DebtAssignment updatedDebt)
        {
            debtFromDb.Debt.Amount = updatedDebt.Debt.Amount;
            debtFromDb.Debt.BorrowReason = updatedDebt.Debt.BorrowReason;
            debtFromDb.Debt.DateOfBorrowing = updatedDebt.Debt.DateOfBorrowing;
            debtFromDb.Debt.DeadlineDate = updatedDebt.Debt.DeadlineDate;
            debtFromDb.SelectedUser = updatedDebt.SelectedUser;
            debtFromDb.NonUser = updatedDebt.NonUser;
            debtFromDb.Debt.CurrencyRate = updatedDebt.Debt.CurrencyRate;

            await dbContext.SaveChangesAsync();
        }
        
        public async Task ApproveDebt(DebtAssignment debtFromDb)
        {
            debtFromDb.Debt.Status = Status.Accepted;
            await dbContext.SaveChangesAsync();
        }

        public async Task RejectDebt(DebtAssignment debtFromDb)
        {
            debtFromDb.Debt.Status = Status.Rejected;
            await dbContext.SaveChangesAsync();
        }

        public async Task PayDebt(DebtAssignment debtFromDb)
        {
            debtFromDb.Debt.IsPaid = true;
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateDeadlineReminderJobId(DebtAssignment debtFromDb, string jobId)
        {
            debtFromDb.DeadlineReminderJobId = jobId;

            await dbContext.SaveChangesAsync();
        }
    }
}
