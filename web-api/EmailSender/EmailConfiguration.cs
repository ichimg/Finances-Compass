using System.Reflection;

namespace EmailSender
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ClientUri { get; set; }

        public static string GetEmailTemplatesFolderPath()
        {
            string currentDirectory = Environment.CurrentDirectory;
            DirectoryInfo currentDirectoryInfo = new DirectoryInfo(currentDirectory);
            DirectoryInfo parentDirectoryInfo = currentDirectoryInfo.Parent;

            return $@"{parentDirectoryInfo}\EmailSender\EmailTemplates";
        }
    }
}
