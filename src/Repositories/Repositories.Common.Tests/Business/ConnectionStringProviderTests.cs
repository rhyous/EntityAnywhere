using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;

namespace Rhyous.EntityAnywhere.Repositories.Common.Tests
{
    [TestClass]
    public class ConnectionStringProviderTests
    {
        private readonly string _DefaultConnectionString = EntityConnectionStringNameProvider<User>.DefaultSqlConnectionStringName;

        [TestMethod]
        public void ConnectionStringProvider_DefaultConnectionStringExists_Test()
        {
            // Arrange
            var provider = new EntityConnectionStringNameProvider<User>
            {
                Connections = new ConnectionStringSettingsCollection
                {
                    new ConnectionStringSettings("AuditSqlRepository", ""),
                    new ConnectionStringSettings(_DefaultConnectionString, "")
                }
            };

            // Act
            var actual = provider.Provide();

            // Assert
            Assert.AreEqual(_DefaultConnectionString, actual);
        }

        [TestMethod]
        public void ConnectionStringProvider_EntityConnectionStringExists_Test()
        {
            // Arrange
            var expectedCustomName = $"{nameof(User)}{_DefaultConnectionString}";
            var provider = new EntityConnectionStringNameProvider<User>
            {
                Connections = new ConnectionStringSettingsCollection
                {
                    new ConnectionStringSettings(expectedCustomName, ""),
                    new ConnectionStringSettings("AuditSqlRepository", ""),
                    new ConnectionStringSettings(_DefaultConnectionString, "")
                }
            };

            // Act
            var actual = provider.Provide();

            // Assert
            Assert.AreEqual(expectedCustomName, actual);
        }

        [TestMethod]
        public void ConnectionStringProvider_OnlyCommonConnectionStringExists_Test()
        {
            // Arrange
            var provider = new EntityConnectionStringNameProvider<User>
            {
                Connections = new ConnectionStringSettingsCollection
                {
                    new ConnectionStringSettings(_DefaultConnectionString, "TheCommonConnectionString")
                }
            };

            // Act
            var actual = provider.Provide();

            // Assert
            Assert.AreEqual(_DefaultConnectionString, actual);
        }

        [TestMethod]
        public void ConnectionStringProvider_NoConnectionStringExists_Test()
        {
            // Arrange
            var provider = new EntityConnectionStringNameProvider<User>
            {
                Connections = new ConnectionStringSettingsCollection { }
            };

            // Act
            // Assert
            Assert.ThrowsException<InvalidOperationException>(() => { provider.Provide(); });
        }

        [TestMethod]
        public void ConnectionStringProvider_EntityGroupConnectionStringExists_Test()
        {
            // Arrange
            var expectedCustomName = "AuditSqlRepository";
            var provider = new EntityConnectionStringNameProvider<Log>
            {
                Connections = new ConnectionStringSettingsCollection
                {
                    new ConnectionStringSettings(expectedCustomName, ""),
                    new ConnectionStringSettings("UserSqlRepository", ""),
                    new ConnectionStringSettings(_DefaultConnectionString, "")
                }
            };

            // Act
            var actual = provider.Provide();

            // Assert
            Assert.AreEqual(expectedCustomName, actual);
        }
    }
}