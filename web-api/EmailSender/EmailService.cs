using DebtsCompass.Domain.Entities.Dtos;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System.Reflection;

namespace EmailSender
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration emailConfiguration;
        private readonly InternetAddress internetAddress;

        public EmailService(EmailConfiguration emailConfiguration)
        {
            this.emailConfiguration = emailConfiguration;
            internetAddress = MailboxAddress.Parse(emailConfiguration.From);
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
            string html = GetEmailConfirmationHtml(callback);
            await SendEmail(receiverInfoDto, html, "E-mail confirmation");
        }

        private string ReadHtmlTemplate(string templatePath)
        {
            string emailTemplatesFolderPath = EmailConfiguration.GetEmailTemplatesFolderPath();
            string templateFullPath = $@"{emailTemplatesFolderPath}\{templatePath}";
            return File.ReadAllText(templateFullPath);
        }

        private string GetEmailConfirmationHtml(string callback)
        {
            string html = ReadHtmlTemplate(@"EmailConfirmationEmailTemplate.html");
            html = html.Replace("{link}", callback);
            return html;
        }
    }
}
