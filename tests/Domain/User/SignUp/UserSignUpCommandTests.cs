using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltedPasswordHashing.Src.Domain.Types;
using SaltedPasswordHashing.Src.Domain.User.SignUp;
using System;
using Moq;

namespace SaltedPasswordHashing.Test.Domain.User.SignUp
{
    [TestClass]
    public class UserSignUpCommandTests
    {
        [TestMethod]
        public void SingsUpUser()
        {
            var userRepository = new Mock<UserRepository>();
            var userIdCreator = new Mock<IUserIdCreator>();
            var command = new UserSignUpCommand(
                userRepository: userRepository.Object,
                userIdCreator: userIdCreator.Object);
            UserSignUpRequest request = UserSignUpRequest.Create(
                email: "user@email.com",
                password: "Passw0rd$"
            ).Result; 
            var userId = new Guid("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4");
            userIdCreator
                .Setup(x => x.Create())
                .Returns(new UserId(value: userId));
            userRepository
                .Setup(x => x.Create(It.Is<User>(x => 
                    x.Id.Value == userId
                    && x.Email.Value == request.Email.Value
                    && x.Password.Value == request.Password.Value)))
                .Returns(new User(new UserId(userId), request.Email, request.Password));


            var result = command.Execute(request);

            Assert.IsTrue(result.IsValid);
        }
    }

    public interface UserRepository
    {
        User Create(User user);
    }

    public sealed class User 
    {
        public UserId Id { get; set; }
        public Email Email { get; }
        public Password Password { get; }

        public User(UserId id, Email email, Password password)
        {
            this.Id = id;
            this.Email = email;
            this.Password = password;
        }
    }

    public sealed class UserId 
    {
        public Guid Value { get; }

        public UserId(Guid value)
        {
            this.Value = value;
        }
    }

    public interface IUserIdCreator
    {
        UserId Create();
    }

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