namespace SaltedPasswordHashing.Src.Domain.Types
{
    public class ValidationResult<T> where T : class
    {
        public T Result { get; }
        public Error? Error { get; }
        public bool IsValid { get; }

        private ValidationResult(T result, Error? error, bool isValid)
        {
            Result = result;
            Error = error;
            IsValid = isValid;
        }

        public static ValidationResult<T> CreateValidResult(T result)
        {
            return new ValidationResult<T>(
                result: result,
                error: null,
                isValid: true);
        }

        public static ValidationResult<T> CreateInvalidResult(Error error)
        {
            return new ValidationResult<T>(
                result: null,
                error: error,
                isValid: false);
        }
    }
}