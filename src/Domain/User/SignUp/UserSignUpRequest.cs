using SaltedPasswordHashing.Src.Domain.Types;
using System.Collections.Generic;
using System.Linq;
using System;

namespace  SaltedPasswordHashing.Src.Domain.User.SignUp
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

        public static RequestValidationResult<UserSignUpRequest> Create(
            string email,
            string password)
        {
            CreationResult<Email> emailCreationResult = Email.Create(value: email);
            CreationResult<Password> passwordCreationResult = Password.Create(value: password);

            var errors = BuilValidationErrorsFrom<Email>(creationResult: emailCreationResult, fieldId: nameof(Email))
                .Concat(BuilValidationErrorsFrom<Password>(creationResult: passwordCreationResult, fieldId: nameof(Password)))
                .ToList();

            if(errors.Any())
            {
                return RequestValidationResult<UserSignUpRequest>.CreateInvalidResult(
                    errors: errors.AsReadOnly()
                );
            }

            UserSignUpRequest request = new UserSignUpRequest(
                email: emailCreationResult.Result,
                password: passwordCreationResult.Result);
            return RequestValidationResult<UserSignUpRequest>.CreateValidResult(result: request);
        }

        private static List<ValidationError> BuilValidationErrorsFrom<T>(
            CreationResult<T> creationResult,
            string fieldId) where T : class
        {
            var errors = new List<ValidationError>();
            if(!creationResult.IsValid)
            {
                errors.Add(new ValidationError(
                    fieldId: fieldId, 
                    error: creationResult.Error.Value));
            }
            return errors;
        }
    }
}