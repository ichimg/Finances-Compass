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
                Guid = debtAssignment.Id.ToString(),
                FirstName = debtAssignment.SelectedUser != null ?
                debtAssignment.SelectedUser.UserInfo.FirstName :
                debtAssignment.NonUser.PersonFirstName,

                LastName = debtAssignment.SelectedUser != null ?
                debtAssignment.SelectedUser.UserInfo.LastName :
                debtAssignment.NonUser.PersonLastName,

                Email = debtAssignment.SelectedUser != null ? debtAssignment.SelectedUser.Email : debtAssignment.NonUser.PersonEmail,
                Username = debtAssignment.SelectedUser != null ? debtAssignment.SelectedUser.UserName : null,
                Amount = debtAssignment.Debt.Amount,
                BorrowingDate = debtAssignment.Debt.DateOfBorrowing,
                Deadline = debtAssignment.Debt.DeadlineDate,
                Reason = debtAssignment.Debt.BorrowReason,
                Status = debtAssignment.Debt.Status.ToString(),
                IsPaid = debtAssignment.Debt.IsPaid,
                IsUserAccount = debtAssignment.SelectedUser != null
            };
        }

        public static DebtDto UserDebtAssignmentDbModelToDebtDto(DebtAssignment debtAssignment)
        {
            return new DebtDto
            {
                Guid = debtAssignment.Id.ToString(),
                FirstName = debtAssignment.CreatorUser.UserInfo.FirstName,
                LastName = debtAssignment.CreatorUser.UserInfo.LastName,
                Username = debtAssignment.CreatorUser.UserName,
                Email = debtAssignment.CreatorUser.Email,
                Amount = debtAssignment.Debt.Amount,
                BorrowingDate = debtAssignment.Debt.DateOfBorrowing,
                Deadline = debtAssignment.Debt.DeadlineDate,
                Reason = debtAssignment.Debt.BorrowReason,
                Status = debtAssignment.Debt.Status.ToString(),
                IsPaid = debtAssignment.Debt.IsPaid,
                IsUserAccount = debtAssignment.SelectedUser != null
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
                        StreetAddress = registerRequest.StreetAddress
                    },
                },
                Email = registerRequest.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
                PhoneNumber = registerRequest.PhoneNumber,
                UserName = registerRequest.Username
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

        public static UserDto UserToUserDto(User user, Status friendStatus)
        {
            return new UserDto
            {
                FirstName = user.UserInfo.FirstName,
                LastName = user.UserInfo.LastName,
                Username = user.UserName,
                Email = user.Email,
                FriendStatus = friendStatus.ToString()
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

        public static DebtAssignment EditDebtRequestToDebtAssignment(EditDebtRequest editDebtRequest, User selectedUser)
        {
            return new DebtAssignment
            {
                SelectedUser = selectedUser,
                Debt = new Debt
                {
                    Amount = editDebtRequest.Amount,
                    BorrowReason = editDebtRequest.Reason,
                    DateOfBorrowing = DateTime.Parse(editDebtRequest.BorrowingDate),
                    DeadlineDate = DateTime.Parse(editDebtRequest.Deadline),
                }
            };
        }

        public static DebtAssignment EditDebtRequestToDebtAssignment(EditDebtRequest editDebtRequest, NonUser selectedUser)
        {
            return new DebtAssignment
            {
                NonUser = selectedUser,
                Debt = new Debt
                {
                    Amount = editDebtRequest.Amount,
                    BorrowReason = editDebtRequest.Reason,
                    DateOfBorrowing = DateTime.Parse(editDebtRequest.BorrowingDate),
                    DeadlineDate = DateTime.Parse(editDebtRequest.Deadline),
                }
            };
        }

        public static DebtEmailInfoDto UserToCreatedDebtEmailInfoDto(User user)
        {
            return new DebtEmailInfoDto
            {
                CreatorFirstName = user.UserInfo.FirstName,
                CreatorLastName = user.UserInfo.LastName
            };
        }

        public static DebtEmailInfoDto DebtAssignmentToCreatedDebtEmailInfoDto(DebtAssignment debtAssignment)
        {
            return new DebtEmailInfoDto
            {
                CreatorFirstName = debtAssignment.CreatorUser.UserInfo.FirstName,
                CreatorLastName = debtAssignment.CreatorUser.UserInfo.LastName,
                Amount = debtAssignment.Debt.Amount.ToString("#.##"),
                Reason = debtAssignment.Debt.BorrowReason,
                Currency = "RON", // for now hardcoded..
                DateOfBorrowing = debtAssignment.Debt.DateOfBorrowing.ToString("dd MMM yyyy"),
                Deadline = debtAssignment.Debt.DeadlineDate.ToString("dd MMM yyyy")
            };
        }

        public static Friendship FriendRequestToFriendship(FriendRequest friendRequest, User userOne, User userTwo)
        {
            return new Friendship
            {
                UserOne = userOne,
                UserTwo = userTwo,
                Status = Enum.Parse<Status>(friendRequest.Status)
            };
        }
    }
}
