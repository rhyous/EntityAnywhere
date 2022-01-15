using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.Tests
{
    [TestClass]
    public class PluginCredentialsValidatorTests
    {
        private MockRepository _MockRepository;

        private Mock<IAppDomain> _MockAppDomain;
        private Mock<IPluginLoaderSettings> _MockPluginLoaderSettings;
        private Mock<IPluginLoaderFactory<ICredentialsValidatorAsync>> _MockPluginLoaderFactory;
        private Mock<IPluginObjectCreator<ICredentialsValidatorAsync>> _MockPluginObjectCreator;
        private Mock<IPluginPaths> _MockPluginPaths;
        private Mock<IPluginLoaderLogger> _MockPluginLoaderLogger;
        private Mock<ILogger> _MockLogger;
        private Mock<ICredentialsValidatorAsync> _MockCredentialsValidator1;
        private Mock<ICredentialsValidatorAsync> _MockCredentialsValidator2;
        private PluginCollection<ICredentialsValidatorAsync> _PluginCollection;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAppDomain = _MockRepository.Create<IAppDomain>();
            _MockPluginLoaderSettings = _MockRepository.Create<IPluginLoaderSettings>();
            _MockPluginLoaderFactory = _MockRepository.Create<IPluginLoaderFactory<ICredentialsValidatorAsync>>();
            _MockPluginObjectCreator = _MockRepository.Create<IPluginObjectCreator<ICredentialsValidatorAsync>>();
            _MockPluginPaths = _MockRepository.Create<IPluginPaths>();
            _MockPluginLoaderLogger = _MockRepository.Create<IPluginLoaderLogger>();
            _MockLogger = _MockRepository.Create<ILogger>();
            _MockCredentialsValidator1 = _MockRepository.Create<ICredentialsValidatorAsync>();
            _MockCredentialsValidator2 = _MockRepository.Create<ICredentialsValidatorAsync>();
            _PluginCollection = new PluginCollection<ICredentialsValidatorAsync>();

        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private PluginCredentialsValidator CreatePluginCredentialsValidator()
        {
            var service = new PluginCredentialsValidator(
                _MockAppDomain.Object,
                _MockPluginLoaderSettings.Object,
                _MockPluginLoaderFactory.Object,
                _MockPluginObjectCreator.Object,
                _MockPluginPaths.Object,
                _MockPluginLoaderLogger.Object,
                _MockLogger.Object);
            typeof(PluginCredentialsValidator).BaseType.GetFieldInfo("_PluginCollection").SetValue(service, _PluginCollection);
            return service;
        }

        [TestMethod]
        public async Task IsValidAsync_GivenValidArguments_ReturnsTokenResult()
        {
            //Arrange
            _MockPluginPaths.Setup(m => m.Paths).Returns(new List<string> { @"c:\path1", @"c:\path2" });
            var reponse = new CredentialsValidatorResponse
            {
                AuthenticationPlugin = "Plugin1",
                Token = Data.Token,
                Success = true,
                Message = "Success"
            };
            _MockCredentialsValidator1.Setup(x => x.IsValidAsync(It.IsAny<ICredentials>()))
                                     .ReturnsAsync(reponse);

            var mockPlugin = _MockRepository.Create<IPlugin<ICredentialsValidatorAsync>>();
            var list = new List<ICredentialsValidatorAsync> { _MockCredentialsValidator1.Object };
            mockPlugin.Setup(m => m.CreatePluginObjects(_MockPluginObjectCreator.Object))
                      .Returns(list);
            _PluginCollection.Add(mockPlugin.Object);

            var pluginCredentialsValidator = CreatePluginCredentialsValidator();

            //Act
            var result = await pluginCredentialsValidator.IsValidAsync(Data.Credentials);

            //Assert
            Assert.AreEqual(Data.Token, result.Token);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task IsValidAsync_GivenValidArguments_TwoPlugins_FirstFails_SecondWorks_ReturnsTokenResult()
        {
            //Arrange
            _MockPluginPaths.Setup(m => m.Paths).Returns(new List<string> { @"c:\path1", @"c:\path2" });
            var failedresponse = new CredentialsValidatorResponse
            {
                AuthenticationPlugin = "Plugin1",
                Success = false,
                Message = "Some failed message."
            };
            _MockCredentialsValidator1.Setup(x => x.IsValidAsync(It.IsAny<ICredentials>()))
                                     .ReturnsAsync(failedresponse);
            var reponse = new CredentialsValidatorResponse
            {
                AuthenticationPlugin = "Plugin2",
                Token = Data.Token,
                Success = true,
                Message = "Success"
            };
            _MockCredentialsValidator2.Setup(x => x.IsValidAsync(It.IsAny<ICredentials>()))
                                     .ReturnsAsync(reponse);

            var mockPlugin = _MockRepository.Create<IPlugin<ICredentialsValidatorAsync>>();
            var list = new List<ICredentialsValidatorAsync> { _MockCredentialsValidator1.Object, _MockCredentialsValidator2.Object };
            mockPlugin.Setup(m => m.CreatePluginObjects(_MockPluginObjectCreator.Object))
                      .Returns(list);            
            _PluginCollection.Add(mockPlugin.Object);

            var pluginCredentialsValidator = CreatePluginCredentialsValidator();

            //Act
            var result = await pluginCredentialsValidator.IsValidAsync(Data.Credentials);

            //Assert
            Assert.AreEqual(Data.Token, result.Token);
            Assert.IsTrue(result.Success);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task IsValidAsync_GivenInvalidArguments_ReturnsTokenNull()
        {
            //Arrange
            _MockPluginPaths.Setup(m => m.Paths).Returns(new List<string> { @"c:\path1", @"c:\path2" });
            var pluginCredentialsValidator = CreatePluginCredentialsValidator();

            //Act
            var result = await pluginCredentialsValidator.IsValidAsync(Data.Credentials);

            //Assert
            Assert.AreEqual(result.Token, null);
            Assert.IsFalse(result.Success);
            _MockRepository.VerifyAll();
        }
    }
}
