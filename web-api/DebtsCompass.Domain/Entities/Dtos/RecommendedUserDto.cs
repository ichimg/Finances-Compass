namespace DebtsCompass.Domain.Entities.Dtos
{
    public class RecommendedUserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public double[] UserVector { get; set; }
    }
}
