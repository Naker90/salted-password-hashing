using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltedPasswordHashing.Src.Domain.Types;
using System;

namespace SaltedPasswordHashing.Test.Domain.Unit.Types
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldNotCreateValidResultWithNull()
        {   
            var result = CreationResult<ResultForTest, ErrorForTest>.CreateValidResult(
                result: null
            ); 
        }

        [TestMethod]
        public void ShouldCreateInvalidResult()
        {   
            var result = CreationResult<ResultForTest, ErrorForTest>.CreateInvalidResult(
                error: ErrorForTest.AnyError
            ); 

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Error, ErrorForTest.AnyError);
        }

        private class ResultForTest
        {
            public string Value { get; }
            public ResultForTest(string value)
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