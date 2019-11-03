using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltedPasswordHashing.Src.Domain.Types;
using SaltedPasswordHashing.Src.Domain.User.SignUp;

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
            Assert.AreEqual(result.Errors[0].FieldId, "Email");
            Assert.AreEqual(result.Errors[0].Error, Error.Required);
        }
    }
}