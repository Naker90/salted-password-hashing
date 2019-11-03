using SaltedPasswordHashing.Src.Domain.Types;

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

        public static ValidationResult<UserSignUpRequest> Create(
            string email,
            string password)
        {
            ValidationResult<Email> emailValidationResult = Email.Create(value: email);
            ValidationResult<Password> passwordValidationResult = Password.Create(value: password);
            
            return ValidationResult<UserSignUpRequest>.CreateValidResult(
                result: new UserSignUpRequest(
                    email: emailValidationResult.Result,
                    password: passwordValidationResult.Result));
        }
    }
}