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
            var errors = ValidateEmail(email: email)
                .Concat(ValidatePassword(password: password))
                .ToList();

            if(errors.Any()){
                
                return RequestValidationResult<UserSignUpRequest>.CreateInvalidResult(
                    errors: errors.AsReadOnly()
                );
            }

            ValidationResult<Email> emailValidationResult = Email.Create(value: email);
            ValidationResult<Password> passwordValidationResult = Password.Create(value: password);
            UserSignUpRequest request = new UserSignUpRequest(
                email: emailValidationResult.Result,
                password: passwordValidationResult.Result);

            return RequestValidationResult<UserSignUpRequest>.CreateValidResult(result: request);
        }

        private static List<ValidationError> ValidateEmail(string email)
        {
            var errors = new List<ValidationError>();
            if(string.IsNullOrEmpty(email))
            {
                errors.Add(new ValidationError(fieldId: nameof(Email), error: Error.Required));
            }
            else
            {
                ValidationResult<Email> emailValidationResult = Email.Create(value: email);
                if(!emailValidationResult.IsValid)
                {
                    errors.Add(new ValidationError(
                        fieldId: nameof(Email), 
                        error: emailValidationResult.Error.Value));
                }
            }
            return errors;
        }

        private static List<ValidationError> ValidatePassword(string password)
        {
            var errors = new List<ValidationError>();
            if(string.IsNullOrEmpty(password))
            {
                errors.Add(new ValidationError(fieldId: nameof(Password), error: Error.Required));
            }
            else
            {
                ValidationResult<Password> passwordValidationResult = Password.Create(value: password);
                if(!passwordValidationResult.IsValid)
                {
                    errors.Add(new ValidationError(
                        fieldId: nameof(Password), 
                        error: passwordValidationResult.Error.Value));
                }
            }
            return errors;
        }
    }
}