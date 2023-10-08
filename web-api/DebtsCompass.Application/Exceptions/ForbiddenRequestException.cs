namespace DebtsCompass.Application.Exceptions
{
    public class ForbiddenRequestException : Exception
    {
        const string DefaultMessage = "You don't have the permission to access this resource.";
        public ForbiddenRequestException() : base(DefaultMessage) { }
    }
}
