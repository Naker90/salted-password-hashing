using System;

namespace SaltedPasswordHashing.Src.Domain.Types
{
    public sealed class CreationResult<TResult, TError> 
        where TResult : class
        where TError : struct, IConvertible
    {
        public TResult Result { get; }
        public TError? Error { get; }
        public bool IsValid { get; }

        private CreationResult(TResult result, TError? error, bool isValid)
        {
            Result = result;
            Error = error;
            IsValid = isValid;
        }

        public static CreationResult<TResult, TError> CreateValidResult(TResult result)
        {
            if(result == null){
                throw new ArgumentNullException();
            }
            return new CreationResult<TResult, TError>(
                result: result,
                error: null,
                isValid: true);
        }

        public static CreationResult<TResult, TError> CreateInvalidResult(TError error)
        {
            return new CreationResult<TResult, TError>(
                result: null,
                error: error,
                isValid: false);
        }
    }
}