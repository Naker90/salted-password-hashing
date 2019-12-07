using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;

namespace SaltedPasswordHashing.Src.Domain.Types
{
    public sealed class RequestValidationResult<TResult, TError> 
        where TResult : class
        where TError : struct, IConvertible
    {
        public TResult Result { get; }
        public ReadOnlyCollection<ValidationError<TError>> Errors { get; }
        public bool IsValid { get; }

        private RequestValidationResult(
            TResult result, 
            ReadOnlyCollection<ValidationError<TError>> errors, 
            bool isValid)
        {
            Result = result;
            Errors = errors;
            IsValid = isValid;
        }

        public static RequestValidationResult<TResult, TError> CreateValidResult(TResult result)
        {
            return new RequestValidationResult<TResult, TError>(
                result: result,
                errors: new List<ValidationError<TError>>().AsReadOnly(),
                isValid: true);
        }

        public static RequestValidationResult<TResult, TError> CreateInvalidResult(
            ReadOnlyCollection<ValidationError<TError>> errors)
        {
            return new RequestValidationResult<TResult, TError>(
                result: null,
                errors: errors,
                isValid: false);
        }
    }

    public sealed class ValidationError<TError> where TError : struct, IConvertible
    {
        public string FieldId { get; }
        public TError Error { get; }

        public ValidationError(string fieldId, TError error)
        {
            FieldId = fieldId;
            Error = error;
        }
    }
}