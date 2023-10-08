using DebtsCompass.Domain.DtoResponses;
using DebtsCompass.Domain.Entities;

namespace DebtsCompass.Domain
{
    public static class Mapper
    {
        public static DebtDto DebtAssignmentDbModelToDebtDto(DebtAssignment debtAssignment)
        {
            return new DebtDto
            {
                Name = $"{debtAssignment.SelectedUser.UserInfo.FirstName}  {debtAssignment.SelectedUser.UserInfo.LastName}",
                Email = debtAssignment.SelectedUser.Email,
                Amount = debtAssignment.Debt.Amount,
                BorrowingDate = debtAssignment.Debt.DateOfBorrowing,
                Deadline = debtAssignment.Debt.DeadlineDate,
                Reason = debtAssignment.Debt.BorrowReason
            };
        }
    }
}
