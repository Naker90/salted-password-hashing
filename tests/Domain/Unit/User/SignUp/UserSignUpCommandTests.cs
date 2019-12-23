using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltedPasswordHashing.Src.Domain.Types;
using SaltedPasswordHashing.Src.Domain.Security;
using SaltedPasswordHashing.Src.Domain.User.SignUp;
using SaltedPasswordHashing.Src.Domain.User;
using System;
using Moq;

namespace SaltedPasswordHashing.Test.Domain.Unit.User.SignUp
{
    [TestClass]
    public class UserSignUpCommandTests
    {
        private Mock<UserRepository> userRepository;
        private Mock<HashingService> hashingService;
        private Mock<SecurePseudoRandomGenerator> securePseudoRandomGenerator;
        private UserSignUpCommand command;

        [TestInitialize]
        public void Init()
        {
            userRepository = new Mock<UserRepository>(); 
            hashingService = new Mock<HashingService>();
            securePseudoRandomGenerator = new Mock<SecurePseudoRandomGenerator>();
            command = new UserSignUpCommand(
                userRepository: userRepository.Object,
                hashingService: hashingService.Object,
                securePseudoRandomGenerator: securePseudoRandomGenerator.Object);
        }

        [TestMethod]
        public void ShouldSignUpUser()
        {
            UserSignUpRequest request = CreateRequest();
            userRepository
                .Setup(x => x.Exist(request.Email))
                .Returns(false);
            var passwordSalt = new Password.Salt(value: "4235346654");
            securePseudoRandomGenerator
                .Setup(x => x.Generate())
                .Returns(passwordSalt);
            var hashedPasswordOutput = "$2y$asdasdVDFJVw4rtfAFVSDfjc34t";
            hashingService
                .Setup(x => x.Hash(request.Password.Value + passwordSalt.Value))
                .Returns(hashedPasswordOutput);

            var result = command.Execute(request);

            Assert.IsTrue(result.IsValid);
            userRepository
                .Verify(x => x.Create(It.Is<SaltedPasswordHashing.Src.Domain.User.User>(x => 
                    x.IdProp.Value != null
                    && x.Email.Value == request.Email.Value
                    && x.Password.Value == hashedPasswordOutput
                    && x.Password.SaltProp.Value == passwordSalt.Value)), Times.Once());
        }

        [TestMethod]
        public void ShouldReturnErrorWhenUserAlreadyExist()
        {
            UserSignUpRequest request = CreateRequest();
            userRepository
                .Setup(x => x.Exist(request.Email))
                .Returns(true);

            var result = command.Execute(request);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Error, SignUpError.UserAlreadyExist);
            securePseudoRandomGenerator
                .Verify(x => x.Generate(), Times.Never());
            hashingService
                .Verify(x => x.Hash(It.IsAny<string>()), Times.Never());
            userRepository
                .Verify(x => x.Create(It.IsAny<SaltedPasswordHashing.Src.Domain.User.User>()), Times.Never());
        }

        private UserSignUpRequest CreateRequest(){
            return UserSignUpRequest.Create(
                email: "user@email.com",                
                password: "Passw0rd$"
            ).Result;
        }
    }
}