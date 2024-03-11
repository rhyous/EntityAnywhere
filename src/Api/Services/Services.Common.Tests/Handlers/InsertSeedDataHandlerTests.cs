using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services.Common.Tests.TestModels;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.Product;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IProduct;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Handlers
{
    [TestClass]
    public class InsertSeedDataHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IRepository<TEntity, TInterface, TId>> _MockEntityRepo;
        private Mock<IUpdateHandler<TEntity, TInterface, TId>> _MockUpdateHandler;
        private Mock<IUrlParameters> _MockUrlParameters;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityRepo = _MockRepository.Create<IRepository<TEntity, TInterface, TId>>();
            _MockUpdateHandler = _MockRepository.Create<IUpdateHandler<TEntity, TInterface, TId>>();
            _MockUrlParameters = _MockRepository.Create<IUrlParameters>();
        }

        private InsertSeedDataHandler<TEntity, TInterface, TId> CreateInsertSeedDataHandler()
        {
            return new InsertSeedDataHandler<TEntity, TInterface, TId>(
                _MockEntityRepo.Object, 
                _MockUpdateHandler.Object,
                _MockUrlParameters.Object);
        }

        private void SetOverwriteUrlParameterValue(bool? allowOverwrite = null)
        {
            NameValueCollection collection = new NameValueCollection();

            if (allowOverwrite.HasValue)
                collection = new NameValueCollection { { "Overwrite", allowOverwrite.ToString() } };

            _MockUrlParameters.Setup(m => m.Collection).Returns(collection);
        }

        #region InsertSeedData

        [TestMethod]
        public void InsertSeedDataHandler_InsertSeedData_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var insertSeedDataHandler = CreateInsertSeedDataHandler();

            // Act
            var result = insertSeedDataHandler.InsertSeedData();

            // Assert
            Assert.IsFalse(result.EntityHasSeedData);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void InsertSeedDataHandler_InsertSeedData_NoExistingEntityData_InsertAll_ReturnsSuccess()
        {
            // Arrange
            IEnumerable<TInterface> insertedSeedData = new List<TInterface>();

            var expectedEntities = new TestProductSeedDataAttribute().Objects.Cast<TInterface>().ToList();
            _MockEntityRepo.Setup(x => x.Get(It.IsAny<IEnumerable<TId>>())).Returns((null) as IQueryable<TInterface>);
            _MockEntityRepo.Setup(x => x.InsertSeedData(It.IsAny<IEnumerable<TInterface>>()))
                .Callback<IEnumerable<TInterface>>((obj) => insertedSeedData = obj)
                .Returns(expectedEntities);
            SetOverwriteUrlParameterValue();

            var insertSeedDataHandler = CreateInsertSeedDataHandler();            

            // Act
            var result = insertSeedDataHandler.InsertSeedData();

            // Assert
            Assert.IsTrue(result.SeedSuccessful);
            Assert.AreEqual(expectedEntities.Count(), insertedSeedData.Count());
            _MockEntityRepo.Verify(x => x.InsertSeedData(It.IsAny<IEnumerable<TInterface>>()), Times.Once); 
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void InsertSeedDataHandler_InsertSeedData_ExistingEntityData_NoNewInsert_ReturnsSuccess()
        {
            // Arrange
            IEnumerable<TInterface> insertedSeedData = new List<TInterface>();

            var existingEntities = new TestProductSeedDataAttribute().Objects.Cast<TEntity>().ToList().AsQueryable();
            _MockEntityRepo.Setup(x => x.Get(It.IsAny<IEnumerable<TId>>())).Returns(existingEntities);
            SetOverwriteUrlParameterValue();

            var insertSeedDataHandler = CreateInsertSeedDataHandler();

            // Act
            var result = insertSeedDataHandler.InsertSeedData();

            // Assert
            Assert.IsTrue(result.SeedSuccessful);
            Assert.IsTrue(insertedSeedData.Count() == 0);
            _MockEntityRepo.Verify(x => x.InsertSeedData(It.IsAny<IEnumerable<TInterface>>()), Times.Never);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void InsertSeedDataHandler_InsertSeedData_InsertThrowsError_ReturnsFailure()
        {
            // Arrange           
            _MockEntityRepo.Setup(x => x.Get(It.IsAny<IEnumerable<TId>>())).Returns((null) as IQueryable<TInterface>);
            _MockEntityRepo.Setup(x => x.InsertSeedData(It.IsAny<IEnumerable<TInterface>>())).Throws(new System.Exception());
            SetOverwriteUrlParameterValue(false);
            var insertSeedDataHandler = CreateInsertSeedDataHandler();

            // Act
            var result = insertSeedDataHandler.InsertSeedData();

            // Assert
            Assert.IsFalse(result.SeedSuccessful);
            _MockEntityRepo.Verify(x => x.InsertSeedData(It.IsAny<IEnumerable<TInterface>>()), Times.Once);
        }

        #endregion

        #region OverwriteSeedData

        [TestMethod]
        public void InsertSeedDataHandler_InsertSeedData_AllowOverwriteFalse_NoUpdate_ReturnsSuccess()
        {
            // Arrange
            IEnumerable<TInterface> updatedSeedData = new List<TInterface>();

            var existingEntities = new TestProductSeedDataAttribute().Objects.Cast<TEntity>().ToList();
            existingEntities[0].Name = "UpdatedProd1";
            var existingEntitiesQueryable = existingEntities.AsQueryable();
            _MockEntityRepo.Setup(x => x.Get(It.IsAny<IEnumerable<TId>>())).Returns(existingEntitiesQueryable);

            var updatedEntities = new List<TInterface>(existingEntities);
            _MockUpdateHandler.Setup(x => x.Update(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<string[]>())).Callback<IEnumerable<TInterface>, string[]>((obj, pms) => updatedSeedData = obj).Returns(updatedEntities.Where(x => x.Id == 1).ToList());

            SetOverwriteUrlParameterValue(false);
            var insertSeedDataHandler = CreateInsertSeedDataHandler();

            // Act
            var result = insertSeedDataHandler.InsertSeedData();

            // Assert
             Assert.IsTrue(result.SeedSuccessful);
            _MockUpdateHandler.Verify(x => x.Update(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<string[]>()), Times.Never);
        }


        [TestMethod]
        public void InsertSeedDataHandler_InsertSeedData_AllowOverwriteTrue_NoRecordsToUpdate_ReturnsSuccess()
        {
            // Arrange
            IEnumerable<TInterface> updatedSeedData = new List<TInterface>();

            var existingEntities = new TestProductSeedDataAttribute().Objects.Cast<TEntity>().ToList();
            var existingEntitiesQueryable = existingEntities.AsQueryable();
            _MockEntityRepo.Setup(x => x.Get(It.IsAny<IEnumerable<TId>>())).Returns(existingEntitiesQueryable);

            var updatedEntities = new List<TInterface>(existingEntities);
            _MockUpdateHandler.Setup(x => x.Update(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<string[]>())).Callback<IEnumerable<TInterface>, string[]>((obj, pms) => updatedSeedData = obj).Returns(updatedEntities);

            SetOverwriteUrlParameterValue(true);
            var insertSeedDataHandler = CreateInsertSeedDataHandler();

            // Act
            var result = insertSeedDataHandler.InsertSeedData();

            // Assert
            Assert.IsTrue(result.SeedSuccessful);
            Assert.IsTrue(updatedSeedData.Count() == 0);
            _MockUpdateHandler.Verify(x => x.Update(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<string[]>()), Times.Never);
        }

        [TestMethod]
        public void InsertSeedDataHandler_InsertSeedData_AllowOverwriteTrue_SomeRecordsToUpdate_ReturnsSuccess()
        {
            // Arrange
            IEnumerable<TInterface> updatedSeedData = new List<TInterface>();

            var existingEntities = new TestProductSeedDataAttribute().Objects.Cast<TEntity>().ToList();
            existingEntities[0].Name = "UpdatedProd1";
            var existingEntitiesQueryable = existingEntities.AsQueryable();
            _MockEntityRepo.Setup(x => x.Get(It.IsAny<IEnumerable<TId>>())).Returns(existingEntitiesQueryable);

            var updatedEntities = new List<TInterface>(existingEntities);
            _MockUpdateHandler.Setup(x => x.Update(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<string[]>()))
                .Callback<IEnumerable<TInterface>, string[]>((obj, pms) => updatedSeedData = obj)
                .Returns(updatedEntities.Where(x => x.Id == 1).ToList());

            SetOverwriteUrlParameterValue(true);
            var insertSeedDataHandler = CreateInsertSeedDataHandler();

            // Act
            var result = insertSeedDataHandler.InsertSeedData();

            // Assert
            Assert.IsTrue(result.SeedSuccessful);
            Assert.IsTrue(updatedSeedData.Count() == 1);
            _MockUpdateHandler.Verify(x => x.Update(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<string[]>()), Times.Once);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void InsertSeedDataHandler_InsertSeedData_AllowOverwriteTrue_ErrorOccured_ReturnsFailure()
        {
            // Arrange
            IEnumerable<TInterface> updatedSeedData = new List<TInterface>();

            var existingEntities = new TestProductSeedDataAttribute().Objects.Cast<TEntity>().ToList();
            existingEntities[0].Name = "UpdatedProd1";
            var existingEntitiesQueryable = existingEntities.AsQueryable();
            _MockEntityRepo.Setup(x => x.Get(It.IsAny<IEnumerable<TId>>())).Returns(existingEntitiesQueryable);

            var updatedEntities = new List<TInterface>(existingEntities);
            _MockUpdateHandler.Setup(x => x.Update(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<string[]>())).Throws(new System.Exception());

            SetOverwriteUrlParameterValue(true);
            var insertSeedDataHandler = CreateInsertSeedDataHandler();

            // Act
            var result = insertSeedDataHandler.InsertSeedData();

            // Assert
            Assert.IsFalse(result.SeedSuccessful);
            _MockUpdateHandler.Verify(x => x.Update(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<string[]>()), Times.Once);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
