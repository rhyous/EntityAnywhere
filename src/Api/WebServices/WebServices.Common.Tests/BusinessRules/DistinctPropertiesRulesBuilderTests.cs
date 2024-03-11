using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Business
{
    [TestClass]
    public class DistinctPropertiesRulesBuilderTests
    {
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


        [TestMethod]
        public void BuildRules_GivenValidParameters_ReturnsValidCollection()
        {
            // Arrange
            var commonServiceMock = new Mock<IServiceCommon<AlternateId, IAlternateId, long>>();
            var distinctPropertiesEnforcer = new DistinctPropertiesEnforcer<AlternateId, IAlternateId, long>(commonServiceMock.Object);
            // Mock Variables
            
            var rulesBuilder = new DistinctPropertiesRulesBuilder<AlternateId, IAlternateId, long>(commonServiceMock.Object);

            // Act
            var result = rulesBuilder.BuildRules(TestEntities, typeof(AlternateId).GetProperties<DistinctPropertyAttribute>(), ChangeType.Create);

            // Assert
            Assert.IsTrue(result.Rules.Count > 0);
        }
    }
}
