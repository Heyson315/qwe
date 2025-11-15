using Microsoft.VisualStudio.TestTools.UnitTesting;
using qwe.Configuration;

namespace qwe.Tests.Configuration
{
    [TestClass]
    public class AppSettingsTests
    {
        [TestMethod]
        public void MaxFileUploadSize_ShouldReturnDefaultValue()
        {
            // Act
            var maxSize = AppSettings.MaxFileUploadSize;

            // Assert
            Assert.IsTrue(maxSize > 0);
            Assert.AreEqual(10485760, maxSize); // 10MB
        }

        [TestMethod]
        public void AllowedFileExtensions_ShouldReturnExtensions()
        {
            // Act
            var extensions = AppSettings.AllowedFileExtensions;

            // Assert
            Assert.IsNotNull(extensions);
            Assert.IsTrue(extensions.Length > 0);
        }

        [TestMethod]
        public void ContactEmail_ShouldReturnEmail()
        {
            // Act
            var email = AppSettings.ContactEmail;

            // Assert
            Assert.IsNotNull(email);
            Assert.IsTrue(email.Contains("@"));
        }

        [TestMethod]
        public void Environment_ShouldReturnEnvironmentName()
        {
            // Act
            var env = AppSettings.Environment;

            // Assert
            Assert.IsNotNull(env);
            Assert.IsFalse(string.IsNullOrWhiteSpace(env));
        }
    }
}
