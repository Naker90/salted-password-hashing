using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltedPasswordHashing.Src.Domain.Types;

namespace SaltedPasswordHashing.Test.Domain.Types
{
    [TestClass]
    public class CreationResultTests
    {
        
        [TestMethod]
        public void ShouldCreateValidResult()
        {
            var resultForTest = new ResultForTest(value: "test");
            var result = CreationResult<ResultForTest, ErrorForTest>.CreateValidResult(
                result: resultForTest
            ); 

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(result.Result.Value, resultForTest.Value);
        }

    }

    class ResultForTest
    {
        public string Value { get; }
        public ResultForTest(string value)
        {
            this.Value = value;
        }
    }

    enum ErrorForTest
    {
        AnyError
    }
}