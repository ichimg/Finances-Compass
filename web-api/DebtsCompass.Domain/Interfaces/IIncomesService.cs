using DebtsCompass.Domain.Entities.Requests;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IIncomesService
    {
        Task<Guid> CreateIncome(CreateIncomeRequest createIncomeRequest, string creatorEmail, bool isRonCurrency = false);
        Task DeleteIncome(string id, string email);
        Task UpdateIncome(EditIncomeRequest editIncomeRequest, string email);
    }
}