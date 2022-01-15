using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.BusinessRules;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Business
{
    [TestClass]
    public class RelatedEntityEnforcerTests
    {
        public static Product TestProduct = new Product
        {
            CreateDate = DateTimeOffset.Now,
            CreatedBy = 123456,
            Description = "Test Product",
            Enabled = true,
            Id = 123,
            LastUpdated = DateTimeOffset.Now,
            LastUpdatedBy = 00124,
            Name = "TestProd",
            TypeId = 1,
            Version = "1.0"
        };

        public List<Product> TestEntities = new List<Product> {
            new Product
            {
                CreateDate = DateTimeOffset.Now,
                CreatedBy = 123456,
                Description = "Test Product",
                Enabled = true,
                Id = 123,
                LastUpdated = DateTimeOffset.Now,
                LastUpdatedBy = 00124,
                Name = "TestProd",
                TypeId = 1,
                Version = "1.0"
            }
        };

        public BusinessRuleResult TestBusinessRuleResult = new BusinessRuleResult
        {
            FailedObjects = new List<object> { "F1" },
            Result = false
        };

        public PatchedEntity<Product, int> TestPatchedEntity = new PatchedEntity<Product, int>
        {
                Entity = TestProduct,
                ChangedProperties = new HashSet<string> { "TypeId" }
        };

        [TestMethod]
        public void Enforce_PostTestWithValidParameters_DoesNotThrowException()
        {
            // Arrange
            var relatedEntityMustExistRuleMock = new Mock<IRelatedEntityRulesBuilder<Product>>();
            var relatedEntityEnforcer = new RelatedEntityEnforcer<Product>(relatedEntityMustExistRuleMock.Object);            
            

            relatedEntityMustExistRuleMock.Setup(x => x.BuildRules(It.IsAny<IEnumerable<Product>>(), It.IsAny<IEnumerable<RelatedEntityAttribute>>(), null).IsMet()).Returns(new BusinessRuleResult {
                FailedObjects = new List<object>(),
                Result = true
            });

            // Act and Assert
            relatedEntityEnforcer.Enforce(TestEntities).Wait();
        }

        [TestMethod]
        public async Task Enforce_PostTestWithValidParameters_DoesThrowException()
        {
            // Arrange
            var mockBusinessRule = new Mock<IBusinessRule>();

            var mockRelatedEntityRulesCollection = new Mock<IBusinessRuleCollection>();
            var businessRuleDictionary = new Dictionary<IBusinessRule, BusinessRuleResult>();
            var mockRelatedEntityRulesBuilder = new Mock<IRelatedEntityRulesBuilder<Product>>();

            mockRelatedEntityRulesCollection.Setup(m => m.IsMet()).Returns(TestBusinessRuleResult);
            mockRelatedEntityRulesCollection.Setup(m => m.Results).Returns(businessRuleDictionary);
            mockBusinessRule.Setup(x => x.Name).Returns("Rule1");
            mockRelatedEntityRulesCollection.Object.Results.Add(mockBusinessRule.Object, TestBusinessRuleResult);            
            mockRelatedEntityRulesBuilder.Setup(x => x.BuildRules(It.IsAny<IEnumerable<Product>>(), It.IsAny<IEnumerable<RelatedEntityAttribute>>(), null)).Returns(mockRelatedEntityRulesCollection.Object);

            var relatedEntityEnforcer = new RelatedEntityEnforcer<Product>(mockRelatedEntityRulesBuilder.Object);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<BusinessRulesNotMetException>(async () => await relatedEntityEnforcer.Enforce(TestEntities));
        }
    }
}
