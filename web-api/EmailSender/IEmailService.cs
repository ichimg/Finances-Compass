using DebtsCompass.Domain.Entities.Dtos;

namespace EmailSender
{
    public interface IEmailService
    {
        Task SendEmailConfirmationNotification(ReceiverInfoDto receiverInfoDto, string callback);
    }
}