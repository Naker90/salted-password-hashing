using SaltedPasswordHashing.Src.Domain.Types;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SaltedPasswordHashing.Src.Domain.User.SignUp
{
    public sealed class UserSignUpRequest
    {
        public Email Email { get; }
        public Password Password { get; }

        private UserSignUpRequest(Email email, Password password)
        {
            Email = email;
            Password = password;
        }

        public static RequestValidationResult<UserSignUpRequest, Error> Create(
            string email,
            string password)
        {
            CreationResult<Email, Error> emailCreationResult = Email.CreateAndValidate(value: email);
            CreationResult<Password, Error> passwordCreationResult = Password.CreateAndValidate(value: password);

            var errors = BuilValidationErrorsFrom<Email, Error>(creationResult: emailCreationResult, fieldId: nameof(Email))
                .Concat(BuilValidationErrorsFrom<Password, Error>(creationResult: passwordCreationResult, fieldId: nameof(Password)))
                .ToList();

            if(errors.Any())
            {
                return RequestValidationResult<UserSignUpRequest, Error>.CreateInvalidResult(
                    errors: errors.AsReadOnly()
                );
            }

            UserSignUpRequest request = new UserSignUpRequest(
                email: emailCreationResult.Result,
                password: passwordCreationResult.Result);
            return RequestValidationResult<UserSignUpRequest, Error>.CreateValidResult(result: request);
        }

        private static List<ValidationError<TError>> BuilValidationErrorsFrom<TResult, TError>(
            CreationResult<TResult, TError> creationResult,
            string fieldId) 
                where TResult : class
                where TError : struct, IConvertible
        {
            var errors = new List<ValidationError<TError>>();
            if(!creationResult.IsValid)
            {
                errors.Add(new ValidationError<TError>(
                    fieldId: fieldId, 
                    error: creationResult.Error.Value));
            }
            return errors;
        }
    }
}