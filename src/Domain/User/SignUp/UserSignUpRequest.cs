using SaltedPasswordHashing.Src.Domain.Types;
using System.Collections.Generic;

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
            if(string.IsNullOrEmpty(email))
            {
                return RequestValidationResult<UserSignUpRequest>.CreateInvalidResult(
                    errors: new List<ValidationError>
                    {
                        new ValidationError(
                            fieldId: nameof(Email),
                            error: Error.Required
                        )
                    }.AsReadOnly()
                );
            }
            if(string.IsNullOrEmpty(password))
            {
                return RequestValidationResult<UserSignUpRequest>.CreateInvalidResult(
                    errors: new List<ValidationError>
                    {
                        new ValidationError(
                            fieldId: nameof(Password),
                            error: Error.Required
                        )
                    }.AsReadOnly()
                );
            }

            ValidationResult<Email> emailValidationResult = Email.Create(value: email);
            ValidationResult<Password> passwordValidationResult = Password.Create(value: password);
            
            UserSignUpRequest request = new UserSignUpRequest(
                email: emailValidationResult.Result,
                password: passwordValidationResult.Result);

            return RequestValidationResult<UserSignUpRequest>.CreateValidResult(result: request);
        }
    }
}