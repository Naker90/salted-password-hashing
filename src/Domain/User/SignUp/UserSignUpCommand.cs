using SaltedPasswordHashing.Src.Domain.Types;
using SaltedPasswordHashing.Src.Domain.User;

namespace SaltedPasswordHashing.Src.Domain.User.SignUp
{
    public sealed class UserSignUpCommand
    {
        private readonly UserRepository userRepository;

        public UserSignUpCommand(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public CreationResult<User> Execute(UserSignUpRequest request)
        {   
            var user = User.Crate(
                email: request.Email,
                password: request.Password
            );
            var createdUser = userRepository.Create(user);
            return CreationResult<User>.CreateValidResult(createdUser);
        }
    }
}