using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.TotalList;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IExpensesService
    {
        Task<Guid> CreateExpense(CreateExpenseRequest createExpenseRequest, string creatorEmail, bool isRonCurrency = false);
        Task DeleteExpense(string id, string email);
        Task EditExpense(EditExpenseRequest editExpenseRequest, string email);
        Task<TotalList<ExpenseOrIncomeDto>> GetAllByEmail(string email, YearMonthDto yearMonthDto);
        Task<TotalExpensesAndIncomesDto> GetExpensesAndIncomesTotalCount(string email);
    }
}