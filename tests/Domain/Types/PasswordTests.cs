using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltedPasswordHashing.Src.Domain.Types;
using SaltedPasswordHashing.Src.Domain.Security;
using Moq;

namespace SaltedPasswordHashing.Test.Domain.Types
{
    [TestClass]
    public class PasswordTests
    {
        [TestMethod]
        public void ShouldCreatesValidPasswords()
        {
            var password = "ValidPassword1$";

            CreationResult<Password, Error> result = Password.CreateAndValidate(value: password);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(result.Result.Value, password);
        }

        [TestMethod]
        public void ShouldCreatesValidPasswordWithoutValidate()
        {
            var value = "anyPassword";

            Password password = Password.CreateWithoutValidate(value: value);

            Assert.AreEqual(password.Value, value);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void ShouldReturnsErrorWhenPasswordIsEmpty(string password)
        {
            CreationResult<Password, Error> result = Password.CreateAndValidate(value: password);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Error, Error.Required);
        }

        [TestMethod]
        public void ShouldReturnsErrorWhenPasswordLenghtIsLowerThan8()
        {
            var password = "short";

            CreationResult<Password, Error> result = Password.CreateAndValidate(value: password);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Error, Error.InvalidFormat);
        }

        [TestMethod]
        public void ShouldReturnsErrorWhenPasswordIsNotAlphanumeric()
        {
            var password = "12345678+";

            CreationResult<Password, Error> result = Password.CreateAndValidate(value: password);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Error, Error.InvalidFormat);
        }

        [TestMethod]
        public void ShouldReturnsErrorWhenPasswordDoesNotContainAtLeastOfOneUpperCaseLetter()
        {
            var password = "lowercasepassword1$";

            CreationResult<Password, Error> result = Password.CreateAndValidate(value: password);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Error, Error.InvalidFormat);
        }

        [TestMethod]
        public void ShouldReturnsErrorWhenPasswordDoesNotContainAtLeastOfOneSymbol()
        {
            var password = "Passw0rdWith0utSymb0ls";

            CreationResult<Password, Error> result = Password.CreateAndValidate(value: password);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Error, Error.InvalidFormat);
        }

        [TestClass]
        public class WhenPasswordIsCreatedSuccessfuly : PasswordTests
        {

            private Mock<EncryptionService> encryptionService;
            private Mock<SecurePseudoRandomGenerator> securePseudoRandomGenerator;
            private Password password;

            [TestInitialize]
            public void Init()
            {
                encryptionService = new Mock<EncryptionService>();
                securePseudoRandomGenerator = new Mock<SecurePseudoRandomGenerator>();
                password = Password.CreateAndValidate(value: "Passw0rd$").Result;
            }

            [TestMethod]
            public void ShouldEncryptPasswordWhenRequested()
            {
                var passwordSalt = new Password.Salt(value: "4235346654");
                securePseudoRandomGenerator
                    .Setup(x => x.Generate())
                    .Returns(passwordSalt);
                var encryptedPasswordOutput = "$2y$asdasdVDFJVw4rtfAFVSDfjc34t";
                encryptionService
                    .Setup(x => x.Encrypt(password.Value + passwordSalt.Value))
                    .Returns(encryptedPasswordOutput);

                password.Encrypt(
                    encryptionService: encryptionService.Object,
                    securePseudoRandomGenerator: securePseudoRandomGenerator.Object);

                Assert.AreEqual(password.Value, encryptedPasswordOutput);
                Assert.AreEqual(password.SaltProp.Value, passwordSalt.Value);
            }
        }
    }
}