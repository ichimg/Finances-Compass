namespace DebtsCompass.Application.Exceptions
{
    public class EmailAlreadyConfirmedException : Exception
    {
        public EmailAlreadyConfirmedException(string email) : base($"The e-mail is already confirmed. E-mail: {email}") { }
    }
}
