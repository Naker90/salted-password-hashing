using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltedPasswordHashing.Test.Domain.Unit.Builders;
using SaltedPasswordHashing.Src.Domain.Types;
using SaltedPasswordHashing.Src.Domain.Security;
using SaltedPasswordHashing.Src.Domain.User.Login;
using SaltedPasswordHashing.Src.Domain.User;
using System;
using Moq;

namespace SaltedPasswordHashing.Test.Domain.Unit.User.SignUp
{
    [TestClass]
    public class UserLoginCommandTests
    {
        private Mock<UserRepository> userRepository;
        private Mock<HashingService> hashingService;
        private UserLoginCommand command;

        [TestInitialize]
        public void Init()
        {
            userRepository = new Mock<UserRepository>(); 
            hashingService = new Mock<HashingService>();
            command = new UserLoginCommand(
                userRepository: userRepository.Object,
                hashingService: hashingService.Object);
        }

        [TestMethod]
        public void ShouldLoginUser()
        {
            UserLoginRequest request = CreateRequest();
            var hashedPassword = "$2y$asdasdVDFJVw4rtfAFVSDfjc34t";
            var user = UserBuilder.Build(email: request.Email.Value, password: hashedPassword);
            userRepository
                .Setup(x => x.FindBy(request.Email))
                .Returns(user);
            var passwordIntent = request.Password.Value + user.Password.SaltProp.Value;
            hashingService
                .Setup(x => x.Verify(user.Password.Value, passwordIntent))
                .Returns(true);

            var result = command.Execute(request);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void ShouldReturnsErrorWhenCredentialsAreInvalid()
        {
            UserLoginRequest request = CreateRequest();
            var user = UserBuilder.Build();
            userRepository
                .Setup(x => x.FindBy(It.IsAny<Email>()))
                .Returns(user);
            hashingService
                .Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            var result = command.Execute(request);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Error, LoginError.InvalidCredentials);
        }

        [TestMethod]
        public void ShouldReturnsErrorWhenUserNotFound()
        {
            SaltedPasswordHashing.Src.Domain.User.User user = null;
            userRepository
                .Setup(x => x.FindBy(It.IsAny<Email>()))
                .Returns(user);

            var result = command.Execute(CreateRequest());

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Error, LoginError.UserNotFound);
            hashingService.Verify(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        private UserLoginRequest CreateRequest(){
            return UserLoginRequest.Create(
                email: "user@email.com",                
                password: "Passw0rd$"
            );
        }
    }
}