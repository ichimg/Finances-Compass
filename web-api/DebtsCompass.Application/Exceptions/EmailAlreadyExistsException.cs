namespace DebtsCompass.Application.Exceptions
{
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException(string email) : base($"The provided e-mail is already registered: {email}") { }
    }
}
