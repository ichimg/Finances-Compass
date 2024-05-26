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
                Amount = Math.Round(debtAssignment.Debt.Amount, 2),
                BorrowingDate = debtAssignment.Debt.DateOfBorrowing,
                Deadline = debtAssignment.Debt.DeadlineDate,
                Reason = debtAssignment.Debt.BorrowReason,
                Status = debtAssignment.Debt.Status.ToString(),
                IsPaid = debtAssignment.Debt.IsPaid,
                IsUserAccount = debtAssignment.SelectedUser != null,
                EurExchangeRate = debtAssignment.Debt.EurExchangeRate ?? 0.0m,
                UsdExchangeRate = debtAssignment.Debt.UsdExchangeRate ?? 0.0m
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
                Amount = Math.Round(debtAssignment.Debt.Amount, 2),
                BorrowingDate = debtAssignment.Debt.DateOfBorrowing,
                Deadline = debtAssignment.Debt.DeadlineDate,
                Reason = debtAssignment.Debt.BorrowReason,
                Status = debtAssignment.Debt.Status.ToString(),
                IsPaid = debtAssignment.Debt.IsPaid,
                IsUserAccount = debtAssignment.SelectedUser != null,
                EurExchangeRate = debtAssignment.Debt.EurExchangeRate ?? 0.0m,
                UsdExchangeRate = debtAssignment.Debt.UsdExchangeRate ?? 0.0m
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
                UserName = registerRequest.Username,
                CurrencyPreference = Enum.Parse<CurrencyPreference>(registerRequest.CurrencyPreference),
                RegisteredDate = DateTime.UtcNow.Date,
                DashboardSelectedYear = DateTime.UtcNow.Date.Year,
                IsDataConsent = registerRequest.IsDataConsent
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

        public static UserDto UserToUserDto(User user, Status friendStatus, bool isPendingFriendRequest = false)
        {
            return new UserDto
            {
                FirstName = user.UserInfo.FirstName,
                LastName = user.UserInfo.LastName,
                Username = user.UserName,
                Email = user.Email,
                FriendStatus = friendStatus.ToString(),
                IsPendingFriendRequest = isPendingFriendRequest
            };
        }

        public static DebtAssignment CreateDebtRequestToDebtAssignment(CreateDebtRequest createDebtRequest, User creatorUser,
            User selectedUser, CurrencyDto currentCurrencies)
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
                        IsPaid = createDebtRequest.IsPaid,
                        EurExchangeRate = currentCurrencies.EurExchangeRate,
                        UsdExchangeRate = currentCurrencies.UsdExchangeRate
                    }
                };
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public static DebtAssignment CreateDebtRequestToDebtAssignment(CreateDebtRequest createDebtRequest, User creatorUser,
            NonUser selectedUser, CurrencyDto currentCurrencies)
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
                        IsPaid = createDebtRequest.IsPaid,
                        EurExchangeRate = currentCurrencies.EurExchangeRate,
                        UsdExchangeRate = currentCurrencies.UsdExchangeRate
                    }
                };
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public static DebtAssignment CreateDebtRequestToDebtAssignment(CreateDebtRequest createDebtRequest, User creatorUser, CurrencyDto currentCurrencies)
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
                        IsPaid = createDebtRequest.IsPaid,
                        EurExchangeRate = currentCurrencies.EurExchangeRate,
                        UsdExchangeRate = currentCurrencies.UsdExchangeRate
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

        public static DebtEmailInfoDto UserToDebtEmailInfoDto(User user)
        {
            return new DebtEmailInfoDto
            {
                CreatorFirstName = user.UserInfo.FirstName,
                CreatorLastName = user.UserInfo.LastName
            };
        }

        public static DebtEmailInfoDto DebtAssignmentToDebtEmailInfoDto(DebtAssignment debtAssignment)
        {
            return new DebtEmailInfoDto
            {
                CreatorFirstName = debtAssignment.CreatorUser.UserInfo.FirstName,
                CreatorLastName = debtAssignment.CreatorUser.UserInfo.LastName,
                Amount = debtAssignment.SelectedUser.CurrencyPreference == CurrencyPreference.EUR ?
                (debtAssignment.Debt.Amount * (decimal)debtAssignment.Debt.EurExchangeRate).ToString("#.##") :
                debtAssignment.SelectedUser.CurrencyPreference == CurrencyPreference.USD ?
                (debtAssignment.Debt.Amount * (decimal)debtAssignment.Debt.UsdExchangeRate).ToString("#.##") :
                debtAssignment.Debt.Amount.ToString("#.##"),
                Reason = debtAssignment.Debt.BorrowReason,
                Currency = debtAssignment.SelectedUser.CurrencyPreference.ToString(),
                DateOfBorrowing = debtAssignment.Debt.DateOfBorrowing.ToString("dd MMM yyyy"),
                Deadline = debtAssignment.Debt.DeadlineDate.ToString("dd MMM yyyy")
            };
        }

        public static DebtEmailInfoDto NoAccountDebtAssignmentToDebtEmailInfoDto(DebtAssignment debtAssignment)
        {
            return new DebtEmailInfoDto
            {
                CreatorFirstName = debtAssignment.CreatorUser.UserInfo.FirstName,
                CreatorLastName = debtAssignment.CreatorUser.UserInfo.LastName,
                Amount = debtAssignment.Debt.Amount.ToString("#.##"),
                Reason = debtAssignment.Debt.BorrowReason,
                Currency = CurrencyPreference.RON.ToString(),
                DateOfBorrowing = debtAssignment.Debt.DateOfBorrowing.ToString("dd MMM yyyy"),
                Deadline = debtAssignment.Debt.DeadlineDate.ToString("dd MMM yyyy")
            };
        }

        public static Friendship FriendRequestToFriendship(FriendRequest friendRequest, User userOne, User userTwo)
        {
            return new Friendship
            {
                RequesterUser = userOne,
                SelectedUser = userTwo,
                Status = Enum.Parse<Status>(friendRequest.Status)
            };
        }

        public static Expense CreateExpenseRequestToExpense(CreateExpenseRequest createExpenseRequest, User user,
            CurrencyDto currentCurrencies, ExpenseCategory category)
        {
            return new Expense
            {
                Amount = createExpenseRequest.Amount,
                Date = DateTime.Parse(createExpenseRequest.Date),
                Category = category,
                Note = createExpenseRequest.Note,
                User = user,
                EurExchangeRate = currentCurrencies.EurExchangeRate,
                UsdExchangeRate = currentCurrencies.UsdExchangeRate
            };
        }

        public static CategoryDto ExpenseCategoryToCategoryDto(ExpenseCategory expenseCategory)
        {
            return new CategoryDto
            {
                Name = expenseCategory.Name
            };
        }

        public static CategoryDto IncomeCategoryToCategoryDto(IncomeCategory incomeCategory)
        {
            return new CategoryDto
            {
                Name = incomeCategory.Name
            };
        }

        public static Expense EditExpenseRequestToExpense(EditExpenseRequest editExpenseRequest, ExpenseCategory category)
        {
            return new Expense
            {
                Amount = editExpenseRequest.Amount,
                Category = category,
                Note = editExpenseRequest.Note,
            };
        }

        public static Income CreateIncomeRequestToIncome(CreateIncomeRequest createIncomeRequest, User user, CurrencyDto currentCurrencies, IncomeCategory category)
        {
            return new Income
            {
                Amount = createIncomeRequest.Amount,
                Date = DateTime.Parse(createIncomeRequest.Date),
                Category = category,
                Note = createIncomeRequest.Note,
                User = user,
                EurExchangeRate = currentCurrencies.EurExchangeRate,
                UsdExchangeRate = currentCurrencies.UsdExchangeRate
            };
        }

        public static Income EditIncomeRequestToIncome(EditIncomeRequest editIncomeRequest, IncomeCategory category)
        {
            return new Income
            {
                Amount = editIncomeRequest.Amount,
                Category = category,
                Note = editIncomeRequest.Note,
            };
        }

        public static ExpenseOrIncomeDto ExpenseToExpenseOrIncomeDto(Expense expense)
        {
            return new ExpenseOrIncomeDto
            {
                Id = expense.Id.ToString(),
                Amount = Math.Round(expense.Amount, 2),
                Date = expense.Date,
                Category = expense.Category.Name,
                Note = expense.Note,
                IsExpense = true
            };
        }

        public static ExpenseOrIncomeDto IncomeToExpenseOrIncomeDto(Income income)
        {
            return new ExpenseOrIncomeDto
            {
                Id = income.Id.ToString(),
                Amount = Math.Round(income.Amount, 2),
                Date = income.Date,
                Category = income.Category.Name,
                Note = income.Note,
                IsExpense = false
            };
        }

        public static LoanEmailInfoDto DebtAssignmentToLoanEmailInfoDto(DebtAssignment debtAssignment)
        {
            return new LoanEmailInfoDto
            {
                SelectedUserFirstName = debtAssignment.SelectedUser != null ?
                debtAssignment.SelectedUser.UserInfo.FirstName :
                debtAssignment.NonUser.PersonFirstName,
                SelectedUserLastName = debtAssignment.SelectedUser != null ?
                debtAssignment.SelectedUser.UserInfo.LastName :
                debtAssignment.NonUser.PersonLastName,
                Reason = debtAssignment.Debt.BorrowReason,
                Currency = debtAssignment.CreatorUser.CurrencyPreference.ToString(),
                DateOfBorrowing = debtAssignment.Debt.DateOfBorrowing.ToString("dd MMM yyyy"),
                Deadline = debtAssignment.Debt.DeadlineDate.ToString("dd MMM yyyy"),
                Amount = debtAssignment.CreatorUser.CurrencyPreference == CurrencyPreference.EUR ?
                (debtAssignment.Debt.Amount * (decimal)debtAssignment.Debt.EurExchangeRate).ToString("#.##") :
                debtAssignment.CreatorUser.CurrencyPreference == CurrencyPreference.USD ?
                (debtAssignment.Debt.Amount * (decimal)debtAssignment.Debt.UsdExchangeRate).ToString("#.##") :
                debtAssignment.Debt.Amount.ToString("#.##"),
            };
        }

        public static YearsDto UserToYearsDto(User user)
        {
            return new YearsDto
            {
                RegisteredYear = user.RegisteredDate.Year,
                DashboardSelectedYear = user.DashboardSelectedYear
            };
        }

        public static RecommendedUserDto UserToRecommendedUserDto(User user, double[] userVector, double cosineSimilarity)
        {
            return new RecommendedUserDto 
            { 
                Id = user.Id,
                FirstName = user.UserInfo.FirstName,
                LastName = user.UserInfo.LastName,
                Username = user.UserName,
                Email = user.Email,
                UserVector = userVector,
                CosineSimilarity = cosineSimilarity
            };
        }

        public static UserDto RecommendedUserDtoToUserDto(RecommendedUserDto recommendedUserDto, Status friendStatus, bool isPendingFriendRequest = false)
        {
            return new UserDto
            {
                FirstName = recommendedUserDto.FirstName,
                LastName = recommendedUserDto.LastName,
                Username = recommendedUserDto.Username,
                Email = recommendedUserDto.Email,
                FriendStatus = friendStatus.ToString(),
                IsPendingFriendRequest = isPendingFriendRequest
            };
        }
    }
}
