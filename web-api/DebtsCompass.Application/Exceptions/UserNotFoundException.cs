namespace DebtsCompass.Application.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string email) : base($"User with e-mail '{email}' was not found.") { }

    }
}
