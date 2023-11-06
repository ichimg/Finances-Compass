using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Requests;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IDebtsService
    {
        Task<List<DebtDto>> GetAllReceivingDebts(string email);
        Task<List<DebtDto>> GetAllUserDebts(string email);
        Task CreateDebt(CreateDebtRequest createDebtRequest, string creatorEmail);
    }
}