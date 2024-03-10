using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Requests;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IExpensesService
    {
        Task<Guid> CreateExpense(CreateExpenseRequest createExpenseRequest, string creatorEmail);
        Task DeleteExpense(string id, string email);
        Task EditExpense(EditExpenseRequest editExpenseRequest, string email);
        Task<List<ExpenseOrIncomeDto>> GetAllByEmail(string email);
    }
}