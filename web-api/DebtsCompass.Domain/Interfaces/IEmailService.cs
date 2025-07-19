using DebtsCompass.Domain.Entities.Dtos;
using DebtsCompass.Domain.Entities.EmailDtos;

namespace DebtsCompass.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailConfirmationNotification(ReceiverInfoDto receiverInfoDto, string callback);
        Task SendNoAccountDebtCreatedNotification(ReceiverInfoDto receiverInfoDto, DebtEmailInfoDto debtEmailInfoDto);
        Task SendDebtCreatedNotification(ReceiverInfoDto receiverInfoDto, DebtEmailInfoDto debtEmailInfoDto);
        Task SendDebtDeletedNotification(ReceiverInfoDto receiverInfoDto, DebtEmailInfoDto debtEmailInfoDto);
        Task SendDeadlineEmails(ReceiverInfoDto receiverInfoDto, DebtEmailInfoDto debtEmailInfoDto, ReceiverInfoDto creatorUserReceiverInfoDto, LoanEmailInfoDto loanEmailInfoDto);
    }
}