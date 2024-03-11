using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Collections;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.Tests
{
    [TestClass]
    public class EntityPropertyServiceTests
    {
        private MockRepository _MockRepository;

        private Mock<IServiceHandlerProvider> _MockServiceHandlerProvider;
        private IPropertyOrderSorter _PropertyOrderSorter;
        private Mock<IAddHandler<EntityProperty, IEntityProperty, int>> _MockAddHandler;
        private Mock<IDeleteHandler<EntityProperty, IEntityProperty, int>> _MockDeleteHandler;
        private Mock<IGetByIdHandler<EntityProperty, IEntityProperty, int>> _MockGetByIdHandler;
        private Mock<IQueryableHandler<EntityProperty, IEntityProperty, int>> _MockQueryableHandler;
        private Mock<IUpdateHandler<EntityProperty, IEntityProperty, int>> _MockUpdateHandler;
        private Mock<ILogger> _MockLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockServiceHandlerProvider = _MockRepository.Create<IServiceHandlerProvider>();
            _PropertyOrderSorter = new PropertyOrderSorter(PreferentialPropertyComparer.Instance);
            _MockAddHandler = _MockRepository.Create<IAddHandler<EntityProperty, IEntityProperty, int>>();
            _MockDeleteHandler = _MockRepository.Create<IDeleteHandler<EntityProperty, IEntityProperty, int>>();
            _MockGetByIdHandler = _MockRepository.Create<IGetByIdHandler<EntityProperty, IEntityProperty, int>>();
            _MockQueryableHandler = _MockRepository.Create<IQueryableHandler<EntityProperty, IEntityProperty, int>>();
            _MockUpdateHandler = _MockRepository.Create<IUpdateHandler<EntityProperty, IEntityProperty, int>>();
            _MockLogger = _MockRepository.Create<ILogger>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            //mockRepository.VerifyAll();
        }

        private EntityPropertyService CreateService()
        {
            return new EntityPropertyService(_MockServiceHandlerProvider.Object, _PropertyOrderSorter);
        }

        #region Delete
        [TestMethod]
        public void EntityPropertyService_Delete_Test()
        {
            // Arrange
            var unitUnderTest = CreateService();

            _MockServiceHandlerProvider.Setup(m => m.Provide<IDeleteHandler<EntityProperty, IEntityProperty, int>>()).Returns(_MockDeleteHandler.Object);
            _MockServiceHandlerProvider.Setup(m => m.Provide<IGetByIdHandler<EntityProperty, IEntityProperty, int>>()).Returns(_MockGetByIdHandler.Object);
            _MockServiceHandlerProvider.Setup(m => m.Provide<IQueryableHandler<EntityProperty, IEntityProperty, int>>()).Returns(_MockQueryableHandler.Object);
            _MockServiceHandlerProvider.Setup(m => m.Provide<IUpdateHandler<EntityProperty, IEntityProperty, int>>()).Returns(_MockUpdateHandler.Object);

            var ep1 = new EntityProperty { Id = 1, Name = "Id", Order = 1, EntityId = 27 };
            var ep2 = new EntityProperty { Id = 2, Name = "Name", Order = 2, EntityId = 27 };
            var ep3 = new EntityProperty { Id = 3, Name = "Prop3", Order = 3, EntityId = 27 };
            var ep4 = new EntityProperty { Id = 4, Name = "Prop4", Order = 4, EntityId = 27 };
            var ep5 = new EntityProperty { Id = 5, Name = "Prop5", Order = 5, EntityId = 27 };
            var epsRemaining = new List<IEntityProperty> { ep1, ep2, ep4, ep5 };
            _MockGetByIdHandler.Setup(m => m.Get(It.IsAny<int>()))
                    .Returns(ep3);
            _MockDeleteHandler.Setup(m => m.Delete(It.IsAny<int>()))
                    .Returns(true);
            _MockQueryableHandler.Setup(m => m.GetQueryable(
                                 It.IsAny<Expression<Func<EntityProperty, bool>>>(),
                                 -1,
                                 -1,
                                 nameof(EntityProperty.Order),
                                 SortOrder.Ascending))
                    .Returns(epsRemaining.AsQueryable());
            _MockUpdateHandler.Setup(m => m.Update(It.IsAny<IEnumerable<IEntityProperty>>(), nameof(EntityProperty.Order)))
                              .Returns((IEnumerable<IEntityProperty> inEps, string[] props) => { return inEps.ToList(); });

            // Act
            var result = unitUnderTest.Delete(ep3.Id);

            // Assert
            Assert.AreEqual(1, ep1.Order, "ep1 did not change.");
            Assert.AreEqual(2, ep2.Order, "ep2 did not change.");
            Assert.AreEqual(3, ep4.Order, "ep4 is now Order 3.");
            Assert.AreEqual(4, ep5.Order, "ep5 is now Order 4.");
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Add
        [TestMethod]
        public async Task EntityPropertyService_Add_AllNew_Test()
        {
            // Arrange
            var unitUnderTest = CreateService();
            
            _MockServiceHandlerProvider.Setup(m => m.Provide<IQueryableHandler<EntityProperty, IEntityProperty, int>>()).Returns(_MockQueryableHandler.Object);
            _MockServiceHandlerProvider.Setup(m => m.Provide<IAddHandler<EntityProperty, IEntityProperty, int>>()).Returns(_MockAddHandler.Object);

            var epId = new EntityProperty { Name = "Id", Order = int.MaxValue };
            var epName = new EntityProperty { Name = "Name", Order = int.MaxValue };
            var ep1 = new EntityProperty { Name = "Prop1", Order = int.MaxValue };
            var ep2 = new EntityProperty { Name = "Prop2", Order = int.MaxValue };
            var ep3 = new EntityProperty { Name = "Prop3", Order = int.MaxValue };
            var ep4 = new EntityProperty { Name = "Prop4", Order = int.MaxValue };
            var ep5 = new EntityProperty { Name = "Prop5", Order = int.MaxValue };
            var epsToAdd = new List<IEntityProperty> { epId, epName, ep1, ep2, ep3, ep4, ep5 };
            epsToAdd.Shuffle();

            _MockQueryableHandler.Setup(m => m.GetQueryable(It.IsAny<Expression<Func<EntityProperty, bool>>>(), -1, -1, nameof(EntityProperty.Id), SortOrder.Ascending))
                                 .Returns(Array.Empty<IEntityProperty>().AsQueryable());

            _MockAddHandler.Setup(m => m.AddAsync(It.IsAny<IOrderedEnumerable<IEntityProperty>>()))
                           .ReturnsAsync(epsToAdd);

            // Act
            var result = await unitUnderTest.AddAsync(epsToAdd);

            // Assert
            Assert.AreEqual(1, epId.Order, "epId is now Order 1.");
            Assert.AreEqual(2, epName.Order, "epName is now Order 2.");
            Assert.AreEqual(3, ep1.Order, "ep1 is now Order 3.");
            Assert.AreEqual(4, ep2.Order, "ep2 is now Order 4.");
            Assert.AreEqual(5, ep3.Order, "ep3 is now Order 5.");
            Assert.AreEqual(6, ep4.Order, "ep4 is now Order 6.");
            Assert.AreEqual(7, ep5.Order, "ep5 is now Order 7.");
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task EntityPropertyService_Add_AllNew_PreferentialPropertyComparer_Test()
        {
            // Arrange
            var unitUnderTest = CreateService();
            
            _MockServiceHandlerProvider.Setup(m => m.Provide<IQueryableHandler<EntityProperty, IEntityProperty, int>>()).Returns(_MockQueryableHandler.Object);
            _MockServiceHandlerProvider.Setup(m => m.Provide<IAddHandler<EntityProperty, IEntityProperty, int>>()).Returns(_MockAddHandler.Object);

            var epId = new EntityProperty { Name = "Id", Order = int.MaxValue };
            var epName = new EntityProperty { Name = "Name", Order = int.MaxValue };
            var ep1 = new EntityProperty { Name = "Prop1", Order = int.MaxValue };
            var ep2 = new EntityProperty { Name = "Prop2", Order = int.MaxValue };
            var ep3 = new EntityProperty { Name = "Prop3", Order = int.MaxValue };
            var ep4 = new EntityProperty { Name = "Prop4", Order = int.MaxValue };
            var ep5 = new EntityProperty { Name = "Prop5", Order = int.MaxValue };
            var epCreateDate = new EntityProperty { Name = "CreateDate", Order = int.MaxValue };
            var epCreatedBy = new EntityProperty { Name = "CreatedBy", Order = int.MaxValue };
            var epLastUpdated = new EntityProperty { Name = "LastUpdated", Order = int.MaxValue };
            var epLastUpdatedBy = new EntityProperty { Name = "LastUpdatedBy", Order = int.MaxValue };
            var epsToAdd = new List<IEntityProperty> { epId, epName, ep1, ep2, ep3, ep4, ep5, epCreateDate, epCreatedBy, epLastUpdated, epLastUpdatedBy };
            epsToAdd.Shuffle();

            _MockQueryableHandler.Setup(m => m.GetQueryable(It.IsAny<Expression<Func<EntityProperty, bool>>>(), -1, -1, nameof(EntityProperty.Id), SortOrder.Ascending))
                                 .Returns(Array.Empty<IEntityProperty>().AsQueryable());

            _MockAddHandler.Setup(m => m.AddAsync(It.IsAny<IOrderedEnumerable<IEntityProperty>>()))
                           .ReturnsAsync(epsToAdd);

            // Act
            var result = await unitUnderTest.AddAsync(epsToAdd);

            // Assert
            int order = 1;
            Assert.AreEqual(order, epId.Order, $"{nameof(EntityProperty.Id)} is now Order {order++}.");
            Assert.AreEqual(order, epName.Order, $"{nameof(EntityProperty.Name)} is now Order {order++}.");
            Assert.AreEqual(order, ep1.Order, $"Prop1 is now Order {order++}.");
            Assert.AreEqual(order, ep2.Order, $"Prop2 is now Order {order++}.");
            Assert.AreEqual(order, ep3.Order, $"Prop3 is now Order {order++}.");
            Assert.AreEqual(order, ep4.Order, $"Prop4 is now Order {order++}.");
            Assert.AreEqual(order, ep5.Order, $"Prop5 is now Order {order++}.");
            Assert.AreEqual(order, epCreateDate.Order, $"{nameof(EntityProperty.CreateDate)} is now Order {order++}.");
            Assert.AreEqual(order, epCreatedBy.Order, $"{nameof(EntityProperty.CreatedBy)} is now Order {order++}.");
            Assert.AreEqual(order, epLastUpdated.Order, $"{nameof(EntityProperty.LastUpdated)} is now Order {order++}.");
            Assert.AreEqual(order, epLastUpdatedBy.Order, $"{nameof(EntityProperty.LastUpdatedBy)} is now Order {order++}.");
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task EntityPropertyService_Add_Middle_Test()
        {
            // Arrange

            _MockServiceHandlerProvider.Setup(m => m.Provide<IAddHandler<EntityProperty, IEntityProperty, int>>()).Returns(_MockAddHandler.Object); 
            _MockServiceHandlerProvider.Setup(m => m.Provide<IQueryableHandler<EntityProperty, IEntityProperty, int>>()).Returns(_MockQueryableHandler.Object);            
            _MockServiceHandlerProvider.Setup(m => m.Provide<IUpdateHandler<EntityProperty, IEntityProperty, int>>()).Returns(_MockUpdateHandler.Object);

            var unitUnderTest = CreateService();
            var epId = new EntityProperty { Name = "Id", Order = 1 };
            var epName = new EntityProperty { Name = "Name", Order = 2 };
            var ep3 = new EntityProperty { Name = "Prop3", Order = 3 };
            var ep4 = new EntityProperty { Name = "Prop4", Order = 4 };
            var ep5 = new EntityProperty { Name = "Prop5", Order = 5 };
            var epExisting = new[] { epId, epName, ep3, ep4, ep5 };

            var ep1 = new EntityProperty { Name = "Prop1", Order = 3 };
            var ep2 = new EntityProperty { Name = "Prop2", Order = 4 };
            var epsToAdd = new List<IEntityProperty> { ep1, ep2 };

            _MockQueryableHandler.Setup(m => m.GetQueryable(It.IsAny<Expression<Func<EntityProperty, bool>>>(), -1, -1, nameof(EntityProperty.Id), SortOrder.Ascending))
                                 .Returns(epExisting.AsQueryable());

            _MockAddHandler.Setup(m => m.AddAsync(It.IsAny<IOrderedEnumerable<IEntityProperty>>()))
                           .ReturnsAsync(epsToAdd);

            _MockUpdateHandler.Setup(m => m.Update(It.IsAny<IEnumerable<IEntityProperty>>(), nameof(EntityProperty.Order)))
                              .Returns((IEnumerable<IEntityProperty> inEps, string[] props) => { return inEps.ToList(); });

            // Act
            var result = await unitUnderTest.AddAsync(epsToAdd);

            // Assert
            Assert.AreEqual(1, epId.Order, "epId is now Order 1.");
            Assert.AreEqual(2, epName.Order, "epName is now Order 2.");
            Assert.AreEqual(3, ep1.Order, "ep1 is now Order 3.");
            Assert.AreEqual(4, ep2.Order, "ep2 is now Order 4.");
            Assert.AreEqual(5, ep3.Order, "ep3 is now Order 5.");
            Assert.AreEqual(6, ep4.Order, "ep4 is now Order 6.");
            Assert.AreEqual(7, ep5.Order, "ep5 is now Order 7.");
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Update
        [TestMethod]
        public void EntityPropertyService_Update_OtherProperty_Test()
        {
            // Arrange
            var unitUnderTest = CreateService();
            
            _MockServiceHandlerProvider.Setup(m => m.Provide<IUpdateHandler<EntityProperty, IEntityProperty, int>>()).Returns(_MockUpdateHandler.Object);

            var ep1 = new EntityProperty { Id = 105, Name = "Prop1", Order = 5, EntityId = 27 };
            _MockUpdateHandler.Setup(m => m.Update(ep1.Id, It.Is<PatchedEntity<IEntityProperty, int>>(pe => pe.Entity == ep1)))
                              .Returns(ep1);
            var patchedEntity = new PatchedEntity<IEntityProperty, int>
            {
                Entity = ep1,
                ChangedProperties = new HashSet<string> { "Name" }
            };

            // Act
            unitUnderTest.Update(ep1.Id, patchedEntity);

            // Assert
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void EntityPropertyService_Update_Move5to3_Test()
        {
            // Arrange
            var unitUnderTest = CreateService();
            
            _MockServiceHandlerProvider.Setup(m => m.Provide<IGetByIdHandler<EntityProperty, IEntityProperty, int>>()).Returns(_MockGetByIdHandler.Object);
            _MockServiceHandlerProvider.Setup(m => m.Provide<IQueryableHandler<EntityProperty, IEntityProperty, int>>()).Returns(_MockQueryableHandler.Object);
            _MockServiceHandlerProvider.Setup(m => m.Provide<IUpdateHandler<EntityProperty, IEntityProperty, int>>()).Returns(_MockUpdateHandler.Object);

            var epId = new EntityProperty { Id = 101, Name = "Id", Order = 1, EntityId = 27 };
            var epName = new EntityProperty { Id = 102, Name = "Name", Order = 2, EntityId = 27 };
            var ep2 = new EntityProperty { Id = 103, Name = "Prop2", Order = 3, EntityId = 27 };
            var ep3 = new EntityProperty { Id = 104, Name = "Prop3", Order = 4, EntityId = 27 };
            var ep1 = new EntityProperty { Id = 105, Name = "Prop1", Order = 5, EntityId = 27 };
            var ep4 = new EntityProperty { Id = 106, Name = "Prop4", Order = 6, EntityId = 27 };
            var ep5 = new EntityProperty { Id = 107, Name = "Prop5", Order = 7, EntityId = 27 };
            var epExisting = new[] { epId, epName, ep2, ep3, ep4, ep5 }; // Leave out ep1

            var ep1updated = new EntityProperty { Id = 105, Name = "Prop1", Order = 3, EntityId = 27 };

            _MockGetByIdHandler.Setup(m => m.Get(ep1updated.Id))
                    .Returns(ep1);
            _MockQueryableHandler.Setup(m => m.GetQueryable(
                                 It.IsAny<Expression<Func<EntityProperty, bool>>>(),
                                 -1,
                                 -1,
                                 nameof(EntityProperty.Order),
                                 It.IsAny<SortOrder>()))
                    .Returns(epExisting.AsQueryable());

            _MockUpdateHandler.Setup(m => m.Update(ep1updated.Id, It.Is<PatchedEntity<IEntityProperty, int>>(pe => pe.Entity == ep1updated)))
                              .Returns(ep1updated);

            _MockUpdateHandler.Setup(m => m.Update(It.IsAny<IEnumerable<IEntityProperty>>(), nameof(EntityProperty.Order)))
                              .Returns((IEnumerable<IEntityProperty> inEps, string[] props) => { return inEps.ToList(); });

            var patchedEntity = new PatchedEntity<IEntityProperty, int>
            {
                Entity = ep1updated,
                ChangedProperties = new HashSet<string> { "Order" }
            };

            // Act
            var result = unitUnderTest.Update(105, patchedEntity);

            // Assert
            Assert.AreEqual(1, epId.Order, "epId is now Order 1.");
            Assert.AreEqual(2, epName.Order, "epName is now Order 2.");
            Assert.AreEqual(3, ep1updated.Order, "ep1 is now Order 3.");
            Assert.AreEqual(4, ep2.Order, "ep2 is now Order 4.");
            Assert.AreEqual(5, ep3.Order, "ep3 is now Order 5.");
            Assert.AreEqual(6, ep4.Order, "ep4 is now Order 6.");
            Assert.AreEqual(7, ep5.Order, "ep5 is now Order 7.");
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
