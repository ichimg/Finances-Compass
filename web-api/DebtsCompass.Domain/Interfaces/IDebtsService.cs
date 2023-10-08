using DebtsCompass.Domain.DtoResponses;

namespace DebtsCompass.Domain.Services
{
    public interface IDebtsService
    {
        Task<List<DebtDto>> GetAll(string email);
    }
}