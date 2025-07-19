namespace DebtsCompass.Domain.Entities.Models
{
    public class NonUser
    {
        public Guid Id { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string PersonEmail { get; set; }
    }
}
