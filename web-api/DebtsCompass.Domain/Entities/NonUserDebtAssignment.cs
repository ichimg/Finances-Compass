namespace DebtsCompass.Domain.Entities
{
    public class NonUserDebtAssignment
    {
        public Guid Id { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string PersonEmail { get; set; }
    }
}
