using SaltedPasswordHashing.Src.Domain.Types;
using SaltedPasswordHashing.Src.Domain.Security;
using SaltedPasswordHashing.Src.Domain.User;

namespace SaltedPasswordHashing.Src.Domain.User.SignUp
{
    public sealed class UserSignUpCommand
    {
        private readonly UserRepository userRepository;
        private readonly PasswordEncryptionService passwordEncryptionService;
        private readonly SecurePseudoRandomGenerator securePseudoRandomGenerator;

        public UserSignUpCommand(
            UserRepository userRepository,
            PasswordEncryptionService passwordEncryptionService,
            SecurePseudoRandomGenerator securePseudoRandomGenerator)
        {
            this.userRepository = userRepository;
            this.passwordEncryptionService = passwordEncryptionService;
            this.securePseudoRandomGenerator = securePseudoRandomGenerator;
        }

        public CreationResult<User> Execute(UserSignUpRequest request)
        {   
            if(userRepository.Exist(email: request.Email)){
                return CreationResult<User>.CreateInvalidResult(Error.Required); 
            }
            request.Password.Encrypt(
                passwordEncryptionService: passwordEncryptionService,
                securePseudoRandomGenerator: securePseudoRandomGenerator);
            var user = User.Crate(
                email: request.Email,
                password: request.Password
            );
            var createdUser = userRepository.Create(user);
            return CreationResult<User>.CreateValidResult(createdUser);
        }
    }
}