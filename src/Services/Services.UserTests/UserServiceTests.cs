using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Tests
{
    [TestClass()]
    public partial class UserServiceTests
    {
        MockRepository _MockRepository;

        private Mock<IServiceHandlerProviderAltKey<User, IUser, long, string>> _MockServiceHandlerProvider;
        Mock<IDuplicateUsernameDetector> _MockDuplicateUsernameDetector;
        Mock<IAppSettings> _MockAppSettings;

        IPasswordManager _PasswordManager;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockServiceHandlerProvider = _MockRepository.Create<IServiceHandlerProviderAltKey<User, IUser, long, string>>();
            _MockDuplicateUsernameDetector = _MockRepository.Create<IDuplicateUsernameDetector>();
            _MockAppSettings = _MockRepository.Create<IAppSettings>();

            _PasswordManager = new PasswordManager(_MockAppSettings.Object);
        }

        public UserService CreateService()
        {
            return new UserService(_MockServiceHandlerProvider.Object,
                                   _MockDuplicateUsernameDetector.Object,
                                   _PasswordManager);
        }

    }
}