namespace DebtsCompass.Application.Configurations
{
    public class PaypalConfiguration
    {
        public string Mode { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SandboxURL { get; set; }
        public string LiveURL { get; set; }
    }
}
