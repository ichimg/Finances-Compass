namespace DebtsCompass.DataAccess.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(int id) : base($"Entity with ID '{id}' was not found.") { }
        public EntityNotFoundException(string email) : base($"Entity with e-mail '{email}' was not found.") { }

    }
}
