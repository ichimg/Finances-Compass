using DebtsCompass.Domain.DtoResponses;
using DebtsCompass.Domain.Entities;
using DebtsCompass.Domain.Requests;

namespace DebtsCompass.Domain
{
    public static class Mapper
    {
        public static DebtDto ReceivingDebtAssignmentDbModelToDebtDto(DebtAssignment debtAssignment)
        {
            return new DebtDto
            {
                Name = debtAssignment.SelectedUser != null ? 
                $"{debtAssignment.SelectedUser.UserInfo.FirstName}  {debtAssignment.SelectedUser.UserInfo.LastName}" : 
                $"{debtAssignment.NonUserDebtAssignment.PersonFirstName} {debtAssignment.NonUserDebtAssignment.PersonLastName}",
                Email = debtAssignment.SelectedUser != null ? debtAssignment.SelectedUser.Email : debtAssignment.NonUserDebtAssignment.PersonEmail,
                Amount = debtAssignment.Debt.Amount,
                BorrowingDate = debtAssignment.Debt.DateOfBorrowing,
                Deadline = debtAssignment.Debt.DeadlineDate,
                Reason = debtAssignment.Debt.BorrowReason,
                IsUserAccount = debtAssignment.NonUserDebtAssignment != null
            };
        }

        public static DebtDto UserDebtAssignmentDbModelToDebtDto(DebtAssignment debtAssignment)
        {
            return new DebtDto
            {
                Name = $"{debtAssignment.CreatorUser.UserInfo.FirstName} {debtAssignment.CreatorUser.UserInfo.LastName}",
                Email = debtAssignment.CreatorUser.Email,
                Amount = debtAssignment.Debt.Amount,
                BorrowingDate = debtAssignment.Debt.DateOfBorrowing,
                Deadline = debtAssignment.Debt.DeadlineDate,
                Reason = debtAssignment.Debt.BorrowReason,
                IsUserAccount = debtAssignment.NonUserDebtAssignment != null
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
