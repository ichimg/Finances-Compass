namespace DebtsCompass.Application.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        const string DefaultMessage = "The entered credentials are invalid.";
        public InvalidCredentialsException() : base(DefaultMessage) { }
    }
}
