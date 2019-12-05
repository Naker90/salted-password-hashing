using SaltedPasswordHashing.Src.Domain.Types;
using SaltedPasswordHashing.Src.Domain.User;

namespace SaltedPasswordHashing.Src.Domain.User.SignUp
{
    public sealed class UserSignUpCommand
    {
        private readonly UserRepository userRepository;
        private readonly IUserIdCreator userIdCreator;

        public UserSignUpCommand(UserRepository userRepository, IUserIdCreator userIdCreator)
        {
            this.userRepository = userRepository;
            this.userIdCreator = userIdCreator;
        }

        public CreationResult<User> Execute(UserSignUpRequest request)
        {
            var userId = userIdCreator.Create();
            var user = new User(
                id: userId,
                email: request.Email,
                password: request.Password
            );
            var createdUser = userRepository.Create(user);
            return CreationResult<User>.CreateValidResult(createdUser);
        }
    }
}