using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Business
{
    [TestClass]
    public class DuplicateEntityPreventerTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityInfoAltKey<AltKeyEntity, string>> _MockEntityInfoAltKey;
        private Mock<IGetByPropertyValuesHandler<AltKeyEntity, IAltKeyEntity, int>> _MockGetByPropertyValuesHandler;
        private AlternateKeyTracker<AltKeyEntity, string> _AlternateKeyTracker;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityInfoAltKey = _MockRepository.Create<IEntityInfoAltKey<AltKeyEntity, string>>();
            _MockGetByPropertyValuesHandler = _MockRepository.Create<IGetByPropertyValuesHandler<AltKeyEntity, IAltKeyEntity, int>>();
            _AlternateKeyTracker = new AlternateKeyTracker<AltKeyEntity, string>();
        }

        private DuplicateEntityPreventer<AltKeyEntity, IAltKeyEntity, int, string> CreateDuplicateEntityPreventer()
        {
            return new DuplicateEntityPreventer<AltKeyEntity, IAltKeyEntity, int, string>(
                _MockEntityInfoAltKey.Object,
                _MockGetByPropertyValuesHandler.Object,
                _AlternateKeyTracker);
        }

        #region Check
        [TestMethod]
        public async Task AddAltKeyHandler_Add_Both_DuplicatesInPost_ThrowsException_Test()
        {
            // Arrange
            var service = CreateDuplicateEntityPreventer();
            var entity1 = new AltKeyEntity { Id = 101, Name = "Duplicate1" };
            var entity2 = new AltKeyEntity { Id = 102, Name = "Duplicate1" };
            var entities = new[] { entity1, entity2 };
            var entityInfoAltKey = new EntityInfoAltKey<AltKeyEntity, string>();
            _MockEntityInfoAltKey.Setup(m => m.PropertyExpressionMethod)
                                 .Returns(entityInfoAltKey.PropertyExpressionMethod);
            _MockEntityInfoAltKey.Setup(m => m.AlternateKeyProperty)
                                 .Returns(entityInfoAltKey.AlternateKeyProperty);

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<DuplicateKeyException>(async () =>
            {
                await service.CheckAsync(entities);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task AddAltKeyHandler_Add_Some_DuplicateInPost_ThrowsException_Test()
        {
            // Arrange
            var service = CreateDuplicateEntityPreventer();
            var entity1 = new AltKeyEntity { Id = 101, Name = "Duplicate1" };
            var entity2 = new AltKeyEntity { Id = 102, Name = "Duplicate1" };
            var entity3 = new AltKeyEntity { Id = 103, Name = "NotDuplicate1" };
            var entities = new[] { entity1, entity2, entity3 };
            var entityInfoAltKey = new EntityInfoAltKey<AltKeyEntity, string>();
            _MockEntityInfoAltKey.Setup(m => m.PropertyExpressionMethod)
                                 .Returns(entityInfoAltKey.PropertyExpressionMethod);
            _MockEntityInfoAltKey.Setup(m => m.AlternateKeyProperty)
                                 .Returns(entityInfoAltKey.AlternateKeyProperty);
            IQueryable<IAltKeyEntity> queryable = null;
            _MockGetByPropertyValuesHandler.Setup(m => m.GetAsync<string>(nameof(AltKeyEntity.Name),
                                                                          It.Is<IEnumerable<string>>(e => e.First() == entity3.Name),
                                                                          null))
                                           .ReturnsAsync(queryable);

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<DuplicateKeyException>(async () =>
            {
                await service.CheckAsync(entities);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task AddAltKeyHandler_Add_Some_DuplicateInSimultaneousCalls_ThrowsException_Test()
        {
            // Arrange
            var service = CreateDuplicateEntityPreventer();
            var entity1 = new AltKeyEntity { Id = 101, Name = "E_101" };
            var entity2 = new AltKeyEntity { Id = 102, Name = "E_102" };
            var entity3 = new AltKeyEntity { Id = 103, Name = "E_103" };
            var entities = new[] { entity1, entity2, entity3 };
            var entityInfoAltKey = new EntityInfoAltKey<AltKeyEntity, string>();
            _MockEntityInfoAltKey.Setup(m => m.PropertyExpressionMethod)
                                 .Returns(entityInfoAltKey.PropertyExpressionMethod);
            _MockEntityInfoAltKey.Setup(m => m.AlternateKeyProperty)
                                 .Returns(entityInfoAltKey.AlternateKeyProperty);
            IQueryable<IAltKeyEntity> queryable = null;
            _MockGetByPropertyValuesHandler.Setup(m => m.GetAsync<string>(nameof(AltKeyEntity.Name),
                                                                          It.IsAny<IEnumerable<string>>(),
                                                                          null))
                                           .ReturnsAsync((string property, IEnumerable<string> values, NameValueCollection parameters) => 
                                           {
                                               Task.Delay(15);
                                               return queryable;
                                           });

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<DuplicateKeyException>(async () =>
            {
                var task1 = service.CheckAsync(entities);
                var task2 = service.CheckAsync(entities);
                await task1;
                await task2;
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task AddAltKeyHandler_Add_DuplicateInRepo_ThrowsException_Test()
        {
            // Arrange
            var service = CreateDuplicateEntityPreventer();
            var entity = new AltKeyEntity { Id = 101, Name = "Duplicate1" };

            var entityInfoAltKey = new EntityInfoAltKey<AltKeyEntity, string>();
            _MockEntityInfoAltKey.Setup(m => m.PropertyExpressionMethod)
                                 .Returns(entityInfoAltKey.PropertyExpressionMethod);
            _MockEntityInfoAltKey.Setup(m => m.AlternateKeyProperty)
                                 .Returns(entityInfoAltKey.AlternateKeyProperty);

            _MockGetByPropertyValuesHandler.Setup(m => m.GetAsync<string>(nameof(AltKeyEntity.Name),
                                                                          It.Is<IEnumerable<string>>(e => e.First() == entity.Name),
                                                                          null))
                                           .ReturnsAsync(new[] { entity }.AsQueryable());


            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<DuplicateKeyException>(async () =>
            {
                await service.CheckAsync(new[] { entity });
            });
            _MockRepository.VerifyAll();
        }
        #endregion

        #region RemoveTracked
        [TestMethod]
        public void DuplicateEntityPreventer_RemoveTracked_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var duplicateEntityPreventer = CreateDuplicateEntityPreventer();
            var entity1 = new AltKeyEntity { Id = 101, Name = "E_101" };
            var entity2 = new AltKeyEntity { Id = 102, Name = "E_102" };
            var entity3 = new AltKeyEntity { Id = 103, Name = "E_103" };
            var entities = new[] { entity1, entity2, entity3 };
            foreach (var entity in entities)
                _AlternateKeyTracker.Add(entity.Name);

            var entityInfoAltKey = new EntityInfoAltKey<AltKeyEntity, string>();
            _MockEntityInfoAltKey.Setup(m => m.PropertyExpressionMethod)
                                 .Returns(entityInfoAltKey.PropertyExpressionMethod);

            // Act
            duplicateEntityPreventer.RemoveTracked(entities);

            // Assert
            Assert.AreEqual(0, _AlternateKeyTracker.Count);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
