using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltedPasswordHashing.Src.Domain.Types;

namespace SaltedPasswordHashing.Test.Domain.Types
{
    [TestClass]
    public class EmailTests
    {
        [TestMethod]
        public void ShouldCreatesValidEmail()
        {
            var userEmail = "user@email.com";

            ValidationResult<Email> result = Email.Create(value: userEmail);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(result.Result.Value, userEmail);
        }

        [TestMethod]
        public void ShouldReturnsErrorWhenEmailFormatIsInvalid()
        {
            var userEmail = "invalid.com";

            ValidationResult<Email> result = Email.Create(value: userEmail);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Error, Error.InvalidFormat);
        }
    }
}