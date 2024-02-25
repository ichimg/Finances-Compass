using DebtsCompass.Domain.Entities.Models;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IDebtRepository
    {
        Task DeleteDebt(Debt debt);
    }
}