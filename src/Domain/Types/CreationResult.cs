using System;

namespace SaltedPasswordHashing.Src.Domain.Types
{
    public sealed class CreationResult<T> where T : class
    {
        public T Result { get; }
        public Error? Error { get; }
        public bool IsValid { get; }

        private CreationResult(T result, Error? error, bool isValid)
        {
            Result = result;
            Error = error;
            IsValid = isValid;
        }

        public static CreationResult<T> CreateValidResult(T result)
        {
            if(result == null){
                throw new ArgumentNullException();
            }
            return new CreationResult<T>(
                result: result,
                error: null,
                isValid: true);
        }

        public static CreationResult<T> CreateInvalidResult(Error error)
        {
            return new CreationResult<T>(
                result: null,
                error: error,
                isValid: false);
        }
    }
}