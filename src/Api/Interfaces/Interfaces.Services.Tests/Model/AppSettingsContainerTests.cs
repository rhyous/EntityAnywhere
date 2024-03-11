using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Rhyous.UnitTesting;
using Rhyous.Wrappers;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Interfaces.Tests
{
    [TestClass]
    public class AppSettingsContainerTests
    {
        private MockRepository _MockRepository;

        private Mock<IFileIO> _MockFile;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockFile = _MockRepository.Create<IFileIO>();
        }

        [TestMethod]
        public void AppSettingsContainer_Constructor_Null_NameValueCollection_Works()
        {
            // Arrange
            NameValueCollection appSettings = null;

            // Act
            var result = new AppSettingsContainer(appSettings, _MockFile.Object);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Collection);
        }

        [TestMethod]
        public void AppSettingsContainer_Constructor_Valid_NameValueCollection_Json_NoJsonPath_Works()
        {
            // Arrange
            var path = @"c:\some\path\to\a\settings\file.json";
            NameValueCollection appSettings = new NameValueCollection();
            _MockFile.Setup(m => m.Exists(path)).Returns(false);

            // Act
            var result = new AppSettingsContainer(appSettings, _MockFile.Object);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(appSettings, result.Collection);
        }

        [TestMethod]
        public void AppSettingsContainer_Constructor_Valid_NameValueCollection_Json_PathProvided_Works()
        {
            // Arrange
            var path = @"c:\some\path\to\a\settings\file.json";
            NameValueCollection appSettings = new NameValueCollection { { AppSettingsContainer.ApplicationSettingsPath, path } };
            var json = "{\"prop1\":\"val1\",\"prop2\":\"val2\"}";
            _MockFile.Setup(m => m.Exists(path)).Returns(true);
            _MockFile.Setup(m => m.ReadAllText(path)).Returns(json);

            // Act
            var result = new AppSettingsContainer(appSettings, _MockFile.Object);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(appSettings, result.Collection);
            Assert.AreEqual(3, appSettings.Count);
        }

        [TestMethod]
        public void AppSettingsContainer_Constructor_Valid_NameValueCollection_Json_PathProvided_SettingWrittenTwice_ConfigTakesPrecedence()
        {
            // Arrange
            var path = @"c:\some\path\to\a\settings\file.json";
            NameValueCollection appSettings = new NameValueCollection
            {
                { AppSettingsContainer.ApplicationSettingsPath, path },
                { "prop1", "firstValue1"}
            }; 
            var json = "{\"prop1\":\"secondValue1\",\"prop2\":\"val2\"}";
            _MockFile.Setup(m => m.Exists(path)).Returns(true);
            _MockFile.Setup(m => m.ReadAllText(path)).Returns(json);

            // Act
            var result = new AppSettingsContainer(appSettings, _MockFile.Object);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(appSettings, result.Collection);
            Assert.AreEqual(3, result.Collection.Count);
            Assert.AreEqual("firstValue1", result.Collection["prop1"]);
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void AppSettingsContainer_Constructor_Valid_NameValueCollection_Json_PathProvided_NullEmptyWhitespace_DoesNotTakePrecedence(string val)
        {
            // Arrange
            var path = @"c:\some\path\to\a\settings\file.json";
            NameValueCollection appSettings = new NameValueCollection
            {
                { AppSettingsContainer.ApplicationSettingsPath, path },
                { "prop1", val}
            };
            var json = JsonConvert.SerializeObject(new { prop1 = "secondValue", prop2 = "val2" });
            _MockFile.Setup(m => m.Exists(path)).Returns(true);
            _MockFile.Setup(m => m.ReadAllText(path)).Returns(json);

            // Act
            var result = new AppSettingsContainer(appSettings, _MockFile.Object);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(appSettings, result.Collection);
            Assert.AreEqual(3, result.Collection.Count);
            Assert.AreEqual("secondValue", result.Collection["prop1"]);
        }
    }
}