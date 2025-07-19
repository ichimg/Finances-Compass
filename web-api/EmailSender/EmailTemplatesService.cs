using DebtsCompass.Domain.Entities.Dtos;
using DebtsCompass.Domain.Entities.EmailDtos;

namespace EmailSender
{
    public class EmailTemplatesService
    {
        private readonly EmailConfiguration emailConfiguration;

        public EmailTemplatesService(EmailConfiguration emailConfiguration)
        {
            this.emailConfiguration = emailConfiguration;
        }

        public string GetEmailConfirmationHtml(string callback)
        {
            string html = ReadHtmlTemplate(@"EmailConfirmationEmailTemplate.html");
            html = html.Replace("{link}", callback);
            return html;
        }

        public string GetNoAccountDebtCreatedHtml(DebtEmailInfoDto debtEmailInfoDto)
        {
            string html = ReadHtmlTemplate(@"NoAccountDebtCreatedEmailTemplate.html");
            html = html.Replace("{creatorName}", $"{debtEmailInfoDto.CreatorFirstName} {debtEmailInfoDto.CreatorLastName}");
            html = html.Replace("{link}", emailConfiguration.ClientUri);
            return html;
        }

        public string GetDebtCreatedHtml(DebtEmailInfoDto debtEmailInfoDto)
        {
            string html = ReadHtmlTemplate(@"DebtCreatedEmailTemplate.html");
            html = html.Replace("{creatorName}", $"{debtEmailInfoDto.CreatorFirstName} {debtEmailInfoDto.CreatorLastName}");
            html = html.Replace("{amount}", $"{debtEmailInfoDto.Amount}");
            html = html.Replace("{reason}", debtEmailInfoDto.Reason == string.Empty ? "Not provided" : $"{debtEmailInfoDto.Reason}");
            html = html.Replace("{currency}", $"{debtEmailInfoDto.Currency}");
            html = html.Replace("{dateOfBorrowing}", $"{debtEmailInfoDto.DateOfBorrowing}");
            html = html.Replace("{deadline}", $"{debtEmailInfoDto.Deadline}");
            html = html.Replace("{link}", emailConfiguration.ClientUri);

            return html;
        }

        public string GetDebtDeletedHtml(DebtEmailInfoDto debtEmailInfoDto)
        {
            string html = ReadHtmlTemplate(@"DebtDeletedEmailTemplate.html");
            html = html.Replace("{creatorName}", $"{debtEmailInfoDto.CreatorFirstName} {debtEmailInfoDto.CreatorLastName}");
            html = html.Replace("{amount}", $"{debtEmailInfoDto.Amount}");
            html = html.Replace("{reason}", debtEmailInfoDto.Reason == string.Empty ? "Not provided" : $"{debtEmailInfoDto.Reason}");
            html = html.Replace("{currency}", $"{debtEmailInfoDto.Currency}");
            html = html.Replace("{dateOfBorrowing}", $"{debtEmailInfoDto.DateOfBorrowing}");
            html = html.Replace("{deadline}", $"{debtEmailInfoDto.Deadline}");
            html = html.Replace("{link}", emailConfiguration.ClientUri);

            return html;
        }

        public string GetDebtDeadlineReminderHtml(DebtEmailInfoDto debtEmailInfoDto)
        {
            string html = ReadHtmlTemplate(@"DebtDeadlineReminderEmailTemplate.html");
            html = html.Replace("{creatorName}", $"{debtEmailInfoDto.CreatorFirstName} {debtEmailInfoDto.CreatorLastName}");
            html = html.Replace("{amount}", $"{debtEmailInfoDto.Amount}");
            html = html.Replace("{reason}", debtEmailInfoDto.Reason == string.Empty ? "Not provided" : $"{debtEmailInfoDto.Reason}");
            html = html.Replace("{currency}", $"{debtEmailInfoDto.Currency}");
            html = html.Replace("{dateOfBorrowing}", $"{debtEmailInfoDto.DateOfBorrowing}");
            html = html.Replace("{deadline}", $"{debtEmailInfoDto.Deadline}");
            html = html.Replace("{link}", emailConfiguration.ClientUri);

            return html;
        }

        public string GetLoanDeadlineReminderHtml(LoanEmailInfoDto loanEmailInfoDto, ReceiverInfoDto receiverInfoDto)
        {
            string html = ReadHtmlTemplate(@"LoanDeadlineReminderEmailTemplate.html");
            html = html.Replace("{debtorName}", $"{loanEmailInfoDto.SelectedUserFirstName} {loanEmailInfoDto.SelectedUserLastName}");
            html = html.Replace("{amount}", $"{loanEmailInfoDto.Amount}");
            html = html.Replace("{reason}", loanEmailInfoDto.Reason == string.Empty ? "Not provided" : $"{loanEmailInfoDto.Reason}");
            html = html.Replace("{currency}", $"{loanEmailInfoDto.Currency}");
            html = html.Replace("{dateOfBorrowing}", $"{loanEmailInfoDto.DateOfBorrowing}");
            html = html.Replace("{deadline}", $"{loanEmailInfoDto.Deadline}");
            html = html.Replace("{link}", emailConfiguration.ClientUri);

            return html;
        }

        private string ReadHtmlTemplate(string templatePath)
        {
            string emailTemplatesFolderPath = EmailConfiguration.GetEmailTemplatesFolderPath();
            string templateFullPath = $@"{emailTemplatesFolderPath}\{templatePath}";
            return File.ReadAllText(templateFullPath);
        }
    }
}
