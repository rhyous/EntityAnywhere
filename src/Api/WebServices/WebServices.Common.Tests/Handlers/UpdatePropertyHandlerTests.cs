using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Wrappers;
using System;
using System.Threading.Tasks;

using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.EntityString;
using TId = System.String;
using TInterface = Rhyous.EntityAnywhere.WebServices.Common.Tests.IEntityString;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Handlers
{
    [TestClass]
    public class UpdatePropertyHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IRelatedEntityEnforcer<TEntity>> _MockRelatedEntityEnforcer;
        private Mock<IDistinctPropertiesEnforcer<TEntity, TInterface, TId>> _MockDistinctPropertiesEnforcer;
        private Mock<IEntityEventAll<TEntity, TId>> _MockEntityEventAll;
        private Mock<IInputValidator<TEntity, TId>> _MockInputValidator;
        private Mock<IServiceCommon<TEntity, TInterface, TId>> _MockServiceCommon;
        private Mock<IEntityInfo<TEntity>> _MockEntityInfo;
        private Mock<IHttpStatusCodeSetter> _MockHttpStatusCodeSetter;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockRelatedEntityEnforcer = _MockRepository.Create<IRelatedEntityEnforcer<TEntity>>();
            _MockDistinctPropertiesEnforcer = _MockRepository.Create<IDistinctPropertiesEnforcer<TEntity, TInterface, TId>>();
            _MockEntityEventAll = _MockRepository.Create<IEntityEventAll<TEntity, TId>>();
            _MockInputValidator = _MockRepository.Create<IInputValidator<TEntity, TId>>();
            _MockServiceCommon = _MockRepository.Create<IServiceCommon<TEntity, TInterface, TId>>();
            _MockEntityInfo = _MockRepository.Create<IEntityInfo<TEntity>>();
            _MockHttpStatusCodeSetter = _MockRepository.Create<IHttpStatusCodeSetter>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private UpdatePropertyHandler<TEntity, TInterface, TId> CreateUpdatePropertyHandler()
        {
            return new UpdatePropertyHandler<TEntity, TInterface, TId>(
                _MockRelatedEntityEnforcer.Object,
                _MockDistinctPropertiesEnforcer.Object,
                _MockEntityEventAll.Object,
                _MockInputValidator.Object,
                _MockServiceCommon.Object,
                _MockEntityInfo.Object,
                _MockHttpStatusCodeSetter.Object);
        }

        [TestMethod]
        public async Task UpdatePropertyHandler_Handle_InputValidatorReturnsFalse()
        {
            // Arrange
            var updatePropertyHandler = CreateUpdatePropertyHandler();

            _MockInputValidator.Setup(m => m.CleanAndValidate(It.IsAny<Type>(), ref It.Ref<string>.IsAny, ref It.Ref<string>.IsAny, ref It.Ref<string>.IsAny))
                               .Returns(false);

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await updatePropertyHandler.Handle(null, null, null);
            });
        }
    }
}
