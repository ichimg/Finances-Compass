using DebtsCompass.Domain.Entities;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IDebtAssignmentRepository
    {
        Task<List<DebtAssignment>> GetAllReceivingDebtsByEmail(string email);
        Task<List<DebtAssignment>> GetAllUserDebtsByEmail(string email);
    }
}