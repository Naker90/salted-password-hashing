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

            ValidationResult<UserSignUpRequest> result = UserSignUpRequest.Create(
                email: email,
                password: password
            ); 

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(result.Result.Email.Value, email);
            Assert.AreEqual(result.Result.Password.Value, password);
        }
    }
}