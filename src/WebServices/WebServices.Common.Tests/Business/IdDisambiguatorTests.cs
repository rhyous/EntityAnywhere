using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Clients2;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Business
{
    [TestClass]
    public class IdDisambiguatorTests
    {
        #region HandleIdKeyword tests
        [TestMethod]
        public void IdDisambiguator_HandleIdKeyword_IdParts_Null_Test()
        {
            // Arrange
            string[] idParts = null;
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();
            var disambiguatedId = new DisambiguatedId<int>();

            // Act
            // Assert
            Assert.ThrowsException<InvalidEntityIdException>(() => { idDisambiguator.HandleIdKeyword(idParts, disambiguatedId ); });
        }

        [TestMethod]
        public void IdDisambiguator_HandleIdKeyword_IdParts_Empty_Test()
        {
            // Arrange
            string[] idParts = new string[0];
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();
            var disambiguatedId = new DisambiguatedId<int>();

            // Act
            // Assert
            Assert.ThrowsException<InvalidEntityIdException>(() => { idDisambiguator.HandleIdKeyword(idParts, disambiguatedId); });
        }

        [TestMethod]
        public void IdDisambiguator_HandleIdKeyword_IdParts_One_Test()
        {
            // Arrange
            string[] idParts = new string[] { IdDisambiguator.Id };
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();
            var disambiguatedId = new DisambiguatedId<int>();

            // Act
            // Assert
            Assert.ThrowsException<InvalidEntityIdException>(() => { idDisambiguator.HandleIdKeyword(idParts, disambiguatedId); });
        }

        [TestMethod]
        public void IdDisambiguator_HandleIdKeyword_IdParts_FirstStringEmpty_Test()
        {
            // Arrange
            string[] idParts = new string[] { "", "1"};
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();
            var disambiguatedId = new DisambiguatedId<int>();

            // Act
            // Assert
            Assert.ThrowsException<InvalidEntityIdException>(() => { idDisambiguator.HandleIdKeyword(idParts, disambiguatedId); });
        }

        [TestMethod]
        public void IdDisambiguator_HandleIdKeyword_IdParts_SecondStringEmpty_Test()
        {
            // Arrange
            string[] idParts = new string[] { IdDisambiguator.Id, "" };
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();
            var disambiguatedId = new DisambiguatedId<int>();

            // Act
            // Assert
            Assert.ThrowsException<InvalidEntityIdException>(() => { idDisambiguator.HandleIdKeyword(idParts, disambiguatedId); });
        }

        [TestMethod]
        public void IdDisambiguator_HandleIdKeyword_IdParts_Valid_Test()
        {
            // Arrange
            string[] idParts = new string[] { IdDisambiguator.Id, "1" };
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();
            var disambiguatedId = new DisambiguatedId<int>();

            // Act
            var actual = idDisambiguator.HandleIdKeyword(idParts, disambiguatedId);

            // Assert
            Assert.AreEqual(IdType.Id, actual.IdType);
            Assert.AreEqual("1", actual.AltId);
            Assert.AreEqual(1, actual.Id);
            Assert.IsNull(actual.AlternateIdProperty);
        }

        [TestMethod]
        public void IdDisambiguator_HandleIdKeyword_IdParts_Valid_AlternateKey_Test()
        {
            // Arrange
            string[] idParts = new string[] { IdDisambiguator.Id, "abc" };
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();
            var disambiguatedId = new DisambiguatedId<int>();

            // Act
            var actual = idDisambiguator.HandleIdKeyword(idParts, disambiguatedId);

            // Assert
            Assert.AreEqual(IdType.Alt, actual.IdType);
            Assert.AreEqual("abc", actual.AltId);
            Assert.AreEqual(0, actual.Id);
            Assert.AreEqual(IdDisambiguator.Key, actual.AlternateIdProperty);
        }
        #endregion

        #region HandleAltKeyword tests
        [TestMethod]
        public void IdDisambiguator_HandleAltKeyword_IdParts_Null_Test()
        {
            // Arrange
            string[] idParts = null;
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();
            var disambiguatedId = new DisambiguatedId<int>();

            // Act
            // Assert
            Assert.ThrowsException<InvalidEntityIdException>(() => { idDisambiguator.HandleAltKeyword(idParts, disambiguatedId); });
        }

        [TestMethod]
        public void IdDisambiguator_HandleAltKeyword_IdParts_Empty_Test()
        {
            // Arrange
            string[] idParts = new string[0];
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();
            var disambiguatedId = new DisambiguatedId<int>();

            // Act
            // Assert
            Assert.ThrowsException<InvalidEntityIdException>(() => { idDisambiguator.HandleAltKeyword(idParts, disambiguatedId); });
        }

        [TestMethod]
        public void IdDisambiguator_HandleAltKeyword_IdParts_One_Test()
        {
            // Arrange
            string[] idParts = new string[] { IdDisambiguator.Id };
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();
            var disambiguatedId = new DisambiguatedId<int>();

            // Act
            // Assert
            Assert.ThrowsException<InvalidEntityIdException>(() => { idDisambiguator.HandleAltKeyword(idParts, disambiguatedId); });
        }

        [TestMethod]
        public void IdDisambiguator_HandleAltKeyword_IdParts_FirstStringEmpty_Test()
        {
            // Arrange
            string[] idParts = new string[] { "", IdDisambiguator.Key, "1" };
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();
            var disambiguatedId = new DisambiguatedId<int>();

            // Act
            // Assert
            Assert.ThrowsException<InvalidEntityIdException>(() => { idDisambiguator.HandleAltKeyword(idParts, disambiguatedId); });
        }
        
        [TestMethod]
        public void IdDisambiguator_HandleAltKeyword_IdParts_SecondStringEmpty_Test()
        {
            // Arrange
            string[] idParts = new string[] { IdDisambiguator.Alt, "", "1" };
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();
            var disambiguatedId = new DisambiguatedId<int>();

            // Act
            // Assert
            Assert.ThrowsException<InvalidEntityIdException>(() => { idDisambiguator.HandleAltKeyword(idParts, disambiguatedId); });
        }

        [TestMethod]
        public void IdDisambiguator_HandleAltKeyword_IdParts_ThirdStringEmpty_Test()
        {
            // Arrange
            string[] idParts = new string[] { IdDisambiguator.Alt, IdDisambiguator.Key, "" };
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();
            var disambiguatedId = new DisambiguatedId<int>();

            // Act
            // Assert
            Assert.ThrowsException<InvalidEntityIdException>(() => { idDisambiguator.HandleAltKeyword(idParts, disambiguatedId); });
        }

        [TestMethod]
        public void IdDisambiguator_HandleAltKeyword_IdParts_Valid_Test()
        {
            // Arrange
            string[] idParts = new string[] { IdDisambiguator.Alt, IdDisambiguator.Key, "1" };
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();
            var disambiguatedId = new DisambiguatedId<int>();

            // Act
            var actual = idDisambiguator.HandleAltKeyword(idParts, disambiguatedId);

            // Assert
            Assert.AreEqual("1", actual.AltId);
            Assert.AreEqual(0, actual.Id);
            Assert.AreEqual(IdDisambiguator.Key, actual.AlternateIdProperty);
        }
        #endregion

        #region Disambiguate $Id tests
        [TestMethod]
        public void IdDisambiguator_Default_Test()
        {
            // Arrange
            var id = "1";
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();

            // Act
            var actual = idDisambiguator.Disambiguate(id);

            // Assert
            Assert.AreEqual(IdType.Id, actual.IdType);
            Assert.AreEqual(id, actual.OrginalId);
            Assert.AreEqual("1", actual.AltId);
            Assert.AreEqual(1, actual.Id);
            Assert.IsNull(actual.AlternateIdProperty);
        }

        [TestMethod]
        public void IdDisambiguator_ReservedKeyWord_Id_Test()
        {
            // Arrange
            var id = $"{IdDisambiguator.Id}{IdDisambiguator.Separator}1";
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();

            // Act
            var actual = idDisambiguator.Disambiguate(id);

            // Assert
            Assert.AreEqual(IdType.Id, actual.IdType);
            Assert.AreEqual(id, actual.OrginalId);
            Assert.AreEqual("1", actual.AltId);
            Assert.AreEqual(1, actual.Id);
            Assert.IsNull(actual.AlternateIdProperty);
        }

        [TestMethod]
        public void IdDisambiguator_ReservedKeyWord_IdWithPrefixButNoSeparator_Works_Test()
        {
            // Arrange
            var id = $"{IdDisambiguator.Id}1";
            var idDisambiguator = new IdDisambiguator<EntityString, string>();

            // Act
            var actual = idDisambiguator.Disambiguate(id);

            // Assert
            Assert.AreEqual(IdType.Id, actual.IdType);
            Assert.AreEqual(id, actual.OrginalId);
            Assert.AreEqual(id, actual.AltId);
            Assert.AreEqual(id, actual.Id);
            Assert.IsNull(actual.AlternateIdProperty);
        }

        [TestMethod]
        public void IdDisambiguator_ReservedKeyWord_IdWithSeparator_Works_Test()
        {
            // Arrange
            var id = $"10.2.1.27";
            var idDisambiguator = new IdDisambiguator<EntityString, string>();

            // Act
            var actual = idDisambiguator.Disambiguate(id);

            // Assert
            Assert.AreEqual(IdType.Id, actual.IdType);
            Assert.AreEqual(id, actual.OrginalId);
            Assert.AreEqual(id, actual.AltId);
            Assert.AreEqual(id, actual.Id);
            Assert.IsNull(actual.AlternateIdProperty);
        }

        [TestMethod]
        public void IdDisambiguator_ReservedKeyWord_Id_AlternateKey_Test()
        {
            // Arrange
            var id = $"{IdDisambiguator.Id}.abc";
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();

            // Act
            var actual = idDisambiguator.Disambiguate(id);

            // Assert
            Assert.AreEqual(IdType.Alt, actual.IdType);
            Assert.AreEqual(id, actual.OrginalId);
            Assert.AreEqual("abc", actual.AltId);
            Assert.AreEqual(0, actual.Id);
            Assert.AreEqual(IdDisambiguator.Key, actual.AlternateIdProperty);
        }

        [TestMethod]
        public void IdDisambiguator_ReservedKeyWord_Id_Invalid_Test()
        {
            // Arrange
            var id = $"{IdDisambiguator.Id}{IdDisambiguator.Separator}System1{IdDisambiguator.Separator}12701";
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();

            // Act
            // Assert
            Assert.ThrowsException<InvalidEntityIdException>(() => { idDisambiguator.Disambiguate(id); });
        }
        #endregion

        #region Disambiguate $Alt tests
        [TestMethod]
        public void IdDisambiguator_ReservedKeyWord_Alt_Test()
        {
            // Arrange
            var id = $"{IdDisambiguator.AltKey}1";

            var idDisambiguator = new IdDisambiguator<EntityInt, int>();

            // Act
            var actual = idDisambiguator.Disambiguate(id);

            // Assert
            Assert.AreEqual(IdType.Alt, actual.IdType);
            Assert.AreEqual(id, actual.OrginalId);
            Assert.AreEqual("1", actual.AltId);
            Assert.AreEqual(0, actual.Id);
            Assert.AreEqual(IdDisambiguator.Key, actual.AlternateIdProperty);
        }

        [TestMethod]
        public void IdDisambiguator_ReservedKeyWord_Alt_IdHasSeparator_Test()
        {
            // Arrange
            var prop = "IpAddress";
            var id = $"{IdDisambiguator.Alt}{IdDisambiguator.Separator}{prop}{IdDisambiguator.Separator}10.1.1.27";
            var expectedId = "10.1.1.27";

            var idDisambiguator = new IdDisambiguator<EntityInt, int>();

            // Act
            var actual = idDisambiguator.Disambiguate(id);

            // Assert
            Assert.AreEqual(IdType.Alt, actual.IdType);
            Assert.AreEqual(id, actual.OrginalId);
            Assert.AreEqual(expectedId, actual.AltId);
            Assert.AreEqual(0, actual.Id);
            Assert.AreEqual(prop, actual.AlternateIdProperty);
        }

        [TestMethod]
        public void IdDisambiguator_ReservedKeyWord_AltKey_IdHasSeparator_Test()
        {
            // Arrange
            var id = $"{IdDisambiguator.AltKey}10.1.1.27";
            var expectedId = "10.1.1.27";

            var idDisambiguator = new IdDisambiguator<EntityInt, int>();

            // Act
            var actual = idDisambiguator.Disambiguate(id);

            // Assert
            Assert.AreEqual(IdType.Alt, actual.IdType);
            Assert.AreEqual(id, actual.OrginalId);
            Assert.AreEqual(expectedId, actual.AltId);
            Assert.AreEqual(0, actual.Id);
            Assert.AreEqual(IdDisambiguator.Key, actual.AlternateIdProperty);
        }

        [TestMethod]
        public void IdDisambiguator_ReservedKeyWord_Alt_Invalid_Test()
        {
            // Arrange
            var id = $"{IdDisambiguator.Alt}{IdDisambiguator.Separator}0000123456";
            var idDisambiguator = new IdDisambiguator<EntityInt, int>();

            // Act
            // Assert
            Assert.ThrowsException<InvalidEntityIdException>(() => { idDisambiguator.Disambiguate(id); });
        }
        #endregion
    }
}