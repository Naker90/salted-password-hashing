using SaltedPasswordHashing.Src.Domain.Types;
using SaltedPasswordHashing.Src.Domain.Security;
using SaltedPasswordHashing.Src.Domain.User;
using System;

namespace SaltedPasswordHashing.Src.Domain.User.Login
{
    public sealed class UserLoginCommand
    {
        private readonly UserRepository userRepository;
        private readonly HashingService hashingService;

        public UserLoginCommand(
            UserRepository userRepository,
            HashingService hashingService)
        {
            this.userRepository = userRepository;
            this.hashingService = hashingService;
        }

        public CreationResult<User, LoginError> Execute(UserLoginRequest request)
        {   
            var user = userRepository.FindBy(request.Email);
            if(user == null){
                return CreationResult<User, LoginError>.CreateInvalidResult(LoginError.UserNotFound);
            }
            if(!AreUserCredentialsValid(request: request, user: user))
            {
                return CreationResult<User, LoginError>.CreateInvalidResult(LoginError.InvalidCredentials);
            }
            return CreationResult<User, LoginError>.CreateValidResult(user);
        }

        private bool AreUserCredentialsValid(UserLoginRequest request, User user)
        {
            var saltedPassword = request.Password.Value + user.Password.SaltProp.Value;
            return hashingService.Verify(
                text: saltedPassword, 
                hash: user.Password.Value);
        }
    }

    public enum LoginError
    {
        InvalidCredentials,
        UserNotFound
    }
}