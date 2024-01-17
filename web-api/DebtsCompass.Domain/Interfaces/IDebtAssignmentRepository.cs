using DebtsCompass.Domain.Entities.Models;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IDebtAssignmentRepository
    {
        Task<List<DebtAssignment>> GetAllReceivingDebtsByEmail(string email);
        Task<List<DebtAssignment>> GetAllUserDebtsByEmail(string email);
        Task CreateDebt(DebtAssignment debtAssignment);
        Task<List<DebtAssignment>> GetAllNonUserDebtsByEmail(string email);
        Task UpdateRange(IEnumerable<DebtAssignment> debtAssignments);
        Task<DebtAssignment> GetDebtById(string id);
        Task DeleteDebt(DebtAssignment debtAssignment);
        Task UpdateDebt(DebtAssignment debtFromDb, DebtAssignment debtToUpdate);
        Task ApproveDebt(DebtAssignment debtFromDb);
        Task RejectDebt(DebtAssignment debtAssignmentFromDb);
    }
}