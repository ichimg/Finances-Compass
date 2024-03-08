using DebtsCompass.Domain.Entities.Requests;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IExpensesService
    {
        Task<Guid> CreateExpense(CreateExpenseRequest createExpenseRequest, string creatorEmail);
    }
}