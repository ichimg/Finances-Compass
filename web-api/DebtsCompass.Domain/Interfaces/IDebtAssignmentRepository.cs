using DebtsCompass.Domain.Entities;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IDebtAssignmentRepository
    {
        Task<List<DebtAssignment>> GetAllByEmailForExistingUsers(string email);
        Task<List<DebtAssignment>> GetAllByEmailForNotExistingUsers(string email);
    }
}