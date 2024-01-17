using DebtsCompass.Domain.Entities.EmailDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
    public class EmailTemplatesService
    {
        public string GetEmailConfirmationHtml(string callback)
        {
            string html = ReadHtmlTemplate(@"EmailConfirmationEmailTemplate.html");
            html = html.Replace("{link}", callback);
            return html;
        }

        public string GetNoAccountDebtCreatedHtml(DebtEmailInfoDto createdDebtEmailInfoDto)
        {
            string html = ReadHtmlTemplate(@"NoAccountDebtCreatedEmailTemplate.html");
            html = html.Replace("{creatorName}", $"{createdDebtEmailInfoDto.CreatorFirstName} {createdDebtEmailInfoDto.CreatorLastName}");
            return html;
        }

        public string GetDebtCreatedHtml(DebtEmailInfoDto createdDebtEmailInfoDto)
        {
            string html = ReadHtmlTemplate(@"DebtCreatedEmailTemplate.html");
            html = html.Replace("{creatorName}", $"{createdDebtEmailInfoDto.CreatorFirstName} {createdDebtEmailInfoDto.CreatorLastName}");
            html = html.Replace("{amount}", $"{createdDebtEmailInfoDto.Amount}");
            html = html.Replace("{reason}", $"{createdDebtEmailInfoDto.Reason}");
            html = html.Replace("{currency}", $"{createdDebtEmailInfoDto.Currency}");
            html = html.Replace("{dateOfBorrowing}", $"{createdDebtEmailInfoDto.DateOfBorrowing}");
            html = html.Replace("{deadline}", $"{createdDebtEmailInfoDto.Deadline}");

            return html;
        }

        public string GetDebtDeletedHtml(DebtEmailInfoDto createdDebtEmailInfoDto)
        {
            string html = ReadHtmlTemplate(@"DebtDeletedEmailTemplate.html");
            html = html.Replace("{creatorName}", $"{createdDebtEmailInfoDto.CreatorFirstName} {createdDebtEmailInfoDto.CreatorLastName}");
            html = html.Replace("{amount}", $"{createdDebtEmailInfoDto.Amount}");
            html = html.Replace("{reason}", $"{createdDebtEmailInfoDto.Reason}");
            html = html.Replace("{currency}", $"{createdDebtEmailInfoDto.Currency}");
            html = html.Replace("{dateOfBorrowing}", $"{createdDebtEmailInfoDto.DateOfBorrowing}");
            html = html.Replace("{deadline}", $"{createdDebtEmailInfoDto.Deadline}");

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
