namespace SaltedPasswordHashing.Src.Domain.Types
{
    public sealed class Email
    {
        public string Value { get; }
        
        private Email(string value)
        {
            Value = value;
        }
        
        public static ValidationResult<Email> Create(string value)
        {
            if(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                return ValidationResult<Email>.CreateInvalidResult(error: Error.Required);
            }
            if(!IsValidEmail(email: value))
            {
                return ValidationResult<Email>.CreateInvalidResult(error: Error.InvalidFormat);
            }
            Email email = new Email(value: value);
            return ValidationResult<Email>.CreateValidResult(result: email);
        }

        private static bool IsValidEmail(string email)
        {
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch {
                return false;
            }
        }
    }
}