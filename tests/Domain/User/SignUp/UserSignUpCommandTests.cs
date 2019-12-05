using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltedPasswordHashing.Src.Domain.Types;
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
        private Mock<IUserIdCreator> userIdCreator;
        private UserSignUpCommand command;

        [TestInitialize]
        public void Init()
        {
            userRepository = new Mock<UserRepository>();
            userIdCreator = new Mock<IUserIdCreator>();
            command = new UserSignUpCommand(
                userRepository: userRepository.Object,
                userIdCreator: userIdCreator.Object);
        }

        [TestMethod]
        public void ShouldSignUpUser()
        {
            UserSignUpRequest request = CreateRequest();
            var userId = new Guid("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4");
            userIdCreator
                .Setup(x => x.Create())
                .Returns(new UserId(value: userId));
            userRepository
                .Setup(x => x.Create(It.Is<SaltedPasswordHashing.Src.Domain.User.User>(x => 
                    x.Id.Value == userId
                    && x.Email.Value == request.Email.Value
                    && x.Password.Value == request.Password.Value)))
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
            var userId = new Guid("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4");
            var email = Email.Create("user@email.com").Result;
            var password = Password.Create("Pass0word$").Result;
            return new SaltedPasswordHashing.Src.Domain.User.User(
                    id: new UserId(userId), 
                    email: email, 
                    password: password);
        }
    }
}