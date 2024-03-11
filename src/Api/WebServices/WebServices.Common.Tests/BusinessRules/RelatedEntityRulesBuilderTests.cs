using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Clients2;
using System;
using System.Collections.Generic;
using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.Product;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Business
{
    [TestClass]
    public class RelatedEntityRulesBuilderTests
    {
        private MockRepository _MockRepository;

        private Mock<INamedFactory<IEntityClientAsync>> _MockNamedFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockNamedFactory = _MockRepository.Create<INamedFactory<IEntityClientAsync>>();
        }

        private RelatedEntityRulesBuilder<TEntity> CreateRelatedEntityRulesBuilder()
        {
            return new RelatedEntityRulesBuilder<TEntity>(
                _MockNamedFactory.Object);
        }

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

        public List<RelatedEntityAttribute> TestRelatedEntityAttributes = new List<RelatedEntityAttribute>
        {
            new RelatedEntityAttribute("Product", null, typeof(int), false, "TypeId")
        };

        public HashSet<string> TestChangedProperties = new HashSet<string> { "TypeId" };

        [TestMethod]
        public void BuildRules_GivenValidParameters_ReturnsValidCollection()
        {
            // Arrange
            var relatedEntityRulesBuilder = CreateRelatedEntityRulesBuilder();
            var mockClient = _MockRepository.Create<IEntityClientAsync>();
            _MockNamedFactory.Setup(m => m.Create(It.IsAny<string>()))
                             .Returns(mockClient.Object);

            // Act
            var result = relatedEntityRulesBuilder.BuildRules(TestEntities, TestRelatedEntityAttributes);


            // Assert
            Assert.IsTrue(result.Rules.Count > 0);
        }

        [TestMethod]
        public void BuildRules_GivenInvalidParameters_ReturnsEmptyCollection()
        {
            // Arrange
            var relatedEntityRulesBuilder = CreateRelatedEntityRulesBuilder();

            // Act
            var result = relatedEntityRulesBuilder.BuildRules(new List<Product>(), new List<RelatedEntityAttribute>());

            // Assert
            Assert.IsTrue(result.Rules.Count == 0);
        }
        
        [TestMethod]
        public void BuildRulesOverload_GivenValidParameters_ReturnsValidCollection()
        {
            // Arrange
            var relatedEntityRulesBuilder = CreateRelatedEntityRulesBuilder();
            var mockClient = _MockRepository.Create<IEntityClientAsync>();
            _MockNamedFactory.Setup(m => m.Create(It.IsAny<string>()))
                             .Returns(mockClient.Object);

            // Act
            var result = relatedEntityRulesBuilder.BuildRules(new[] { TestProduct }, TestRelatedEntityAttributes, TestChangedProperties);


            // Assert
            Assert.IsTrue(result.Rules.Count > 0);
        }

        [TestMethod]
        public void BuildRulesOverload_GivenInvalidParameters_ReturnsEmptyCollection()
        {
            // Arrange
            var relatedEntityRulesBuilder = CreateRelatedEntityRulesBuilder();

            // Act
            var result = relatedEntityRulesBuilder.BuildRules(new[] { new Product() }, new List<RelatedEntityAttribute>(), new List<string>());
            
            // Assert
            Assert.IsTrue(result.Rules.Count == 0);
        }
    }
}
