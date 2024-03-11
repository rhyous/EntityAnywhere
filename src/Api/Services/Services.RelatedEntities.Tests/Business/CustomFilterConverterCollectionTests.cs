using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Filter;
using TEntity = Rhyous.EntityAnywhere.Interfaces.Common.Tests.EntityInt;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities.Tests.Business
{
    [TestClass]
    public class CustomFilterConverterCollectionTests
    {
        private MockRepository _MockRepository;

        private Mock<IRelatedEntityFilterConverter<TEntity>> _MockRelatedEntityFilterConverter;
        private Mock<IRelatedEntityExtensionFilterConverter<TEntity>> _MockRelatedEntityExtensionFilterConverter;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockRelatedEntityFilterConverter = _MockRepository.Create<IRelatedEntityFilterConverter<TEntity>>();
            _MockRelatedEntityExtensionFilterConverter = _MockRepository.Create<IRelatedEntityExtensionFilterConverter<TEntity>>();
        }

        private CustomFilterConverterCollection<TEntity> CreateCustomFilterConverterCollection()
        {
            return new CustomFilterConverterCollection<TEntity>(
                _MockRelatedEntityFilterConverter.Object,
                _MockRelatedEntityExtensionFilterConverter.Object);
        }

        #region Converters
        [TestMethod]
        public void CustomFilterConverterCollection_()
        {
            // Arrange
            var customFilterConverterCollection = CreateCustomFilterConverterCollection();

            // Act
            var actual = customFilterConverterCollection.Converters;

            // Assert
            Assert.AreEqual(2, actual.Count);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
