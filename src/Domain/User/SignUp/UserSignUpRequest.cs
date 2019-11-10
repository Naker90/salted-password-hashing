using SaltedPasswordHashing.Src.Domain.Types;
using System.Collections.Generic;
using System.Linq;

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
            var errors = new List<ValidationError>();
            if(string.IsNullOrEmpty(email))
            {
                errors.Add(new ValidationError(fieldId: nameof(Email), error: Error.Required));
            }
            if(string.IsNullOrEmpty(password))
            {
                errors.Add(new ValidationError(fieldId: nameof(Password), error: Error.Required));
            }
            ValidationResult<Email> emailValidationResult = Email.Create(value: email);
            if(!emailValidationResult.IsValid){
                errors.Add(new ValidationError(
                    fieldId: nameof(Email), 
                    error: emailValidationResult.Error.Value));
            }
            ValidationResult<Password> passwordValidationResult = Password.Create(value: password);
            if(!passwordValidationResult.IsValid){
                errors.Add(new ValidationError(
                    fieldId: nameof(Password), 
                    error: passwordValidationResult.Error.Value));
            }

            if(errors.Any()){
                return RequestValidationResult<UserSignUpRequest>.CreateInvalidResult(
                    errors: errors.AsReadOnly()
                );
            }

            UserSignUpRequest request = new UserSignUpRequest(
                email: emailValidationResult.Result,
                password: passwordValidationResult.Result);

            return RequestValidationResult<UserSignUpRequest>.CreateValidResult(result: request);
        }
    }
}