using System;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Behaviors;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Behaviors.HeaderValidation.Tests
{
    [TestClass]
    public class HeaderValidationInspector_Append_Tests
    {
        private MockRepository _MockRepository;

        private Mock<IPluginHeaderValidator> _MockHeaderValidator;
        private Mock<IAccessController> _MockAccessController;
        private Mock<ILogger> _MockLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockHeaderValidator = _MockRepository.Create<IPluginHeaderValidator>();
            _MockAccessController = _MockRepository.Create<IAccessController>();
            _MockLogger = _MockRepository.Create<ILogger>();
        }

        private HeaderValidationInspector CreateHeaderValidationInspector()
        {
            return new HeaderValidationInspector(
                _MockHeaderValidator.Object,
                _MockAccessController.Object,
                _MockLogger.Object);
        }

        [TestMethod]
        public void Append_NullFirst_Test()
        {
            // Arrange
            NameValueCollection nvc1 = null;
            var nvc2 = new NameValueCollection { { "a", "c" } };
            var headerValidationInspector = CreateHeaderValidationInspector();

            // Act
            var AppenddNvc = headerValidationInspector.Append(nvc1, nvc2);

            // Assert
            Assert.AreEqual("c", AppenddNvc["a"]);
        }

        [TestMethod]
        public void Append_NullSecond_Test()
        {
            // Arrange
            var nvc1 = new NameValueCollection { { "a", "b" } };
            NameValueCollection nvc2 = null;
            var headerValidationInspector = CreateHeaderValidationInspector();

            // Act
            var AppenddNvc = headerValidationInspector.Append(nvc1, nvc2);

            // Assert
            Assert.AreEqual("b", AppenddNvc["a"]);
        }

        [TestMethod]
        public void Append_SameKeyDifferentValues_Test()
        {
            // Arrange
            var nvc1 = new NameValueCollection { { "a", "b" } };
            var nvc2 = new NameValueCollection { { "a", "c" } };
            var headerValidationInspector = CreateHeaderValidationInspector();

            // Act
            var AppenddNvc = headerValidationInspector.Append(nvc1, nvc2);

            // Assert
            Assert.AreEqual("b,c", AppenddNvc["a"]);
        }

        [TestMethod]
        public void Append_DuplicateKeyValuePair_Test()
        {
            // Arrange
            var nvc1 = new NameValueCollection { { "a", "b" } };
            var nvc2 = new NameValueCollection { { "a", "b" } };
            var headerValidationInspector = CreateHeaderValidationInspector();

            // Act
            var AppenddNvc = headerValidationInspector.Append(nvc1, nvc2);

            // Assert
            Assert.AreEqual("b", AppenddNvc["a"]);
        }

        [TestMethod]
        public void Append_DuplicateKeyValuePairAfterNonDuplicate_Test()
        {
            // Arrange
            var nvc1 = new NameValueCollection { { "a", "b" } };
            var nvc2 = new NameValueCollection { { "a", "c" } };
            var nvc3 = new NameValueCollection { { "a", "b" } };
            var headerValidationInspector = CreateHeaderValidationInspector();

            // Act
            var AppenddNvc = headerValidationInspector.Append(nvc1, nvc2, nvc3);

            // Assert
            Assert.AreEqual("b,c", AppenddNvc["a"]);
        }
    }
}
