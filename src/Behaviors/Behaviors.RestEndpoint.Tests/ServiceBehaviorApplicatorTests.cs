using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Behaviors;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceModel.Description;

namespace Rhyous.EntityAnywhere.Behaviors.RestEndpoint.Tests
{
    [TestClass]
    public class ServiceBehaviorApplicatorTests
    {
        MockRepository _MockRepository;
        Mock<ILogger> _MockLogger;


       [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);
            _MockLogger = _MockRepository.Create<ILogger>();
            _MockLogger.Setup(m => m.Write(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            _MockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            _MockLogger.Setup(m => m.Debug(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
        }

        internal ServiceBehavior1 CreateServiceBehavior1()
        {
            return new ServiceBehavior1
            {
                Logger = _MockLogger.Object
            };
        }

        #region AddServiceBehavior tests      
        [TestMethod]
        public void AddServiceBehaviorNullAttributesListTest()
        {
            // Arrange
            List<Attribute> attributes = null;
            var behaviors = new KeyedByTypeCollection<IServiceBehavior>();
            var pluginBehaviors = new List<IServiceBehavior> { CreateServiceBehavior1() };

            // Act
            ServiceBehaviorApplicator.AddServiceBehavior(attributes, behaviors, pluginBehaviors, _MockLogger.Object);

            // Assert
            Assert.AreEqual(behaviors[0], pluginBehaviors[0]);
        }

        [TestMethod]
        public void AddServiceBehaviorEmptyAttributesListTest()
        {
            // Arrange
            List<Attribute> attributes = new List<Attribute> { };
            var behaviors = new KeyedByTypeCollection<IServiceBehavior>();
            var pluginBehaviors = new List<IServiceBehavior> { CreateServiceBehavior1() };

            // Act
            ServiceBehaviorApplicator.AddServiceBehavior(attributes, behaviors, pluginBehaviors, _MockLogger.Object);

            // Assert
            Assert.AreEqual(behaviors[0], pluginBehaviors[0]);
        }

        [TestMethod]
        public void AddServiceBehaviorRegularAttributeTest()
        {
            // Arrange
            List<Attribute> attributes = new List<Attribute> { new EntityAttribute() };
            var behaviors = new KeyedByTypeCollection<IServiceBehavior>();
            var pluginBehaviors = new List<IServiceBehavior> { CreateServiceBehavior1() };

            // Act
            ServiceBehaviorApplicator.AddServiceBehavior(attributes, behaviors, pluginBehaviors, _MockLogger.Object);

            // Assert
            Assert.AreEqual(behaviors[0], pluginBehaviors[0]);
        }

        [TestMethod]
        public void AddServiceBehaviorConflictingAttributesTest()
        {
            // Arrange
            List<Attribute> attributes = new List<Attribute> { new IncludedServiceBehaviorsAttribute(), new ExcludedServiceBehaviorsAttribute() };
            var behaviors = new KeyedByTypeCollection<IServiceBehavior>();
            var pluginBehaviors = new List<IServiceBehavior> { CreateServiceBehavior1() };

            // Act and Assert
            Assert.ThrowsException<ConflictingAttributesException>(() => ServiceBehaviorApplicator.AddServiceBehavior(attributes, behaviors, pluginBehaviors, _MockLogger.Object));
        }

        [TestMethod]
        public void AddServiceBehaviorIsIncludedAttributesTest()
        {
            // Arrange
            List<Attribute> attributes = new List<Attribute> { new IncludedServiceBehaviorsAttribute("ServiceBehavior1") };
            var behaviors = new KeyedByTypeCollection<IServiceBehavior>();
            var pluginBehaviors = new List<IServiceBehavior> { CreateServiceBehavior1() };

            // Act
            ServiceBehaviorApplicator.AddServiceBehavior(attributes, behaviors, pluginBehaviors, _MockLogger.Object);

            // Assert
            Assert.AreEqual(behaviors[0], pluginBehaviors[0]);
        }

        [TestMethod]
        public void AddServiceBehaviorIsIncludedByTypeAttributesTest()
        {
            // Arrange
            List<Attribute> attributes = new List<Attribute> { new IncludedServiceBehaviorTypesAttribute(ServiceBehaviorType.Authenticator) };
            var behaviors = new KeyedByTypeCollection<IServiceBehavior>();
            var pluginBehaviors = new List<IServiceBehavior> { CreateServiceBehavior1() };

            // Act
            ServiceBehaviorApplicator.AddServiceBehavior(attributes, behaviors, pluginBehaviors, _MockLogger.Object);

            // Assert
            Assert.AreEqual(behaviors[0], pluginBehaviors[0]);
        }

        [TestMethod]
        public void AddServiceBehaviorIsExcludedAttributesTest()
        {
            // Arrange
            List<Attribute> attributes = new List<Attribute> { new ExcludedServiceBehaviorsAttribute("ServiceBehavior1") };
            var behaviors = new KeyedByTypeCollection<IServiceBehavior>();
            var pluginBehaviors = new List<IServiceBehavior> { CreateServiceBehavior1() };

            // Act
            ServiceBehaviorApplicator.AddServiceBehavior(attributes, behaviors, pluginBehaviors, _MockLogger.Object);

            // Act and Assert
            Assert.AreEqual(0, behaviors.Count);
        }

        [TestMethod]
        public void AddServiceBehaviorIsExcludedByTypeAttributesTest()
        {
            // Arrange
            List<Attribute> attributes = new List<Attribute> { new ExcludedServiceBehaviorTypesAttribute(ServiceBehaviorType.Authenticator) };
            var behaviors = new KeyedByTypeCollection<IServiceBehavior>();
            var pluginBehaviors = new List<IServiceBehavior> { CreateServiceBehavior1() };

            // Act
            ServiceBehaviorApplicator.AddServiceBehavior(attributes, behaviors, pluginBehaviors, _MockLogger.Object);

            // Act and Assert
            Assert.AreEqual(0, behaviors.Count);
        }
        #endregion

        #region IsIncluded tests
        [TestMethod]
        public void IsIncludedServiceBehaviorBaseTrueTests()
        {
            // Arrange
            var sb = CreateServiceBehavior1();
            var attrib = new IncludedServiceBehaviorsAttribute("ServiceBehavior1");
            // Act & Assert
            Assert.IsTrue(ServiceBehaviorApplicator.IsIncluded(sb, attrib));
        }

        [TestMethod]
        public void IsIncludedServiceBehaviorBaseFalseTests()
        {
            // Arrange
            var sb = CreateServiceBehavior1();
            var attrib = new IncludedServiceBehaviorsAttribute("ServiceBehaviorBogus");
            // Act & Assert
            Assert.IsFalse(ServiceBehaviorApplicator.IsIncluded(sb, attrib));
        }

        [TestMethod]
        public void IsIncludedServiceBehaviorByTypeFalseTests()
        {
            // Arrange
            var sb = CreateServiceBehavior1();
            var attrib = new IncludedServiceBehaviorTypesAttribute(ServiceBehaviorType.Authenticator);
            // Act & Assert
            Assert.IsTrue(ServiceBehaviorApplicator.IsIncluded(sb, attrib));
        }

        [TestMethod]
        public void IsIncludedIServiceBehavior1s()
        {
            // Arrange
            var sb = new ServiceBehaviorThirdParty();
            var attrib = new IncludedServiceBehaviorsAttribute("ServiceBehaviorBogus");
            // Act & Assert
            Assert.IsFalse(ServiceBehaviorApplicator.IsIncluded(sb, attrib));
        }
        #endregion

        #region IsExcluded Tests
        [TestMethod]
        public void IsExcludedServiceBehaviorBaseTrueTests()
        {
            // Arrange
            var sb = CreateServiceBehavior1();
            var attrib = new ExcludedServiceBehaviorsAttribute("ServiceBehavior1");
            // Act & Assert
            Assert.IsTrue(ServiceBehaviorApplicator.IsExcluded(sb, attrib));
        }

        [TestMethod]
        public void IsExcludedServiceBehaviorBaseFalseTests()
        {
            // Arrange
            var sb = CreateServiceBehavior1();
            var attrib = new ExcludedServiceBehaviorsAttribute("ServiceBehaviorBogus");
            // Act & Assert
            Assert.IsFalse(ServiceBehaviorApplicator.IsExcluded(sb, attrib));
        }

        [TestMethod]
        public void IsExcludedServiceBehaviorByTypeFalseTests()
        {
            // Arrange
            var sb = CreateServiceBehavior1();
            var attrib = new ExcludedServiceBehaviorTypesAttribute(ServiceBehaviorType.Authenticator);
            // Act & Assert
            Assert.IsTrue(ServiceBehaviorApplicator.IsExcluded(sb, attrib));
        }

        [TestMethod]
        public void IsExcludedIServiceBehavior1s()
        {
            // Arrange
            var sb = new ServiceBehaviorThirdParty();
            var attrib = new ExcludedServiceBehaviorsAttribute("ServiceBehavior1");
            // Act & Assert
            Assert.IsFalse(ServiceBehaviorApplicator.IsIncluded(sb, attrib));
        }
        #endregion

        #region helper classes



        #endregion
    }
}
