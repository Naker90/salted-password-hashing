using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace SaltedPasswordHashing.Src.Domain.Types
{
    public sealed class RequestValidationResult<T> where T : class
    {
        public T Result { get; }
        public ReadOnlyCollection<ValidationError> Errors { get; }
        public bool IsValid { get; }

        private RequestValidationResult(
            T result, 
            ReadOnlyCollection<ValidationError> errors, 
            bool isValid)
        {
            Result = result;
            Errors = errors;
            IsValid = isValid;
        }

        public static RequestValidationResult<T> CreateValidResult(T result)
        {
            return new RequestValidationResult<T>(
                result: result,
                errors: new List<ValidationError>().AsReadOnly(),
                isValid: true);
        }

        public static RequestValidationResult<T> CreateInvalidResult(
            ReadOnlyCollection<ValidationError> errors)
        {
            return new RequestValidationResult<T>(
                result: null,
                errors: errors,
                isValid: false);
        }
    }

    public sealed class ValidationError
    {
        public string FieldId { get; }
        public Error Error { get; }

        public ValidationError(string fieldId, Error error)
        {
            FieldId = fieldId;
            Error = error;
        }
    }
}