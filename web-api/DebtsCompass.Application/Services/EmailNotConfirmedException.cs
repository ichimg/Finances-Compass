
namespace DebtsCompass.Application.Services
{
    public class EmailNotConfirmedException : Exception
    {
        public EmailNotConfirmedException(string email) : base($"The provided e-mail is not confirmed. E-mail: {email}") { }
    }
}
