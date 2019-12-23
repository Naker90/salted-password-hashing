using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltedPasswordHashing.Src.Domain.Types;

namespace SaltedPasswordHashing.Test.Domain.Unit.Types
{
    [TestClass]
    public class EmailTests
    {
        [TestMethod]
        public void ShouldCreatesValidEmail()
        {
            var userEmail = "user@email.com";

            CreationResult<Email, Error> result = Email.CreateAndValidate(value: userEmail);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(result.Result.Value, userEmail);
        }

        [TestMethod]
        public void ShouldCreatesValidEmailWithoutValidate()
        {
            var userEmail = "anyEmail";

            Email email = Email.CreateWithoutValidate(value: userEmail);

            Assert.AreEqual(email.Value, userEmail);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void ShouldReturnsErrorWhenEmailIsEmpty(string userEmail)
        {
            CreationResult<Email, Error> result = Email.CreateAndValidate(value: userEmail);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Error, Error.Required);
        }

        [TestMethod]
        public void ShouldReturnsErrorWhenEmailFormatIsInvalid()
        {
            var userEmail = "invalid.com";

            CreationResult<Email, Error> result = Email.CreateAndValidate(value: userEmail);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Error, Error.InvalidFormat);
        }
    }
}