using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Requests;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IDebtsService
    {
        Task<List<DebtDto>> GetAllReceivingDebts(string email);
        Task<List<DebtDto>> GetAllUserDebts(string email);
        Task<Guid> CreateDebt(CreateDebtRequest createDebtRequest, string creatorEmail);
        Task DeleteDebt(string id, string email);
        Task EditDebt(EditDebtRequest editDebtRequest, string email);
        Task ApproveDebt(string debtId, string email);
        Task RejectDebt(string debtId, string email);
        Task MarkPaid(string debtId);
    }
}