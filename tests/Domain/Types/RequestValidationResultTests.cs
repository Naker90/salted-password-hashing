using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltedPasswordHashing.Src.Domain.Types;

namespace SaltedPasswordHashing.Test.Domain.Types
{
    [TestClass]
    public class RequestValidationResultTests
    {

        [TestMethod]
        public void ShouldCreateValidRequestResult()
        {
            var requestForTest = new RequestForTest(value: "test");
            
            var result = RequestValidationResult<RequestForTest, Error>.CreateValidResult(
                result: requestForTest
            ); 

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(result.Result.Value, requestForTest.Value);
            Assert.AreEqual(result.Errors.Count, 0);
        }

        private class RequestForTest
        {
            public string Value { get; }
            public RequestForTest(string value)
            {
                this.Value = value;
            }
        }

        private enum ErrorForTest
        {
            AnyError
        }
    }
}