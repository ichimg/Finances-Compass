using System.Text.RegularExpressions;

namespace DebtsCompass.Application.Validators
{
    public class EmailValidator
    {
        public EmailValidator() { }

        public bool IsValid(string email)
        {
            var regex = new Regex(@"^([0-9a-zA-Z]([-\.\'\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
            return !string.IsNullOrEmpty(email) && regex.IsMatch(email);
        }
    }
}
