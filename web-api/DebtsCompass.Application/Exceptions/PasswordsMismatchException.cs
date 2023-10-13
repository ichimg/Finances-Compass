namespace DebtsCompass.Application.Exceptions
{
    public class PasswordMismatchException : Exception
    {
        const string DefaultMessage = "The provided passwords doesn't match.";
        public PasswordMismatchException() : base(DefaultMessage) { }
    }
}
