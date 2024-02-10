namespace DebtsCompass.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() : base($"Bad Request, please try again.") { }
    }
}
