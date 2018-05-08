using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdvancedXml.Tests
{
    [TestClass]
    public class XmlValidatorTests
    {
        [TestMethod]
        public void Test_Validate_ValidXml_ShouldReturnTrue()
        {
            var validator = new XmlValidator();
            var success = validator.Validate("books_valid.xml", (lineNumber, linePosition, message) =>
            {
                Console.WriteLine($"[{lineNumber}:{linePosition}] {message}");
            });

            Assert.AreEqual(true, success);
        }

        [TestMethod]
        public void Test_Validate_InvalidXml_ShouldReturnFalse()
        {
            var validator = new XmlValidator();
            var success = validator.Validate("books_invalid.xml", (lineNumber, linePosition, message) =>
            {
                Console.WriteLine($"[{lineNumber}:{linePosition}] {message}");
            });

            Assert.AreEqual(false, success);
        }
    }
}
