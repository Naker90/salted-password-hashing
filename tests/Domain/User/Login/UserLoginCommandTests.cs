using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltedPasswordHashing.Src.Domain.Types;
using SaltedPasswordHashing.Src.Domain.Security;
using SaltedPasswordHashing.Src.Domain.User.Login;
using SaltedPasswordHashing.Src.Domain.User;
using System;
using Moq;

namespace SaltedPasswordHashing.Test.Domain.User.SignUp
{
    [TestClass]
    public class UserLoginCommandTests
    {
        private Mock<UserRepository> userRepository;
        private Mock<PasswordEncryptionService> passwordEncryptionService;
        private UserLoginCommand command;

        [TestInitialize]
        public void Init()
        {
            userRepository = new Mock<UserRepository>(); 
            passwordEncryptionService = new Mock<PasswordEncryptionService>();
            command = new UserLoginCommand(
                userRepository: userRepository.Object,
                passwordEncryptionService: passwordEncryptionService.Object);
        }

        [TestMethod]
        public void ShouldLoginUser()
        {
            UserLoginRequest request = CreateRequest();
            var encryptedPassword = "$2y$asdasdVDFJVw4rtfAFVSDfjc34t";
            var user = BuildUser(email: request.Email.Value, password: encryptedPassword);
            userRepository
                .Setup(x => x.FindBy(request.Email))
                .Returns(user);
            passwordEncryptionService
                .Setup(x => x.Encrypt(request.Password.Value + user.Password.SaltProp.Value))
                .Returns(encryptedPassword);

            var result = command.Execute(request);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void ShouldReturnsErrorWhenCredentialsAreInvalid()
        {
            UserLoginRequest request = CreateRequest();
            var user = BuildUser(password: "$2y$asdasdVDFJVw4rtfAFVSDfjc34t");
            userRepository
                .Setup(x => x.FindBy(It.IsAny<Email>()))
                .Returns(user);
            var encryptedPassword = "differentEncryptedPassword";
            passwordEncryptionService
                .Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns(encryptedPassword);

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
            passwordEncryptionService.Verify(x => x.Encrypt(It.IsAny<string>()), Times.Never());
        }

        private UserLoginRequest CreateRequest(){
            return UserLoginRequest.Create(
                email: "user@email.com",                
                password: "Passw0rd$"
            );
        }

        private SaltedPasswordHashing.Src.Domain.User.User BuildUser(
            string email = "user@email.com",
            string password = "Pass0word$")
        {
            return SaltedPasswordHashing.Src.Domain.User.User.Create(
                email: Email.CreateWithoutValidate(email), 
                password: Password.CreateWithoutValidate(password));
        }
    }
}