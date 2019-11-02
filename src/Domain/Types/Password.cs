namespace SaltedPasswordHashing.Src.Domain.Types
{
    public class Password
    {
        public string Value { get; }
        
        private Password(string value)
        {
            Value = value;
        }
        
        public static ValidationResult<Password> Create(string value)
        {
            if(!IsValidPassword(password: value)){
                return ValidationResult<Password>.CreateInvalidResult(error: Error.InvalidFormat);
            }
            Password password = new Password(value: value);
            return ValidationResult<Password>.CreateValidResult(result: password);
        }

        private static bool IsValidPassword(string password)
        {
            const int MIN_ALLOWED_PASSWORD_LENGHT = 8;
            return password.Length >= MIN_ALLOWED_PASSWORD_LENGHT;
        }
    }
}