using DebtsCompass.Domain.Entities.DtoResponses;

namespace DebtsCompass.Domain.Services
{
    public interface IDebtsService
    {
        Task<List<DebtDto>> GetAllReceivingDebts(string email);
        Task<List<DebtDto>> GetAllUserDebts(string email);
    }
}