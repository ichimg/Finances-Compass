using DebtsCompass.Domain.Entities;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Dtos;
using DebtsCompass.Domain.Entities.EmailDtos;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Enums;

namespace DebtsCompass.Domain
{
    public static class Mapper
    {
        public static DebtDto ReceivingDebtAssignmentDbModelToDebtDto(DebtAssignment debtAssignment)
        {
            return new DebtDto
            {
                FirstName = debtAssignment.SelectedUser != null ?
                debtAssignment.SelectedUser.UserInfo.FirstName :
                debtAssignment.NonUser.PersonFirstName,

                LastName = debtAssignment.SelectedUser != null ?
                debtAssignment.SelectedUser.UserInfo.LastName :
                debtAssignment.NonUser.PersonLastName,

                Email = debtAssignment.SelectedUser != null ? debtAssignment.SelectedUser.Email : debtAssignment.NonUser.PersonEmail,
                Amount = debtAssignment.Debt.Amount,
                BorrowingDate = debtAssignment.Debt.DateOfBorrowing,
                Deadline = debtAssignment.Debt.DeadlineDate,
                Reason = debtAssignment.Debt.BorrowReason,
                Status = debtAssignment.Debt.Status.ToString(),
                IsPaid = debtAssignment.Debt.IsPaid,
                IsUserAccount = debtAssignment.NonUser != null
            };
        }

        public static DebtDto UserDebtAssignmentDbModelToDebtDto(DebtAssignment debtAssignment)
        {
            return new DebtDto
            {
                FirstName = debtAssignment.CreatorUser.UserInfo.FirstName,
                LastName = debtAssignment.CreatorUser.UserInfo.LastName,
                Email = debtAssignment.CreatorUser.Email,
                Amount = debtAssignment.Debt.Amount,
                BorrowingDate = debtAssignment.Debt.DateOfBorrowing,
                Deadline = debtAssignment.Debt.DeadlineDate,
                Reason = debtAssignment.Debt.BorrowReason,
                Status = debtAssignment.Debt.Status.ToString(),
                IsPaid = debtAssignment.Debt.IsPaid,
                IsUserAccount = debtAssignment.NonUser != null
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
                PhoneNumber = registerRequest.PhoneNumber,
                UserName = registerRequest.Email
            };
        }

        public static ReceiverInfoDto UserToReceiverInfoDto(User user)
        {
            return new ReceiverInfoDto
            {
                Firstname = user.UserInfo.FirstName,
                Lastname = user.UserInfo.LastName,
                Email = user.Email
            };
        }

        public static ReceiverInfoDto NonUserToReceiverInfoDto(NonUser nonUser)
        {
            return new ReceiverInfoDto
            {
                Firstname = nonUser.PersonFirstName,
                Lastname = nonUser.PersonLastName,
                Email = nonUser.PersonEmail
            };
        }

        public static UserDto UserToUserDto(User user)
        {
            return new UserDto
            {
                FirstName = user.UserInfo.FirstName,
                LastName = user.UserInfo.LastName,
                Email = user.Email
            };
        }

        public static DebtAssignment CreateDebtRequestToDebtAssignment(CreateDebtRequest createDebtRequest, User creatorUser, User selectedUser)
        {
            if (Enum.TryParse(createDebtRequest.Status, out Status debtStatus))
            {
                return new DebtAssignment
                {
                    CreatorUser = creatorUser,
                    SelectedUser = selectedUser,
                    Debt = new Debt
                    {
                        Amount = createDebtRequest.Amount,
                        BorrowReason = createDebtRequest.Reason,
                        DateOfBorrowing = DateTime.Parse(createDebtRequest.BorrowingDate),
                        DeadlineDate = DateTime.Parse(createDebtRequest.Deadline),
                        Status = debtStatus,
                        IsPaid = createDebtRequest.IsPaid
                    }
                };
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public static DebtAssignment CreateDebtRequestToDebtAssignment(CreateDebtRequest createDebtRequest, User creatorUser, NonUser selectedUser)
        {
            if (Enum.TryParse(createDebtRequest.Status, out Status debtStatus))
            {
                return new DebtAssignment
                {
                    CreatorUser = creatorUser,
                    NonUser = selectedUser,
                    Debt = new Debt
                    {
                        Amount = createDebtRequest.Amount,
                        BorrowReason = createDebtRequest.Reason,
                        DateOfBorrowing = DateTime.Parse(createDebtRequest.BorrowingDate),
                        DeadlineDate = DateTime.Parse(createDebtRequest.Deadline),
                        Status = debtStatus,
                        IsPaid = createDebtRequest.IsPaid
                    }
                };
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public static DebtAssignment CreateDebtRequestToDebtAssignment(CreateDebtRequest createDebtRequest, User creatorUser)
        {
            if (Enum.TryParse(createDebtRequest.Status, out Status debtStatus))
            {
                return new DebtAssignment
                {
                    CreatorUser = creatorUser,
                    NonUser = new NonUser
                    {
                        PersonFirstName = createDebtRequest.FirstName,
                        PersonLastName = createDebtRequest.LastName,
                        PersonEmail = createDebtRequest.Email,
                    },
                    Debt = new Debt
                    {
                        Amount = createDebtRequest.Amount,
                        BorrowReason = createDebtRequest.Reason,
                        DateOfBorrowing = DateTime.Parse(createDebtRequest.BorrowingDate),
                        DeadlineDate = DateTime.Parse(createDebtRequest.Deadline),
                        Status = debtStatus,
                        IsPaid = createDebtRequest.IsPaid
                    }
                };
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public static CreatedDebtEmailInfoDto UserToCreatedDebtEmailInfoDto(User user)
        {
            return new CreatedDebtEmailInfoDto
            {
                CreatorFirstName = user.UserInfo.FirstName,
                CreatorLastName = user.UserInfo.LastName
            };
        }
    }
}
