namespace DebtsCompass.Domain.Entities.EmailDtos
{
    public class DebtEmailInfoDto
    {
        public string CreatorFirstName { get; set; }
        public string CreatorLastName { get; set; }
        public string Amount { get; set; }
        public string Reason { get; set; }
        public string Currency { get; set; }
        public string DateOfBorrowing { get; set; }
        public string Deadline { get; set; }
    }
}
