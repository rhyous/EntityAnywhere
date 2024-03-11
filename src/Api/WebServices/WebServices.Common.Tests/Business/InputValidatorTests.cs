using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Linq;
using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.EntityBasic;
using TId = System.Int32;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Business
{
    [TestClass]
    public class InputValidatorTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityInfo<TEntity>> _MockEntityInfo;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);
            _MockEntityInfo = _MockRepository.Create<IEntityInfo<TEntity>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private IInputValidator<TEntity, TId> CreateInputValidator()
        {
            return new InputValidator<TEntity, TId>(_MockEntityInfo.Object);
        }

        #region CleanAndValidate for UpdatePropertyHandler
        [TestMethod]
        public void InputValidator_CleanAndValidate_Update_EntityType_Null_ReturnsFalse()
        {
            // Arrange
            var inputValidator = CreateInputValidator();
            Type type = null;
            string id = "27";
            string property = "Name";
            string value = "somevalue";

            // Act
            bool result = inputValidator.CleanAndValidate(type, ref id, ref property, ref value);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void InputValidator_CleanAndValidate_Update_Id_NullEmptyOrWhitespace_ReturnsFalse(string id)
        {
            // Arrange
            var inputValidator = CreateInputValidator();
            Type type = null;
            string property = "Name";
            string value = "somevalue";

            // Act
            bool result = inputValidator.CleanAndValidate(type, ref id, ref property, ref value);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void InputValidator_CleanAndValidate_Update_Property_NullEmptyOrWhitespace_ReturnsFalse(string property)
        {
            // Arrange
            var inputValidator = CreateInputValidator();
            Type type = null;
            string id = "27";
            string value = "somevalue";

            // Act
            bool result = inputValidator.CleanAndValidate(type, ref id, ref property, ref value);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void InputValidator_CleanAndValidate_Value_Null() // We should be able to update a value to null
        {
            // Arrange
            var inputValidator = CreateInputValidator();
            string id = "27";
            Type entityType = typeof(EntityBasic);
            string property = "Name";
            string value = null;
            var props = typeof(TEntity).GetProperties()
                                       .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
            _MockEntityInfo.Setup(m => m.Properties).Returns(props);

            // Act
            bool result = inputValidator.CleanAndValidate(entityType, ref id,
                                                          ref property, ref value);

            // Assert
            Assert.IsNull(value);
        }

        [TestMethod]
        public void UpdateProperty_TrimmableAndNotIgnoredProperty_TextIsTrimmed()
        {
            // Arrange
            var inputValidator = CreateInputValidator();
            string id = "27";
            Type entityType = typeof(EntityBasic);
            string property = "Name";
            string value = "spacetrimtest      ";
            var props = typeof(TEntity).GetProperties()
                                       .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
            _MockEntityInfo.Setup(m => m.Properties).Returns(props);

            // Act
            bool result = inputValidator.CleanAndValidate(entityType, ref id,
                                                          ref property, ref value);

            // Assert
            Assert.AreEqual("spacetrimtest", value);
        }
        #endregion

        #region CleanAndValidate for Patch
        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void InputValidator_CleanAndValidate_Patch_Id_NullEmptyOrWhitespace_ReturnsFalse(string id)
        {
            // Arrange
            var inputValidator = CreateInputValidator();
            PatchedEntity<EntityBasic, int> patchedEntity = null;

            // Act
            bool result = inputValidator.CleanAndValidate(ref id, patchedEntity);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void InputValidator_CleanAndValidate_Patch_PatchedEntity_Null_ReturnsFalse()
        {
            // Arrange
            var inputValidator = CreateInputValidator();
            string id = "27";
            PatchedEntity<EntityBasic, int> patchedEntity = null;

            // Act
            bool result = inputValidator.CleanAndValidate(ref id, patchedEntity);

            // Assert
            Assert.IsFalse(result);
        }
        #endregion

        #region CleanAndValidate for PUT
        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void InputValidator_CleanAndValidate_Put_Id_NullEmptyOrWhitespace_ReturnsFalse(string id)
        {
            // Arrange
            var inputValidator = CreateInputValidator();
            EntityBasic entity = null;

            // Act
            bool result = inputValidator.CleanAndValidate(ref id, entity);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void InputValidator_CleanAndValidate_Put_Entity_Null_ReturnsFalse()
        {
            // Arrange
            var inputValidator = CreateInputValidator();
            string id = "27";
            EntityBasic entity = null;

            // Act
            bool result = inputValidator.CleanAndValidate(ref id, entity);

            // Assert
            Assert.IsFalse(result);
        }
        #endregion
    }
}
