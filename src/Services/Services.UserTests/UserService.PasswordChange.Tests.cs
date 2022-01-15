using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.Tests
{
    public partial class UserServicePasswordInitializeTests
    {
        const int Sha256HashLength = 64;
        MockRepository _MockRepository;

        private Mock<IServiceHandlerProviderAltKey<User, IUser, long, string>> _MockServiceHandlerProvider;
        private Mock<IAddAltKeyHandler<User, IUser, long, string>> _MockAddAltKeyHandler;
        private Mock<IGetByIdHandler<User, IUser, long>> _MockGetByIdHandler;
        private Mock<IUpdateAltKeyHandler<User, IUser, long, string>> _MockUpdateAltKeyHandler;
        Mock<IDuplicateUsernameDetector> _MockDuplicateUsernameDetector;
        Mock<IAppSettings> _MockAppSettings;

        IPasswordManager _PasswordManager;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockServiceHandlerProvider = _MockRepository.Create<IServiceHandlerProviderAltKey<User, IUser, long, string>>();
            _MockAddAltKeyHandler = _MockRepository.Create<IAddAltKeyHandler<User, IUser, long, string>>();
            _MockGetByIdHandler = _MockRepository.Create<IGetByIdHandler<User, IUser, long>>();
            _MockUpdateAltKeyHandler = _MockRepository.Create<IUpdateAltKeyHandler<User, IUser, long, string>>();
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

        #region Add User
        [TestMethod]
        public async Task UserService_AddUserWillAutoCreatePasswordTest()
        {
            // Arrange
            var service = CreateService();
            var user = new User { Username = "TestUser" };
            _MockServiceHandlerProvider.Setup(m => m.AddAltKeyHandler).Returns(_MockAddAltKeyHandler.Object);

            _MockDuplicateUsernameDetector.Setup(m => m.Detect(It.IsAny<IEnumerable<string>>(), true))
                                          .Returns((IEnumerable<string>)null);

            _MockAddAltKeyHandler.Setup(m => m.AddAsync(It.Is<IEnumerable<IUser>>(e=>e.First() == user)))
                                 .ReturnsAsync(new List<IUser> { user });

            // Act
            var actual = await service.AddAsync(user);

            // Assert
            Assert.IsNotNull(actual.Password);
            Assert.AreEqual(_PasswordManager.DefaultPasswordLength, actual.Password.Length);
            Assert.IsFalse(actual.IsHashed);
            Assert.IsNull(actual.Salt);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserService_AddUserWithExternalAuthWillNotAutoCreatePasswordTest()
        {
            // Arrange
            var service = CreateService();
            var user = new User { Username = "TestUser", ExternalAuth = true };
            _MockServiceHandlerProvider.Setup(m => m.AddAltKeyHandler).Returns(_MockAddAltKeyHandler.Object);
            _MockAddAltKeyHandler.Setup(m => m.AddAsync(It.Is<IEnumerable<IUser>>(e => e.First() == user)))
                                 .ReturnsAsync(new List<IUser> { user });

            _MockDuplicateUsernameDetector.Setup(m => m.Detect(It.IsAny<IEnumerable<string>>(), true))
                                          .Returns((IEnumerable<string>)null);


            // Act
            var actual = await service.AddAsync(user);

            // Assert
            Assert.IsNull(actual.Password);
            Assert.IsNull(actual.Salt);
            Assert.IsFalse(actual.IsHashed);
            Assert.IsTrue(actual.ExternalAuth);
            Assert.IsNull(actual.Salt);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserService_AddDuplicateUserWillThrowTest()
        {
            // Arrange
            var service = CreateService();
            var user = new User { Username = "TestUser" };
            _MockServiceHandlerProvider.Setup(m => m.AddAltKeyHandler).Returns(_MockAddAltKeyHandler.Object);
            _MockAddAltKeyHandler.Setup(m => m.AddAsync(It.Is<IEnumerable<IUser>>(e => e.First() == user)))
                                 .ReturnsAsync(new List<IUser> { user });

            var duplicateUsername = "DupName";
            var dupUser = new User { Id = 277, Username = duplicateUsername };
            _MockDuplicateUsernameDetector.Setup(m => m.Detect(It.IsAny<IEnumerable<string>>(), true))
                                          .Throws(new DuplicateUsernameException(""));

            // Act & Assert
            await Assert.ThrowsExceptionAsync<DuplicateUsernameException>(async () =>
            {
                await service.AddAsync(user);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserService_AddUserWillAutoCreateHashedSaltedPasswordTest()
        {
            // Arrange
            var service = CreateService();
            var user = new User { Username = "TestUser", IsHashed = true };

            _MockServiceHandlerProvider.Setup(m => m.AddAltKeyHandler).Returns(_MockAddAltKeyHandler.Object);
            _MockAddAltKeyHandler.Setup(m => m.AddAsync(It.Is<IEnumerable<IUser>>(e => e.First() == user)))
                                 .ReturnsAsync(new List<IUser> { user });

            _MockDuplicateUsernameDetector.Setup(m => m.Detect(It.IsAny<IEnumerable<string>>(), true))
                                          .Returns((IEnumerable<string>)null);

            // Act
            var actual = await service.AddAsync(user);

            // Assert
            Assert.IsNotNull(actual.Password);
            Assert.AreEqual(Sha256HashLength, actual.Password.Length);
            Assert.IsTrue(actual.IsHashed);
            Assert.AreEqual(Sha256HashLength, actual.Salt.Length);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserService_AddUserWithIsHashedHashesProvidedPasswordAndAutocreatesSaltTest()
        {
            // Arrange
            var service = CreateService();
            var username = "TestUser";
            var pw = "AbctestPass123!";
            var user = new User { Username = username, IsHashed = true, Password = pw };

            _MockServiceHandlerProvider.Setup(m => m.AddAltKeyHandler).Returns(_MockAddAltKeyHandler.Object);
            _MockAddAltKeyHandler.Setup(m => m.AddAsync(It.Is<IEnumerable<IUser>>(e => e.First() == user)))
                                 .ReturnsAsync(new List<IUser> { user });

            _MockDuplicateUsernameDetector.Setup(m => m.Detect(It.IsAny<IEnumerable<string>>(), true))
                                          .Returns((IEnumerable<string>)null);

            // Act
            var actual = await service.AddAsync(user);

            // Assert
            Assert.IsNotNull(actual.Password);
            Assert.AreEqual(Sha256HashLength, actual.Password.Length);
            Assert.IsTrue(actual.IsHashed);
            Assert.AreEqual(actual.Salt.Length, Sha256HashLength);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserService_AddUserWithIsHashedHashesProvidedPasswordWithProvidedSaltTest()
        {
            // Arrange
            var service = CreateService();
            var username = "TestUser";
            var pw = "AbctestPass123!";
            var salt = "SomeAwesomeSalt";
            var user = new User { Username = username, IsHashed = true, Password = pw, Salt = salt };
            _MockServiceHandlerProvider.Setup(m => m.AddAltKeyHandler).Returns(_MockAddAltKeyHandler.Object);
            _MockAddAltKeyHandler.Setup(m => m.AddAsync(It.Is<IEnumerable<IUser>>(e => e.First() == user)))
                                 .ReturnsAsync(new List<IUser> { user });

            _MockDuplicateUsernameDetector.Setup(m => m.Detect(It.IsAny<IEnumerable<string>>(), true))
                                          .Returns((IEnumerable<string>)null);

            // Act
            var actual = await service.AddAsync(user);

            // Assert
            Assert.IsNotNull(actual.Password);
            Assert.AreEqual(Sha256HashLength, actual.Password.Length);
            Assert.AreEqual(Hash.Get(pw, salt), actual.Password);
            Assert.IsTrue(actual.IsHashed);
            Assert.AreEqual(salt, actual.Salt);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Update User
        [TestMethod]
        public void UserService_UpdateProperty_ChangeToIsHashedWillAutoCreateSaltAndHashPasswordTest()
        {
            // Arrange
            var service = CreateService();
            var username = "TestUser";
            var pw = "AbctestPass123!";
            var existingUser = new User { Id = 121, Username = username, Password = pw };
            IEnumerable<string> actualChangedProperties = null;
            IUser actualUser = null;
            _MockServiceHandlerProvider.Setup(m => m.GetByIdHandler).Returns(_MockGetByIdHandler.Object);
            _MockGetByIdHandler.Setup(m => m.Get(existingUser.Id)).Returns(existingUser);

            _MockServiceHandlerProvider.Setup(m => m.UpdateAltKeyHandler).Returns(_MockUpdateAltKeyHandler.Object);

            _MockUpdateAltKeyHandler.Setup(m => m.Update(existingUser.Id, It.IsAny<PatchedEntity<IUser, long>>()))
                                    .Returns((PatchedEntity<IUser, long> pe, bool stageOnly) =>
                                    {
                                        actualChangedProperties = pe.ChangedProperties;
                                        return actualUser = pe.Entity;
                                    });

            // Act
            var actual = service.UpdateProperty(existingUser.Id, "IsHashed", "true");

            // Assert
            Assert.AreEqual("True", actual);
            Assert.AreEqual(Sha256HashLength, actualUser.Password.Length);
            Assert.IsTrue(actualUser.IsHashed);
            Assert.AreEqual(Sha256HashLength, actualUser.Salt.Length);
            Assert.IsTrue(actualChangedProperties.Contains("Password"));
            Assert.IsTrue(actualChangedProperties.Contains("Salt"));
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void UserService_Update_ChangeToIsHashedWillAutoCreateSaltAndHashPasswordTest()
        {
            // Arrange
            var service = CreateService();
            var username = "TestUser";
            var pw = "AbctestPass123!";
            var existingUser = new User { Id = 121, Username = username, Password = pw };
            IEnumerable<string> actualChangedProperties = null;
            IUser actualUser = null;
            var user = new User { IsHashed = true };
            var patchedEntity = new PatchedEntity<IUser, long>
            {
                Entity = user,
                ChangedProperties = new HashSet<string> { "IsHashed" }
            };

            _MockServiceHandlerProvider.Setup(m => m.GetByIdHandler).Returns(_MockGetByIdHandler.Object);
            _MockGetByIdHandler.Setup(m => m.Get(existingUser.Id)).Returns(existingUser);

            _MockServiceHandlerProvider.Setup(m => m.UpdateAltKeyHandler).Returns(_MockUpdateAltKeyHandler.Object);

            _MockUpdateAltKeyHandler.Setup(m => m.Update(existingUser.Id, patchedEntity))
                                    .Returns((PatchedEntity<IUser, long> pe, bool stageOnly) =>
                                    {
                                        actualChangedProperties = pe.ChangedProperties;
                                        return actualUser = pe.Entity;
                                    });

            // Act
            var actual = service.Update(existingUser.Id, patchedEntity);

            // Assert
            Assert.AreEqual(Sha256HashLength, actualUser.Password.Length);
            Assert.IsTrue(actualUser.IsHashed);
            Assert.AreEqual(Sha256HashLength, actualUser.Salt.Length);
            Assert.IsTrue(actualChangedProperties.Contains("Password"));
            Assert.IsTrue(actualChangedProperties.Contains("Salt"));
            _MockRepository.VerifyAll();

        }

        [TestMethod]
        public void UserService_ChangingAHashedPasswordResultsInAHashOfTheNewPasswordTest()
        {
            // Arrange
            var service = CreateService();
            var username = "TestUser";
            var pw = "AbctestPass123!";
            var newPw = "NewP@ss#27";
            var salt = Hash.Get(username);
            var existingUser = new User { Id = 121, Username = username, IsHashed = true, Password = Hash.Get(pw, salt), Salt = salt };
            var expectedHashedPassword = Hash.Get(newPw, existingUser.Salt);
            var returnUser = new User { Id = 121, Username = username, IsHashed = true, Password = expectedHashedPassword, Salt = salt };
            IUser actualUser = null;
            _MockServiceHandlerProvider.Setup(m => m.GetByIdHandler).Returns(_MockGetByIdHandler.Object);
            _MockGetByIdHandler.Setup(m => m.Get(existingUser.Id)).Returns(existingUser);

            _MockServiceHandlerProvider.Setup(m => m.UpdateAltKeyHandler).Returns(_MockUpdateAltKeyHandler.Object);

            _MockUpdateAltKeyHandler.Setup(m => m.Update(existingUser.Id, It.IsAny<PatchedEntity<IUser, long>>()))
                                    .Returns((PatchedEntity<IUser, long> pe, bool stageOnly) =>
                                    {
                                        return actualUser = pe.Entity;
                                    });

            // Act
            var actual = service.UpdateProperty(existingUser.Id, "Password", newPw);

            // Assert
            Assert.AreEqual(Hash.Get(newPw, existingUser.Salt), actual);
            Assert.AreEqual(Sha256HashLength, actualUser.Password.Length);
            Assert.AreEqual(expectedHashedPassword, actualUser.Password);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void UserService_ClearingAHashedPassword_ThrowsException_Test()
        {
            // Arrange
            var service = CreateService();
            var username = "TestUser";
            var pw = "AbctestPass123!";
            var salt = Hash.Get(username);
            var existingUser = new User { Id = 121, Username = username, IsHashed = true, Password = Hash.Get(pw, salt), Salt = salt };

            _MockServiceHandlerProvider.Setup(m => m.GetByIdHandler).Returns(_MockGetByIdHandler.Object);
            _MockGetByIdHandler.Setup(m => m.Get(existingUser.Id)).Returns(existingUser);

            // Act
            // Assert
            Assert.ThrowsException<InvalidUserDataException>(() =>
            {
                service.UpdateProperty(existingUser.Id, "Password", null);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void UserService_Update_ChangingIsHashedToFalse()
        {
            // Arrange
            var service = CreateService();
            var username = "TestUser";
            var pw = "AbctestPass123!";
            var salt = Hash.Get(username);
            var existingUser = new User { Id = 121, Username = username, IsHashed = true, Password = Hash.Get(pw, salt), Salt = salt };
            IUser actualUser = null;
            var newPw = "Abc@123!";
            var updatedUser = new User { Password = newPw, IsHashed = false };
            var patchedEntity = new PatchedEntity<IUser, long>
            {
                Entity = updatedUser,
                ChangedProperties = new HashSet<string> { "Password", "IsHashed" }
            };
            _MockServiceHandlerProvider.Setup(m => m.GetByIdHandler).Returns(_MockGetByIdHandler.Object);
            _MockGetByIdHandler.Setup(m => m.Get(existingUser.Id)).Returns(existingUser);

            _MockServiceHandlerProvider.Setup(m => m.UpdateAltKeyHandler).Returns(_MockUpdateAltKeyHandler.Object);

            _MockUpdateAltKeyHandler.Setup(m => m.Update(existingUser.Id, patchedEntity))
                                    .Returns((PatchedEntity<IUser, long> pe, bool stageOnly) =>
                                    {
                                        return actualUser = pe.Entity;
                                    });

            // Act
            var actual = service.Update(121, patchedEntity);

            // Assert
            Assert.IsFalse(actual.IsHashed);
            Assert.AreEqual(salt, actual.Salt);
            Assert.AreEqual(newPw, actual.Password);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void UserService_UpdateFailsWithDuplicateUsername()
        {
            // Arrange
            var service = CreateService();
            var username = "TestUser";
            var pw = "AbctestPass123!";
            var salt = Hash.Get(username);
            var duplicateUsername = "DupName";
            var dupUser = new User { Id = 277, Username = duplicateUsername };
            var existingUser = new User { Id = 121, Username = username, IsHashed = true, Password = Hash.Get(pw, salt), Salt = salt };
            
            _MockServiceHandlerProvider.Setup(m => m.GetByIdHandler).Returns(_MockGetByIdHandler.Object);
            _MockGetByIdHandler.Setup(m => m.Get(existingUser.Id)).Returns(existingUser);

            _MockDuplicateUsernameDetector.Setup(m => m.Detect(It.IsAny<IEnumerable<string>>(), It.IsAny<bool>()))
                                          .Throws(new DuplicateUsernameException("test"));

            // Act & Assert
            Assert.ThrowsException<DuplicateUsernameException>(() =>
            {
                service.UpdateProperty(existingUser.Id, "Username", duplicateUsername);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void UserService_UpdateOfFirstname()
        {
            // Arrange
            var service = CreateService();
            var username = "TestUser";
            var pw = "AbctestPass123!";
            var salt = Hash.Get(username);
            var existingUser = new User { Id = 121, Username = username, Firstname = "Jared", IsHashed = true, Password = Hash.Get(pw, salt), Salt = salt };
            var newFirstName = "Abram";
            IUser actualUser = null;
            _MockServiceHandlerProvider.Setup(m => m.GetByIdHandler).Returns(_MockGetByIdHandler.Object);
            _MockGetByIdHandler.Setup(m => m.Get(existingUser.Id)).Returns(existingUser);

            _MockServiceHandlerProvider.Setup(m => m.UpdateAltKeyHandler).Returns(_MockUpdateAltKeyHandler.Object);

            _MockUpdateAltKeyHandler.Setup(m => m.Update(existingUser.Id, It.IsAny<PatchedEntity<IUser, long>>()))
                                    .Returns((PatchedEntity<IUser, long> pe, bool stageOnly) =>
                                    {
                                        return actualUser = pe.Entity;
                                    });

            // Act
            var actual = service.UpdateProperty(existingUser.Id, nameof(User.Firstname), newFirstName);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}