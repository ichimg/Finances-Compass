namespace DebtsCompass.Application.Exceptions
{
    public class InvalidPasswordException : Exception
    {
        const string DefaultMessage = "The provided password doesn't meet the minimum requirements.";
        public InvalidPasswordException() : base(DefaultMessage) { }
    }
}
