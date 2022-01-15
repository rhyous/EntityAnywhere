using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.WebServices.Common.Tests.Business.TestEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Business
{
    [TestClass]
    public class DistinctPropertiesMustBeUniqueRuleTests
    {
        public List<TestEntityWithOneGroup> TestEntities = new List<TestEntityWithOneGroup> {
            new TestEntityWithOneGroup
            {
                Entity = "Organization",
                EntityId = "1234",
                Property = "SapId",
                Value = "11223344"
            },
            new TestEntityWithOneGroup
            {
                Entity = "Organization",
                EntityId = "1235",
                Property = "SapId",
                Value = "11223345"
            }
        };

        public List<TestEntityWithOneGroup> UpdatedEntity = new List<TestEntityWithOneGroup> {
            new TestEntityWithOneGroup
            {
                Id = 27,
                Entity = "Organization",
                EntityId = "1234",
                Property = "SapId",
                Value = "11223344"
            }
        };

        public List<TestEntityWithOneGroup> TestEntitiesWithDupes = new List<TestEntityWithOneGroup> {
            new TestEntityWithOneGroup
            {
                Entity = "Organization",
                EntityId = "1234",
                Property = "SapId",
                Value = "11223344"
            },
            new TestEntityWithOneGroup
            {
                Entity = "Organization",
                EntityId = "1234",
                Property = "SapId",
                Value = "11223344"
            }
        };

        public List<TestEntityWithTwoGroups> TestEntitiesTwoGroups = new List<TestEntityWithTwoGroups> {
            new TestEntityWithTwoGroups
            {
                Entity = "Organization",
                EntityId = "1234",
                Property = "SapId",
                Value = "11223344"
            },
            new TestEntityWithTwoGroups
            {
                Entity = "Organization",
                EntityId = "1235",
                Property = "SapId",
                Value = "11223345"
            }
        };

        public List<TestEntityWithTwoGroups> TestEntitiesTwoGroupsAndDuplicates = new List<TestEntityWithTwoGroups> {
            new TestEntityWithTwoGroups
            {
                Entity = "Organization",
                EntityId = "1234",
                Property = "SapId",
                Value = "11223344"
            },
            new TestEntityWithTwoGroups
            {
                Entity = "Organization",
                EntityId = "1234",
                Property = "SapId",
                Value = "11223344"
            }
        };

        #region Create
        [TestMethod]
        public void IsMet_GivenValidParameters_ReturnsResult()
        {
            // Arrange
            var commonServiceMock = new Mock<IServiceCommon<TestEntityWithOneGroup, ITestEntityWithOneGroup, long>>();
            var attrib = typeof(TestEntityWithOneGroup).GetProperties<DistinctPropertyAttribute>();
            var distinctPropertiesMustBeUniqueRule = new DistinctPropertiesMustBeUniqueRule<TestEntityWithOneGroup, ITestEntityWithOneGroup, long>(
                                TestEntities,
                                attrib,
                                commonServiceMock.Object,
                                ChangeType.Create);

            // Act
            var result = distinctPropertiesMustBeUniqueRule.IsMet();

            // Assert
            Assert.AreEqual(result.Result, true);
        }

        [TestMethod]
        public void IsMet_GivenDuplicateEntities_Fails()
        {
            // Arrange
            var commonServiceMock = new Mock<IServiceCommon<TestEntityWithOneGroup, ITestEntityWithOneGroup, long>>();
            var attrib = typeof(TestEntityWithOneGroup).GetProperties<DistinctPropertyAttribute>();
            var distinctPropertiesMustBeUniqueRule = new DistinctPropertiesMustBeUniqueRule<TestEntityWithOneGroup, ITestEntityWithOneGroup, long>(
                                TestEntitiesWithDupes,
                                attrib,
                                commonServiceMock.Object,
                                ChangeType.Create);
            // Act
            var result = distinctPropertiesMustBeUniqueRule.IsMet();

            // Assert
            Assert.AreEqual(result.Result, false);
        }

        [TestMethod]
        public void IsMet_GivenMultipleGroups_ReturnsResult()
        {
            // Arrange
            var commonServiceMock = new Mock<IServiceCommon<TestEntityWithTwoGroups, ITestEntityWithTwoGroups, long>>();
            var attrib = typeof(TestEntityWithTwoGroups).GetProperties<DistinctPropertyAttribute>();
            var distinctPropertiesMustBeUniqueRule = new DistinctPropertiesMustBeUniqueRule<TestEntityWithTwoGroups, ITestEntityWithTwoGroups, long>(
                                TestEntitiesTwoGroups,
                                attrib,
                                commonServiceMock.Object,
                                ChangeType.Create);
            // Act
            var result = distinctPropertiesMustBeUniqueRule.IsMet();

            // Assert
            Assert.AreEqual(result.Result, true);
        }

        [TestMethod]
        public void IsMet_GivenMultipleGroupsWithDuplicateEntities_Fails()
        {
            // Arrange
            var commonServiceMock = new Mock<IServiceCommon<TestEntityWithTwoGroups, ITestEntityWithTwoGroups, long>>();
            var attrib = typeof(TestEntityWithTwoGroups).GetProperties<DistinctPropertyAttribute>();
            var distinctPropertiesMustBeUniqueRule = new DistinctPropertiesMustBeUniqueRule<TestEntityWithTwoGroups, ITestEntityWithTwoGroups, long>(
                                TestEntitiesTwoGroupsAndDuplicates,
                                attrib, 
                                commonServiceMock.Object,
                                ChangeType.Create);

            // Act
            var result = distinctPropertiesMustBeUniqueRule.IsMet();

            // Assert
            Assert.AreEqual(result.Result, false);
        }
        #endregion

        #region Update
        [TestMethod]
        public void DistinctPropertiesMustBeUniqueRule_IsMet_Update_Test()
        {
            // Arrange
            var commonServiceMock = new Mock<IServiceCommon<TestEntityWithOneGroup, ITestEntityWithOneGroup, long>>();
            Expression<Func<TestEntityWithOneGroup, bool>> actualExpression = null;
            var getRet = new ITestEntityWithOneGroup[] { };
            commonServiceMock.Setup(m=>m.Get(It.IsAny<Expression<Func<TestEntityWithOneGroup, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                             .Returns((Expression<Func<TestEntityWithOneGroup, bool>> ex, int i1, int i2) => 
                             {
                                 actualExpression = ex;
                                 return getRet.AsQueryable();
                             });
            var attrib = typeof(TestEntityWithOneGroup).GetProperties<DistinctPropertyAttribute>();
            var distinctPropertiesMustBeUniqueRule = new DistinctPropertiesMustBeUniqueRule<TestEntityWithOneGroup, ITestEntityWithOneGroup, long>(
                                UpdatedEntity,
                                attrib,
                                commonServiceMock.Object,
                                ChangeType.Update);

            // Act
            var result = distinctPropertiesMustBeUniqueRule.IsMet();

            // Assert
            Assert.AreEqual(result.Result, true);
            Assert.IsTrue(actualExpression.ToString().Contains("AndAlso Not("));
        }
        #endregion
    }
}