using DebtsCompass.Domain.Entities.Dtos;
using DebtsCompass.Domain.Entities.EmailDtos;

namespace EmailSender
{
    public interface IEmailService
    {
        Task SendEmailConfirmationNotification(ReceiverInfoDto receiverInfoDto, string callback);
        Task SendNoAccountDebtCreatedNotification(ReceiverInfoDto receiverInfoDto, DebtEmailInfoDto createdDebtEmailInfoDto);
        Task SendDebtCreatedNotification(ReceiverInfoDto receiverInfoDto, DebtEmailInfoDto createdDebtEmailInfoDto);
        Task SendDebtDeletedNotification(ReceiverInfoDto receiverInfoDto, DebtEmailInfoDto createdDebtEmailInfoDto);
    }
}