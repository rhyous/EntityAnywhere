using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Rhyous.EntityAnywhere.Behaviors.RestEndpoint.Tests
{
    [TestClass]
    public class ServiceBehaviorApplicatorTests
    {

        #region AddServiceBehavior tests      
        [TestMethod]
        public void AddServiceBehaviorNullAttributesListTest()
        {
            // Arrange
            List<Attribute> attributes = null;
            var behaviors = new KeyedByTypeCollection<IServiceBehavior>();
            var pluginBehaviors = new List<IServiceBehavior> { new ServiceBehavior1() };

            // Act
            ServiceBehaviorApplicator.AddServiceBehavior(attributes, behaviors, pluginBehaviors);

            // Assert
            Assert.AreEqual(behaviors[0], pluginBehaviors[0]);
        }

        [TestMethod]
        public void AddServiceBehaviorEmptyAttributesListTest()
        {
            // Arrange
            List<Attribute> attributes = new List<Attribute> { };
            var behaviors = new KeyedByTypeCollection<IServiceBehavior>();
            var pluginBehaviors = new List<IServiceBehavior> { new ServiceBehavior1() };

            // Act
            ServiceBehaviorApplicator.AddServiceBehavior(attributes, behaviors, pluginBehaviors);

            // Assert
            Assert.AreEqual(behaviors[0], pluginBehaviors[0]);
        }

        [TestMethod]
        public void AddServiceBehaviorRegularAttributeTest()
        {
            // Arrange
            List<Attribute> attributes = new List<Attribute> { new EntityAttribute() };
            var behaviors = new KeyedByTypeCollection<IServiceBehavior>();
            var pluginBehaviors = new List<IServiceBehavior> { new ServiceBehavior1() };

            // Act
            ServiceBehaviorApplicator.AddServiceBehavior(attributes, behaviors, pluginBehaviors);

            // Assert
            Assert.AreEqual(behaviors[0], pluginBehaviors[0]);
        }

        [TestMethod]
        public void AddServiceBehaviorConflictingAttributesTest()
        {
            // Arrange
            List<Attribute> attributes = new List<Attribute> { new IncludedServiceBehaviorsAttribute(), new ExcludedServiceBehaviorsAttribute() };
            var behaviors = new KeyedByTypeCollection<IServiceBehavior>();
            var pluginBehaviors = new List<IServiceBehavior> { new ServiceBehavior1() };

            // Act and Assert
            Assert.ThrowsException<ConflictingAttributesException>(() => ServiceBehaviorApplicator.AddServiceBehavior(attributes, behaviors, pluginBehaviors));
        }

        [TestMethod]
        public void AddServiceBehaviorIsIncludedAttributesTest()
        {
            // Arrange
            List<Attribute> attributes = new List<Attribute> { new IncludedServiceBehaviorsAttribute("ServiceBehavior1") };
            var behaviors = new KeyedByTypeCollection<IServiceBehavior>();
            var pluginBehaviors = new List<IServiceBehavior> { new ServiceBehavior1() };

            // Act
            ServiceBehaviorApplicator.AddServiceBehavior(attributes, behaviors, pluginBehaviors);

            // Assert
            Assert.AreEqual(behaviors[0], pluginBehaviors[0]);
        }

        [TestMethod]
        public void AddServiceBehaviorIsIncludedByTypeAttributesTest()
        {
            // Arrange
            List<Attribute> attributes = new List<Attribute> { new IncludedServiceBehaviorTypesAttribute(ServiceBehaviorType.Authenticator) };
            var behaviors = new KeyedByTypeCollection<IServiceBehavior>();
            var pluginBehaviors = new List<IServiceBehavior> { new ServiceBehavior1() };

            // Act
            ServiceBehaviorApplicator.AddServiceBehavior(attributes, behaviors, pluginBehaviors);

            // Assert
            Assert.AreEqual(behaviors[0], pluginBehaviors[0]);
        }

        [TestMethod]
        public void AddServiceBehaviorIsExcludedAttributesTest()
        {
            // Arrange
            List<Attribute> attributes = new List<Attribute> { new ExcludedServiceBehaviorsAttribute("ServiceBehavior1") };
            var behaviors = new KeyedByTypeCollection<IServiceBehavior>();
            var pluginBehaviors = new List<IServiceBehavior> { new ServiceBehavior1() };

            // Act
            ServiceBehaviorApplicator.AddServiceBehavior(attributes, behaviors, pluginBehaviors);

            // Act and Assert
            Assert.AreEqual(0, behaviors.Count);
        }

        [TestMethod]
        public void AddServiceBehaviorIsExcludedByTypeAttributesTest()
        {
            // Arrange
            List<Attribute> attributes = new List<Attribute> { new ExcludedServiceBehaviorTypesAttribute(ServiceBehaviorType.Authenticator) };
            var behaviors = new KeyedByTypeCollection<IServiceBehavior>();
            var pluginBehaviors = new List<IServiceBehavior> { new ServiceBehavior1() };

            // Act
            ServiceBehaviorApplicator.AddServiceBehavior(attributes, behaviors, pluginBehaviors);

            // Act and Assert
            Assert.AreEqual(0, behaviors.Count);
        }
        #endregion

        #region IsIncluded tests
        [TestMethod]
        public void IsIncludedServiceBehaviorBaseTrueTests()
        {
            // Arrange
            var sb = new ServiceBehavior1();
            var attrib = new IncludedServiceBehaviorsAttribute("ServiceBehavior1");
            // Act & Assert
            Assert.IsTrue(ServiceBehaviorApplicator.IsIncluded(sb, attrib));
        }

        [TestMethod]
        public void IsIncludedServiceBehaviorBaseFalseTests()
        {
            // Arrange
            var sb = new ServiceBehavior1();
            var attrib = new IncludedServiceBehaviorsAttribute("ServiceBehaviorBogus");
            // Act & Assert
            Assert.IsFalse(ServiceBehaviorApplicator.IsIncluded(sb, attrib));
        }

        [TestMethod]
        public void IsIncludedServiceBehaviorByTypeFalseTests()
        {
            // Arrange
            var sb = new ServiceBehavior1();
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
            var sb = new ServiceBehavior1();
            var attrib = new ExcludedServiceBehaviorsAttribute("ServiceBehavior1");
            // Act & Assert
            Assert.IsTrue(ServiceBehaviorApplicator.IsExcluded(sb, attrib));
        }

        [TestMethod]
        public void IsExcludedServiceBehaviorBaseFalseTests()
        {
            // Arrange
            var sb = new ServiceBehavior1();
            var attrib = new ExcludedServiceBehaviorsAttribute("ServiceBehaviorBogus");
            // Act & Assert
            Assert.IsFalse(ServiceBehaviorApplicator.IsExcluded(sb, attrib));
        }

        [TestMethod]
        public void IsExcludedServiceBehaviorByTypeFalseTests()
        {
            // Arrange
            var sb = new ServiceBehavior1();
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
