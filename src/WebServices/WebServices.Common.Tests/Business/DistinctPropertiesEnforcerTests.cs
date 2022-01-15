using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.BusinessRules;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Business
{
    [TestClass]
    public class DistinctPropertiesEnforcerTests
    {
        public BusinessRuleResult TestBusinessRuleResult = new BusinessRuleResult
        {
            FailedObjects = new List<object> { "F1" },
            Result = false
        };

        public BusinessRuleResult TestBusinessRuleResultTrue = new BusinessRuleResult
        {
            FailedObjects = new List<object> { "F1" },
            Result = true
        };

        public static AlternateId TestAlternateId = new AlternateId
        {
            CreateDate = DateTimeOffset.Now,
            CreatedBy = 123456,
            Id = 123,
            LastUpdated = DateTimeOffset.Now,
            LastUpdatedBy = 00124,
            Entity = "Organization",
            EntityId = "1234",
            Property = "SapId",
            Value = "11223344"
        };

        public List<AlternateId> TestEntities = new List<AlternateId> {
            new AlternateId
            {
                CreateDate = DateTimeOffset.Now,
                CreatedBy = 123456,
                Id = 123,
                LastUpdated = DateTimeOffset.Now,
                LastUpdatedBy = 00124,
                Entity = "Organization",
                EntityId = "1234",
                Property = "SapId",
                Value = "11223344"
            }
        };

        public PatchedEntity<AlternateId, long> TestPatchedEntity = new PatchedEntity<AlternateId, long>
        {
            Entity = TestAlternateId,
            ChangedProperties = new HashSet<string> { "Property" }
        };

        [TestMethod]
        public void Enforce_UpdateTestWithValidParameters_DoesNotThrowException()
        {
            // Arrange
            var commonServiceMock = new Mock<IServiceCommon<AlternateId, IAlternateId, long>>();
            var distinctPropertiesEnforcer = new DistinctPropertiesEnforcer<AlternateId, IAlternateId, long>(commonServiceMock.Object);
            var businessRuleDictionary = new Dictionary<IBusinessRule, BusinessRuleResult>();

            // Mock Variables
            var mockRulesCollection = new Mock<IBusinessRuleCollection>();
            mockRulesCollection.Setup(m => m.IsMet()).Returns(TestBusinessRuleResultTrue);
            var mockRulesBuilder = new Mock<IDistinctPropertiesRulesBuilder<AlternateId, IAlternateId, long>>();
            var mockBusinessRule = new Mock<IBusinessRule>();
            mockRulesBuilder.Setup(x => x.BuildRules(It.IsAny<IEnumerable<AlternateId>>(), It.IsAny<IEnumerable<PropertyInfo>>(), ChangeType.Update)).Returns(mockRulesCollection.Object);
            distinctPropertiesEnforcer.RulesBuilder = mockRulesBuilder.Object;

            // Act and Assert
            distinctPropertiesEnforcer.Enforce(new List<AlternateId> { TestAlternateId }, ChangeType.Update).Wait();

        }

        [TestMethod]
        public async Task Enforce_UpdateTestWithValidParameters_ThrowsException()
        {
            // Arrange
            var commonServiceMock = new Mock<IServiceCommon<AlternateId, IAlternateId, long>>();
            var businessRuleDictionary = new Dictionary<IBusinessRule, BusinessRuleResult>();

            // Mock Variables
            var mockRulesCollection = new Mock<IBusinessRuleCollection>();
            var mockRulesBuilder = new Mock<IDistinctPropertiesRulesBuilder<AlternateId, IAlternateId, long>>();
            var mockBusinessRule = new Mock<IBusinessRule>();

            // Mock Setup
            mockRulesCollection.Setup(m => m.IsMet()).Returns(TestBusinessRuleResult);
            mockRulesCollection.Setup(m => m.Results).Returns(businessRuleDictionary);
            mockBusinessRule.Setup(x => x.Name).Returns("Rule1");
            mockRulesCollection.Object.Results.Add(mockBusinessRule.Object, TestBusinessRuleResult);
            mockRulesBuilder.Setup(x => x.BuildRules(It.IsAny<IEnumerable<AlternateId>>(), It.IsAny<IEnumerable<PropertyInfo>>(), ChangeType.Update)).Returns(mockRulesCollection.Object);

            // Add Mock(s) to test
            var distinctPropertiesEnforcer = new DistinctPropertiesEnforcer<AlternateId, IAlternateId, long>(commonServiceMock.Object);
            distinctPropertiesEnforcer.RulesBuilder = mockRulesBuilder.Object;

            // Act and Assert
            await Assert.ThrowsExceptionAsync<BusinessRulesNotMetException>(async () =>
            {
                await distinctPropertiesEnforcer.Enforce(new List<AlternateId> { TestAlternateId }, ChangeType.Update);
            });
        }

        [TestMethod]
        public void Enforce_PatchTestWithValidParameters_DoesNotThrowException()
        {
            // Arrange
            var commonServiceMock = new Mock<IServiceCommon<AlternateId, IAlternateId, long>>();
            var distinctPropertiesEnforcer = new DistinctPropertiesEnforcer<AlternateId, IAlternateId, long>(commonServiceMock.Object);
            var distinctPropertiesRuleMock = new Mock<IDistinctPropertiesRulesBuilder<AlternateId, IAlternateId, long>>();

            distinctPropertiesRuleMock.Setup(x => x.BuildRules(It.IsAny<IEnumerable<AlternateId>>(), It.IsAny<IEnumerable<PropertyInfo>>(), ChangeType.Update).IsMet()).Returns(new BusinessRuleResult
            {
                FailedObjects = new List<object>(),
                Result = true
            });

            distinctPropertiesEnforcer.RulesBuilder = distinctPropertiesRuleMock.Object;

            // Act and Assert
            distinctPropertiesEnforcer.Enforce(new List<AlternateId> { TestPatchedEntity.Entity }, ChangeType.Update).Wait();
        }

        [TestMethod]
        public async Task Enforce_PatchTestWithValidParameters_DoesThrowException()
        {
            // Arrange
            var commonServiceMock = new Mock<IServiceCommon<AlternateId, IAlternateId, long>>();
            var businessRuleDictionary = new Dictionary<IBusinessRule, BusinessRuleResult>();

            // Mock Variables            
            var mockRulesCollection = new Mock<IBusinessRuleCollection>();
            var mockRulesBuilder = new Mock<IDistinctPropertiesRulesBuilder<AlternateId, IAlternateId, long>>();
            var mockBusinessRule = new Mock<IBusinessRule>();

            // Mock Setup
            mockRulesCollection.Setup(m => m.IsMet()).Returns(TestBusinessRuleResult);
            mockRulesCollection.Setup(m => m.Results).Returns(businessRuleDictionary);
            mockBusinessRule.Setup(x => x.Name).Returns("Rule1");
            mockRulesCollection.Object.Results.Add(mockBusinessRule.Object, TestBusinessRuleResult);
            mockRulesBuilder.Setup(x => x.BuildRules(It.IsAny<IEnumerable<AlternateId>>(),It.IsAny<IEnumerable<PropertyInfo>>(), ChangeType.Update)).Returns(mockRulesCollection.Object);

            // Add Mock(s) to test
            var distinctPropertiesEnforcer = new DistinctPropertiesEnforcer<AlternateId, IAlternateId, long>(commonServiceMock.Object);
            distinctPropertiesEnforcer.RulesBuilder = mockRulesBuilder.Object;

            // Act and Assert
            await Assert.ThrowsExceptionAsync<BusinessRulesNotMetException>(async () =>
            {
                await distinctPropertiesEnforcer.Enforce(new List<AlternateId> { TestPatchedEntity.Entity }, ChangeType.Update);
            });
        }

        [TestMethod]
        public void Enforce_PostTestWithValidParameters_DoesNotThrowException()
        {
            // Arrange
            var commonServiceMock = new Mock<IServiceCommon<AlternateId, IAlternateId, long>>();
            var distinctPropertiesEnforcer = new DistinctPropertiesEnforcer<AlternateId, IAlternateId, long>(commonServiceMock.Object);
            var distinctPropertiesRuleMock = new Mock<IDistinctPropertiesRulesBuilder<AlternateId, IAlternateId, long>>();

            distinctPropertiesRuleMock.Setup(x => x.BuildRules(It.IsAny<IEnumerable<AlternateId>>(), It.IsAny<IEnumerable<PropertyInfo>>(), ChangeType.Create).IsMet()).Returns(new BusinessRuleResult
            {
                FailedObjects = new List<object>(),
                Result = true
            });

            distinctPropertiesEnforcer.RulesBuilder = distinctPropertiesRuleMock.Object;

            // Act and Assert
            distinctPropertiesEnforcer.Enforce(TestEntities, ChangeType.Create).Wait();

        }

        [TestMethod]
        public async Task Enforce_PostTestWithValidParameters_DoesThrowException()
        {
            // Arrange
            var commonServiceMock = new Mock<IServiceCommon<AlternateId, IAlternateId, long>>();
            var distinctPropertiesEnforcer = new DistinctPropertiesEnforcer<AlternateId, IAlternateId, long>(commonServiceMock.Object);
            var businessRuleDictionary = new Dictionary<IBusinessRule, BusinessRuleResult>();

            // Mock Variables            
            var mockRulesCollection = new Mock<IBusinessRuleCollection>();
            var mockRulesBuilder = new Mock<IDistinctPropertiesRulesBuilder<AlternateId, IAlternateId, long>>();
            var mockBusinessRule = new Mock<IBusinessRule>();

            // Mock Setup
            mockRulesCollection.Setup(m => m.IsMet()).Returns(TestBusinessRuleResult);
            mockRulesCollection.Setup(m => m.Results).Returns(businessRuleDictionary);
            mockBusinessRule.Setup(x => x.Name).Returns("Rule1");
            mockRulesCollection.Object.Results.Add(mockBusinessRule.Object, TestBusinessRuleResult);
            mockRulesBuilder.Setup(x => x.BuildRules(It.IsAny<IEnumerable<AlternateId>>(), It.IsAny<IEnumerable<PropertyInfo>>(), ChangeType.Create)).Returns(mockRulesCollection.Object);

            // Add Mock(s) to test
            distinctPropertiesEnforcer.RulesBuilder = mockRulesBuilder.Object;

            // Act and Assert
            await Assert.ThrowsExceptionAsync<BusinessRulesNotMetException>(async () => 
                await distinctPropertiesEnforcer.Enforce(TestEntities, ChangeType.Create));
        }
    }
}
