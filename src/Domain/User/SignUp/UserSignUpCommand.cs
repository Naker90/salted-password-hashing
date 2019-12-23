using SaltedPasswordHashing.Src.Domain.Types;
using SaltedPasswordHashing.Src.Domain.Security;
using SaltedPasswordHashing.Src.Domain.User;

namespace SaltedPasswordHashing.Src.Domain.User.SignUp
{
    public sealed class UserSignUpCommand
    {
        private readonly UserRepository userRepository;
        private readonly EncryptionService encryptionService;
        private readonly SecurePseudoRandomGenerator securePseudoRandomGenerator;

        public UserSignUpCommand(
            UserRepository userRepository,
            EncryptionService encryptionService,
            SecurePseudoRandomGenerator securePseudoRandomGenerator)
        {
            this.userRepository = userRepository;
            this.encryptionService = encryptionService;
            this.securePseudoRandomGenerator = securePseudoRandomGenerator;
        }

        public CreationResult<User, SignUpError> Execute(UserSignUpRequest request)
        {   
            if(userRepository.Exist(email: request.Email)){
                return CreationResult<User, SignUpError>.CreateInvalidResult(SignUpError.UserAlreadyExist); 
            }
            var user = EncryptPasswordAndCreateUser(request);
            userRepository.Create(user);
            return CreationResult<User, SignUpError>.CreateValidResult(user);
        }


        private User EncryptPasswordAndCreateUser(UserSignUpRequest request)
        {
            request.Password.Encrypt(
                encryptionService: encryptionService,
                securePseudoRandomGenerator: securePseudoRandomGenerator);
            return User.Create(email: request.Email, password: request.Password);
        }
    }

    public enum SignUpError
    {
        UserAlreadyExist
    }
}