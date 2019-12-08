using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltedPasswordHashing.Src.Domain.Types;
using System.Collections.Generic;

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

        [TestMethod]
        public void ShouldCreateInvalidRequestResult()
        {
            var errors = new List<ValidationError<ErrorForTest>>
            {
                new ValidationError<ErrorForTest>(fieldId: "FieldId", error: ErrorForTest.AnyError)
            }.AsReadOnly();
            
            var result = RequestValidationResult<RequestForTest, ErrorForTest>.CreateInvalidResult(
                errors: errors
            ); 

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Errors[0].FieldId, errors[0].FieldId);
            Assert.AreEqual(result.Errors[0].Error, errors[0].Error);
            Assert.AreEqual(result.Errors.Count, 1);
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