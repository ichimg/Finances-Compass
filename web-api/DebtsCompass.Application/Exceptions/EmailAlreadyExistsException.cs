namespace DebtsCompass.Application.Exceptions
{
    public class EmailAlreadyExistsException : Exception
    {
        const string DefaultMessage = "Account e-mail already exists";
        public EmailAlreadyExistsException() : base(DefaultMessage) { }
    }
}
