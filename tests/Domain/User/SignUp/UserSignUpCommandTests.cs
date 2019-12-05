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
        [TestMethod]
        public void ShouldSignUpUser()
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
                .Setup(x => x.Create(It.Is<SaltedPasswordHashing.Src.Domain.User.User>(x => 
                    x.Id.Value == userId
                    && x.Email.Value == request.Email.Value
                    && x.Password.Value == request.Password.Value)))
                .Returns(new SaltedPasswordHashing.Src.Domain.User.User(
                    new UserId(userId), request.Email, request.Password));


            var result = command.Execute(request);

            Assert.IsTrue(result.IsValid);
        }
    }
}