using SaltedPasswordHashing.Src.Domain.Types;
using SaltedPasswordHashing.Src.Domain.Security;
using SaltedPasswordHashing.Src.Domain.User;

namespace SaltedPasswordHashing.Src.Domain.User.Login
{
    public sealed class UserLoginCommand
    {
        private readonly UserRepository userRepository;
        private readonly PasswordEncryptionService passwordEncryptionService;

        public UserLoginCommand(
            UserRepository userRepository,
            PasswordEncryptionService passwordEncryptionService)
        {
            this.userRepository = userRepository;
            this.passwordEncryptionService = passwordEncryptionService;
        }

        public CreationResult<User, LoginError> Execute(UserLoginRequest request)
        {   
            var user = userRepository.FindBy(request.Email);
            if(user == null){
                return CreationResult<User, LoginError>.CreateInvalidResult(LoginError.UserNotFound);
            }
            var saltedPassword = request.Password.Value + user.Password.SaltProp.Value;
            var expectedPassword = passwordEncryptionService.Encrypt(password: saltedPassword);
            if(user.Password.Value != expectedPassword)
            {
                return CreationResult<User, LoginError>.CreateInvalidResult(LoginError.InvalidCredentials);
            }
            return CreationResult<User, LoginError>.CreateValidResult(user);
        }
    }

    public enum LoginError
    {
        InvalidCredentials,
        UserNotFound
    }
}