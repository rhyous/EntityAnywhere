using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces.Common.Tests;
using Rhyous.EntityAnywhere.Tools;
using System;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Tools
{
    [TestClass]
    public class EntityInfoResolverTests
    {
        private MockRepository _MockRepository;

        private Mock<IDependencyInjectionResolver> _MockDIResolver;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockDIResolver = _MockRepository.Create<IDependencyInjectionResolver>();
        }

        private TypeInfoResolver CreateEntityInfoResolver()
        {
            return new TypeInfoResolver(_MockDIResolver.Object);
        }

        #region Resolve
        [TestMethod]
        public void EntityInfoResolver_Resolve_ForwardsToGeneric_Resolves_Test()
        {
            // Arrange
            var entityInfoResolver = CreateEntityInfoResolver();
            Type type = typeof(EntityInt);
            var entityInfo = new EntityInfo<EntityInt>();
            _MockDIResolver.Setup(m => m.Resolve<ITypeInfo<EntityInt>>()).Returns(entityInfo);

            // Act
            var result = entityInfoResolver.Resolve(type);

            // Assert
            Assert.IsNotNull(result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
