namespace DebtsCompass.Application.Exceptions
{
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException(string email) : base($"The provided e-mail is invalid: {email}") { }
    }
}
