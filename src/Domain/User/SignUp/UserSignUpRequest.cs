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
            ValidationResult<Email> emailValidationResult = Email.Create(value: email);
            ValidationResult<Password> passwordValidationResult = Password.Create(value: password);

            var errors = BuilValidationErrorsFrom<Email>(validationResult: emailValidationResult, fieldId: nameof(Email))
                .Concat(BuilValidationErrorsFrom<Password>(validationResult: passwordValidationResult, fieldId: nameof(Password)))
                .ToList();

            if(errors.Any())
            {
                return RequestValidationResult<UserSignUpRequest>.CreateInvalidResult(
                    errors: errors.AsReadOnly()
                );
            }

            UserSignUpRequest request = new UserSignUpRequest(
                email: emailValidationResult.Result,
                password: passwordValidationResult.Result);
            return RequestValidationResult<UserSignUpRequest>.CreateValidResult(result: request);
        }

        private static List<ValidationError> BuilValidationErrorsFrom<T>(
            ValidationResult<T> validationResult,
            string fieldId) where T : class
        {
            var errors = new List<ValidationError>();
            if(!validationResult.IsValid)
            {
                errors.Add(new ValidationError(
                    fieldId: fieldId, 
                    error: validationResult.Error.Value));
            }
            return errors;
        }
    }
}