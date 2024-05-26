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

        public static string GetEmailTemplatesFolderPath()
        {
            string currentDirectory = Environment.CurrentDirectory;
            DirectoryInfo currentDirectoryInfo = new DirectoryInfo(currentDirectory);
            DirectoryInfo parentDirectoryInfo = currentDirectoryInfo.Parent;

            return $@"{parentDirectoryInfo}\EmailSender\EmailTemplates";
        }

        private static string FindSolutionPath(string assemblyLocation)
        {
            string directory = Path.GetDirectoryName(assemblyLocation);

            while (!string.IsNullOrEmpty(directory))
            {
                string[] solutionFiles = Directory.GetFiles(directory, "*.sln");

                if (solutionFiles.Any())
                {
                    return directory;
                }

                directory = Path.GetDirectoryName(directory);
            }

            return null;
        }

    }
}
