using DebtsCompass.Domain.DtoResponses;
using DebtsCompass.Domain.Entities;
using DebtsCompass.Domain.Requests;

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

        public static User RegisterRequestToUserDbModel(RegisterRequest registerRequest)
        {
            return new User
            {
                UserInfo = new UserInfo
                {
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    Address = new Address
                    {
                        Country = registerRequest.Country,
                        County = registerRequest.State,
                        City = registerRequest.City,
                        PostalCode = registerRequest.PostalCode,
                        StreetAddress = registerRequest.StreetAddress
                    },
                    Iban = registerRequest.Iban,
                },
                Email = registerRequest.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
                PhoneNumber = registerRequest.PhoneNumber
            };
        }
    }
}
