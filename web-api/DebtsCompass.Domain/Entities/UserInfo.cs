namespace DebtsCompass.Domain.Entities
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public string Iban { get; set; }
    }
}
