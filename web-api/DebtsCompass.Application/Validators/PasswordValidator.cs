using System;
using System.Text.RegularExpressions;

namespace DebtsCompass.Application.Validators
{
    public class PasswordValidator
    {
        public PasswordValidator() { }

        public bool IsValid(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            return !string.IsNullOrEmpty(password) && hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password);
        }
    }
}
