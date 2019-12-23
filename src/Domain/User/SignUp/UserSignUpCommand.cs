using SaltedPasswordHashing.Src.Domain.Types;
using SaltedPasswordHashing.Src.Domain.Security;
using SaltedPasswordHashing.Src.Domain.User;

namespace SaltedPasswordHashing.Src.Domain.User.SignUp
{
    public sealed class UserSignUpCommand
    {
        private readonly UserRepository userRepository;
        private readonly HashingService hashingService;
        private readonly SecurePseudoRandomGenerator securePseudoRandomGenerator;

        public UserSignUpCommand(
            UserRepository userRepository,
            HashingService hashingService,
            SecurePseudoRandomGenerator securePseudoRandomGenerator)
        {
            this.userRepository = userRepository;
            this.hashingService = hashingService;
            this.securePseudoRandomGenerator = securePseudoRandomGenerator;
        }

        public CreationResult<User, SignUpError> Execute(UserSignUpRequest request)
        {   
            if(userRepository.Exist(email: request.Email)){
                return CreationResult<User, SignUpError>.CreateInvalidResult(SignUpError.UserAlreadyExist); 
            }
            var user = HashPasswordAndCreateUser(request);
            userRepository.Create(user);
            return CreationResult<User, SignUpError>.CreateValidResult(user);
        }


        private User HashPasswordAndCreateUser(UserSignUpRequest request)
        {
            request.Password.Hash(
                hashingService: hashingService,
                securePseudoRandomGenerator: securePseudoRandomGenerator);
            return User.Create(email: request.Email, password: request.Password);
        }
    }

    public enum SignUpError
    {
        UserAlreadyExist
    }
}