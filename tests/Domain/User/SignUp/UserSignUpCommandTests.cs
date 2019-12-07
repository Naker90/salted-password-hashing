using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltedPasswordHashing.Src.Domain.Types;
using SaltedPasswordHashing.Src.Domain.Security;
using SaltedPasswordHashing.Src.Domain.User.SignUp;
using SaltedPasswordHashing.Src.Domain.User;
using System;
using Moq;

namespace SaltedPasswordHashing.Test.Domain.User.SignUp
{
    [TestClass]
    public class UserSignUpCommandTests
    {
        private Mock<UserRepository> userRepository;
        private Mock<PasswordEncryptionService> passwordEncryptionService;
        private Mock<SecurePseudoRandomGenerator> securePseudoRandomGenerator;
        private UserSignUpCommand command;

        [TestInitialize]
        public void Init()
        {
            userRepository = new Mock<UserRepository>(); 
            passwordEncryptionService = new Mock<PasswordEncryptionService>();
            securePseudoRandomGenerator = new Mock<SecurePseudoRandomGenerator>();
            command = new UserSignUpCommand(
                userRepository: userRepository.Object,
                passwordEncryptionService: passwordEncryptionService.Object,
                securePseudoRandomGenerator: securePseudoRandomGenerator.Object);
        }

        [TestMethod]
        public void ShouldSignUpUser()
        {
            UserSignUpRequest request = CreateRequest();
            var passwordSalt = new Password.Salt(value: 4235346654);
            securePseudoRandomGenerator
                .Setup(x => x.Generate())
                .Returns(passwordSalt);
            var encryptedPasswordOutput = "$2y$asdasdVDFJVw4rtfAFVSDfjc34t";
            passwordEncryptionService
                .Setup(x => x.Encrypt(request.Password.Value, passwordSalt))
                .Returns(encryptedPasswordOutput);
            userRepository
                .Setup(x => x.Create(It.Is<SaltedPasswordHashing.Src.Domain.User.User>(x => 
                    x.IdProp.Value != null
                    && x.Email.Value == request.Email.Value
                    && x.Password.Value == encryptedPasswordOutput
                    && x.Password.SaltProp.Value == passwordSalt.Value)))
                .Returns(BuildUser());

            var result = command.Execute(request);

            Assert.IsTrue(result.IsValid);
        }

        private UserSignUpRequest CreateRequest(){
            return UserSignUpRequest.Create(
                email: "user@email.com",                
                password: "Passw0rd$"
            ).Result;
        }

        private SaltedPasswordHashing.Src.Domain.User.User BuildUser(){
            var email = Email.Create("user@email.com").Result;
            var password = Password.Create("Pass0word$").Result;
            return SaltedPasswordHashing.Src.Domain.User.User.Crate(
                email: email, 
                password: password);
        }
    }
}