using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltedPasswordHashing.Src.Domain.Types;

namespace SaltedPasswordHashing.Test.Domain.Types
{
    [TestClass]
    public class PasswordTests
    {
        [TestMethod]
        public void ShouldCreatesValidPasswords()
        {
            var password = "ValidPassword1$";

            ValidationResult<Password> result = Password.Create(value: password);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(result.Result.Value, password);
        }

        [TestMethod]
        public void ShouldReturnsErrorWhenPasswordLenghtIsLowerThan8()
        {
            var password = "short";

            ValidationResult<Password> result = Password.Create(value: password);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Error, Error.InvalidFormat);
        }

        [TestMethod]
        public void ShouldReturnsErrorWhenPasswordIsNotAlphanumeric()
        {
            var password = "12345678+";

            ValidationResult<Password> result = Password.Create(value: password);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Error, Error.InvalidFormat);
        }

        [TestMethod]
        public void ShouldReturnsErrorWhenPasswordDoesNotContainAtLeastOfOneUpperCaseLetter()
        {
            var password = "lowercasepassword1$";

            ValidationResult<Password> result = Password.Create(value: password);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Error, Error.InvalidFormat);
        }

        [TestMethod]
        public void ShouldReturnsErrorWhenPasswordDoesNotContainAtLeastOfOneSymbol()
        {
            var password = "Passw0rdWith0utSymb0ls";

            ValidationResult<Password> result = Password.Create(value: password);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Error, Error.InvalidFormat);
        }
    }
}