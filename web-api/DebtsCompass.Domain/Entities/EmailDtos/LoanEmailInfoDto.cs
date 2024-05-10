namespace DebtsCompass.Domain.Entities.EmailDtos
{
    public class LoanEmailInfoDto
    {
        public string SelectedUserFirstName { get; set; }
        public string SelectedUserLastName { get; set; }
        public string Amount { get; set; }
        public string Reason { get; set; }
        public string Currency { get; set; }
        public string DateOfBorrowing { get; set; }
        public string Deadline { get; set; }
    }
}
