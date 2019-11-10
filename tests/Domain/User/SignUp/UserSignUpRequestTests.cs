using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltedPasswordHashing.Src.Domain.Types;
using SaltedPasswordHashing.Src.Domain.User.SignUp;
using System.Linq;

namespace SaltedPasswordHashing.Test.Domain.User.SignUp
{
    [TestClass]
    public class UserSignUpRequestTests
    {
        [TestMethod]
        public void ShouldCreatesValidSignUpRequest()
        {
            var email = "user@email.com";
            var password = "Passw0rd$";

            RequestValidationResult<UserSignUpRequest> result = UserSignUpRequest.Create(
                email: email,
                password: password
            ); 

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(result.Result.Email.Value, email);
            Assert.AreEqual(result.Result.Password.Value, password);
        }

        [TestMethod]
        public void ShouldReturnsErrorWhenEmailIsEmpty()
        {
            RequestValidationResult<UserSignUpRequest> result = UserSignUpRequest.Create(
                email: string.Empty,
                password: "Passw0rd$"
            ); 

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Errors.First().FieldId, "Email");
            Assert.AreEqual(result.Errors.First().Error, Error.Required);
        }

        [TestMethod]
        public void ShouldReturnsErrorWhenPasswordIsEmpty()
        {
            RequestValidationResult<UserSignUpRequest> result = UserSignUpRequest.Create(
                email: "user@email.com",
                password: null
            ); 

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Errors.First().FieldId, "Password");
            Assert.AreEqual(result.Errors.First().Error, Error.Required);
        }

        [TestMethod]
        public void ShouldReturnsErrorWhenEmailCreationIsFail()
        {
            RequestValidationResult<UserSignUpRequest> result = UserSignUpRequest.Create(
                email: "user.com",
                password: "Passw0rd$"
            ); 

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Errors.First().FieldId, "Email");
            Assert.AreEqual(result.Errors.First().Error, Error.InvalidFormat);
        }

        
        [TestMethod]
        public void ShouldReturnsErrorWhenCreationIsFail()
        {
            RequestValidationResult<UserSignUpRequest> result = UserSignUpRequest.Create(
                email: "user@email.com",
                password: "password"
            ); 

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Errors.First().FieldId, "Password");
            Assert.AreEqual(result.Errors.First().Error, Error.InvalidFormat);
        }
    }
}