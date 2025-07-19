using DebtsCompass.Domain.Entities.Dtos;
using DebtsCompass.Domain.Entities.EmailDtos;
using DebtsCompass.Domain.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace EmailSender
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration emailConfiguration;
        private readonly EmailTemplatesService emailTemplatesService;
        private readonly InternetAddress internetAddress;

        public EmailService(EmailConfiguration emailConfiguration, EmailTemplatesService emailTemplatesService)
        {
            this.emailConfiguration = emailConfiguration;
            this.emailTemplatesService = emailTemplatesService;
            internetAddress = MailboxAddress.Parse(emailConfiguration.From);
            internetAddress.Name = "Finances Compass";
        }

        private async Task SendEmail(ReceiverInfoDto receiverInfoDto, string htmlEmailBody, string subject)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(internetAddress);
            email.To.Add(new MailboxAddress(receiverInfoDto.Firstname, receiverInfoDto.Email));
            email.Body = new TextPart(TextFormat.Html) { Text = htmlEmailBody };
            email.Subject = subject;

            await SendEmailAsync(email);
        }

        private async Task SendEmailAsync(MimeMessage email)
        {
            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(emailConfiguration.SmtpServer, emailConfiguration.Port, true);
            await smtp.AuthenticateAsync(emailConfiguration.UserName, emailConfiguration.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendEmailConfirmationNotification(ReceiverInfoDto receiverInfoDto, string callback)
        {
            string html = emailTemplatesService.GetEmailConfirmationHtml(callback);
            await SendEmail(receiverInfoDto, html, "E-mail confirmation");
        }

        public async Task SendNoAccountDebtCreatedNotification(ReceiverInfoDto receiverInfoDto, DebtEmailInfoDto debtEmailInfoDto)
        {
            string html = emailTemplatesService.GetNoAccountDebtCreatedHtml(debtEmailInfoDto);
            await SendEmail(receiverInfoDto, html, "Join Finances Compass to see what you owe to your friends");
        }

        public async Task SendDebtCreatedNotification(ReceiverInfoDto receiverInfoDto, DebtEmailInfoDto debtEmailInfoDto)
        {
            string html = emailTemplatesService.GetDebtCreatedHtml(debtEmailInfoDto);
            await SendEmail(receiverInfoDto, html, "Check new debt added");
        }

        public async Task SendDebtDeletedNotification(ReceiverInfoDto receiverInfoDto, DebtEmailInfoDto debtEmailInfoDto)
        {
            string html = emailTemplatesService.GetDebtDeletedHtml(debtEmailInfoDto);
            await SendEmail(receiverInfoDto, html, "A debt just got deleted");
        }

        public async Task SendDeadlineEmails(ReceiverInfoDto receiverInfoDto, DebtEmailInfoDto debtEmailInfoDto, ReceiverInfoDto creatorUserReceiverInfoDto, LoanEmailInfoDto loanEmailInfoDto)
        {
            await SendDebtDeadlineReminderNotification(receiverInfoDto, debtEmailInfoDto);
            await SendLoanDeadlineReminderNotification(creatorUserReceiverInfoDto, loanEmailInfoDto);
        }

        private async Task SendDebtDeadlineReminderNotification(ReceiverInfoDto receiverInfoDto, DebtEmailInfoDto debtEmailInfoDto)
        {
            string html = emailTemplatesService.GetDebtDeadlineReminderHtml(debtEmailInfoDto);
            await SendEmail(receiverInfoDto, html, "A debt is due soon");
        }

        private async Task SendLoanDeadlineReminderNotification(ReceiverInfoDto receiverInfoDto, LoanEmailInfoDto loanEmailInfoDto)
        {
            string html = emailTemplatesService.GetLoanDeadlineReminderHtml(loanEmailInfoDto, receiverInfoDto);
            await SendEmail(receiverInfoDto, html, "A loan is due soon");
        }
    }
}
