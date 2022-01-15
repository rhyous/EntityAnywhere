using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Collections;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Tools;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Interfaces.Common.Tests
{
    [TestClass]
    public class PropertyOrderSorterTests
    {
        private PropertyOrderSorter CreatePropertyOrderSorter()
        {
            return new PropertyOrderSorter(
                new PreferentialPropertyComparer());
        }

        [TestMethod]
        public void PropertyOrderSorter_Sort_Test()
        {
            // Arrange
            var unitUnderTest = CreatePropertyOrderSorter();
            var ep1 = new EntityProperty { Id = 1, Name = "Id", Order = int.MaxValue, EntityId = 27 };
            var ep2 = new EntityProperty { Id = 2, Name = "Name", Order = int.MaxValue, EntityId = 27 };
            var ep3 = new EntityProperty { Id = 3, Name = "Prop3", Order = int.MaxValue, EntityId = 27 };
            var ep4 = new EntityProperty { Id = 4, Name = "Prop4", Order = int.MaxValue, EntityId = 27 };
            var ep5 = new EntityProperty { Id = 5, Name = "Prop5", Order = int.MaxValue, EntityId = 27 };
            List<EntityProperty> list = new List<EntityProperty> 
                                        { ep1,ep2, ep3, ep4, ep5 };
            list.Shuffle();

            // Act
            var result = unitUnderTest.Sort(
                            list,
                            e => e.EntityId,
                            e => e.Order,
                            e => e.Name);
            var sorted = result[27];

            // Assert
            Assert.AreEqual(sorted[0], ep1, "ep1 is 0");
            Assert.AreEqual(sorted[1], ep2, "ep2 is 1");
            Assert.AreEqual(sorted[2], ep3, "ep3 is 2");
            Assert.AreEqual(sorted[3], ep4, "ep4 is 3");
            Assert.AreEqual(sorted[4], ep5, "ep5 is 4");
        }

        [TestMethod]
        public void PropertyOrderSorter_Sort_TwoEntityIdGroups_Test()
        {
            // Arrange
            var unitUnderTest = CreatePropertyOrderSorter();
            var ep1a = new EntityProperty { Id = 1, Name = "Id", Order = int.MaxValue, EntityId = 27 };
            var ep2a = new EntityProperty { Id = 2, Name = "Name", Order = int.MaxValue, EntityId = 27 };
            var ep3a = new EntityProperty { Id = 3, Name = "Prop3", Order = int.MaxValue, EntityId = 27 };
            var ep4a = new EntityProperty { Id = 4, Name = "Prop4", Order = int.MaxValue, EntityId = 27 };
            var ep5a = new EntityProperty { Id = 5, Name = "Prop5", Order = int.MaxValue, EntityId = 27 };
            var ep1b = new EntityProperty { Id = 1, Name = "Id", Order = int.MaxValue, EntityId = 81 };
            var ep2b = new EntityProperty { Id = 2, Name = "Name", Order = int.MaxValue, EntityId = 81 };
            var ep3b = new EntityProperty { Id = 3, Name = "Prop3", Order = int.MaxValue, EntityId = 81 };
            var ep4b = new EntityProperty { Id = 4, Name = "Prop4", Order = int.MaxValue, EntityId = 81 };
            var ep5b = new EntityProperty { Id = 5, Name = "Prop5", Order = int.MaxValue, EntityId = 81 };

            List<EntityProperty> list = new List<EntityProperty>()
                                        { ep1a, ep2a, ep3a, ep4a, ep5a,
                                          ep1b, ep2b, ep3b, ep4b, ep5b };
            list.Shuffle();

            // Act
            var result = unitUnderTest.Sort(
                            list,
                            e => e.EntityId,
                            e => e.Order,
                            e => e.Name);
            var sorted1 = result[27];
            var sorted2 = result[81];

            // Assert
            Assert.AreEqual(sorted1[0], ep1a, "ep1a is 0");
            Assert.AreEqual(sorted1[1], ep2a, "ep2a is 1");
            Assert.AreEqual(sorted1[2], ep3a, "ep3a is 2");
            Assert.AreEqual(sorted1[3], ep4a, "ep4a is 3");
            Assert.AreEqual(sorted1[4], ep5a, "ep5a is 4");

            Assert.AreEqual(sorted2[0], ep1b, "ep1 is 0");
            Assert.AreEqual(sorted2[1], ep2b, "ep2 is 1");
            Assert.AreEqual(sorted2[2], ep3b, "ep3 is 2");
            Assert.AreEqual(sorted2[3], ep4b, "ep4 is 3");
            Assert.AreEqual(sorted2[4], ep5b, "ep5 is 4");
        }

        [TestMethod]
        public void PropertyOrderSorter_UpdateSortOrder_Test()
        {
            // Arrange
            var unitUnderTest = CreatePropertyOrderSorter();
            var ep1 = new EntityProperty { Id = 1, Name = "Id", Order = int.MaxValue, EntityId = 27 };
            var ep2 = new EntityProperty { Id = 2, Name = "Name", Order = int.MaxValue, EntityId = 27 };
            var ep3 = new EntityProperty { Id = 3, Name = "Prop3", Order = int.MaxValue, EntityId = 27 };
            var ep4 = new EntityProperty { Id = 4, Name = "Prop4", Order = int.MaxValue, EntityId = 27 };
            var ep5 = new EntityProperty { Id = 5, Name = "Prop5", Order = int.MaxValue, EntityId = 27 };
            var list = new List<EntityProperty> { ep1, ep2, ep3, ep4, ep5 };

            // Act
            var result = unitUnderTest.UpdateSortOrder(list)
                                      .ToList();

            // Assert
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(1, ep1.Order, "ep1 is 1");
            Assert.AreEqual(2, ep2.Order, "ep2 is 2");
            Assert.AreEqual(3, ep3.Order, "ep3 is 3");
            Assert.AreEqual(4, ep4.Order, "ep4 is 4");
            Assert.AreEqual(5, ep5.Order, "ep5 is 5");
        }

        [TestMethod]
        public void PropertyOrderSorter_UpdateSortOrder_SameOrderNotChanged_Test()
        {
            // Arrange
            var unitUnderTest = CreatePropertyOrderSorter();
            var ep1 = new EntityProperty { Id = 1, Name = "Id", Order = 1, EntityId = 27 };
            var ep2 = new EntityProperty { Id = 2, Name = "Name", Order = 2, EntityId = 27 };
            var ep3 = new EntityProperty { Id = 3, Name = "Prop3", Order = int.MaxValue, EntityId = 27 };
            var ep4 = new EntityProperty { Id = 4, Name = "Prop4", Order = int.MaxValue, EntityId = 27 };
            var ep5 = new EntityProperty { Id = 5, Name = "Prop5", Order = int.MaxValue, EntityId = 27 };
            var list = new List<EntityProperty> { ep1, ep2, ep3, ep4, ep5 };

            // Act
            var result = unitUnderTest.UpdateSortOrder(list)
                                      .ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(1, ep1.Order, "ep1 is 1");
            Assert.AreEqual(2, ep2.Order, "ep2 is 2");
            Assert.AreEqual(3, ep3.Order, "ep3 is 3");
            Assert.AreEqual(4, ep4.Order, "ep4 is 4");
            Assert.AreEqual(5, ep5.Order, "ep5 is 5");
        }

        [TestMethod]
        public void Collate_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var epIda = new EntityProperty { Id = 1, Name = "Id", Order = int.MaxValue, EntityId = 27 };
            var epNamea = new EntityProperty { Id = 2, Name = "Name", Order = int.MaxValue, EntityId = 27 };
            var ep3a = new EntityProperty { Id = 3, Name = "Prop3", Order = int.MaxValue, EntityId = 27 };
            var ep4a = new EntityProperty { Id = 4, Name = "Prop4", Order = int.MaxValue, EntityId = 27 };
            var ep5a = new EntityProperty { Id = 5, Name = "Prop5", Order = int.MaxValue, EntityId = 27 };
            var epIdb = new EntityProperty { Id = 1, Name = "Id", Order = int.MaxValue, EntityId = 81 };
            var epNameb = new EntityProperty { Id = 2, Name = "Name", Order = int.MaxValue, EntityId = 81 };
            var ep3b = new EntityProperty { Id = 3, Name = "Prop3", Order = int.MaxValue, EntityId = 81 };
            var ep4b = new EntityProperty { Id = 4, Name = "Prop4", Order = int.MaxValue, EntityId = 81 };
            var ep5b = new EntityProperty { Id = 5, Name = "Prop5", Order = int.MaxValue, EntityId = 81 };

            var map = new Dictionary<int, List<EntityProperty>>
            {
                {27, new List<EntityProperty> { epIda, epNamea, ep3a, ep4a, ep5a } },
                {81, new List<EntityProperty> { epIdb, epNameb, ep3b, ep4b, ep5b } }
            };
            var ep1a = new EntityProperty { Name = "Prop1", Order = 3, EntityId = 27 };
            var ep2a = new EntityProperty { Name = "Prop2", Order = 4, EntityId = 27 };
            var ep1b = new EntityProperty { Name = "Prop1", Order = 3, EntityId = 81 };
            var ep2b = new EntityProperty { Name = "Prop2", Order = 4, EntityId = 81 };

            var addMap = new Dictionary<int, List<EntityProperty>>
            { 
                {27, new List<EntityProperty> {  ep1a, ep2a } },
                { 81, new List<EntityProperty> { ep1b, ep2b } }
            };
            var unitUnderTest = CreatePropertyOrderSorter();

            // Act
            unitUnderTest.Collate(
                map,
                addMap,
                e => e.EntityId);

            // Assert
            Assert.AreEqual(map[27][0], epIda, "epIda is 0");
            Assert.AreEqual(map[27][1], epNamea, "epNamea is 1");
            Assert.AreEqual(map[27][2], ep1a, "ep1a is 2");
            Assert.AreEqual(map[27][3], ep2a, "ep2a is 3");
            Assert.AreEqual(map[27][4], ep3a, "ep3a is 4");
            Assert.AreEqual(map[27][5], ep4a, "ep4a is 5");
            Assert.AreEqual(map[27][6], ep5a, "ep5a is 6");

            Assert.AreEqual(map[81][0], epIdb, "epIdb is 0");
            Assert.AreEqual(map[81][1], epNameb, "epNameb is 1");
            Assert.AreEqual(map[81][2], ep1b, "ep1b is 2");
            Assert.AreEqual(map[81][3], ep2b, "ep2b is 3");
            Assert.AreEqual(map[81][4], ep3b, "ep3b is 4");
            Assert.AreEqual(map[81][5], ep4b, "ep4b is 5");
            Assert.AreEqual(map[81][6], ep5b, "ep5b is 6");
        }
    }
}
