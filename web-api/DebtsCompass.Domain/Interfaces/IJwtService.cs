namespace DebtsCompass.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string email);
    }
}