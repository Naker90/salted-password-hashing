using System.Linq;

namespace SaltedPasswordHashing.Src.Domain.Types
{
    public sealed class Password
    {
        public string Value { get; }
        
        private Password(string value)
        {
            Value = value;
        }
        
        public static ValidationResult<Password> Create(string value)
        {
            if(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                return ValidationResult<Password>.CreateInvalidResult(error: Error.Required);
            }
            if(!IsValidPassword(password: value)){
                return ValidationResult<Password>.CreateInvalidResult(error: Error.InvalidFormat);
            }
            Password password = new Password(value: value);
            return ValidationResult<Password>.CreateValidResult(result: password);
        }

        private static bool IsValidPassword(string password)
        {
            const int MIN_ALLOWED_PASSWORD_LENGHT = 8;
            return password.Length >= MIN_ALLOWED_PASSWORD_LENGHT 
                    && IsAlphanumeric()
                    && ContainsAtLeastOfOneUpperCaseLetter()
                    && ContainsAtLeastOfOneSymbol();

            bool IsAlphanumeric()
            {
                return password.Any(char.IsNumber) && password.Any(char.IsLetter); 
            }

            bool ContainsAtLeastOfOneUpperCaseLetter(){
                return password.Any(char.IsUpper);
            }

            bool ContainsAtLeastOfOneSymbol(){
                return password.Any(char.IsSymbol);
            }
        }
    }
}