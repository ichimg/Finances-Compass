namespace DebtsCompass.Application.Exceptions
{
    public class UsernameAlreadyExistsException : Exception
    {
        const string DefaultMessage = "Username already exists";
        public UsernameAlreadyExistsException() : base(DefaultMessage) { }
    }
}

