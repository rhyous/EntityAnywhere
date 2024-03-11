using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;

namespace Rhyous.EntityAnywhere.Repositories.Common.Tests
{
    [TestClass]
    public class ConnectionStringProviderTests
    {
        private readonly string _DefaultConnectionString = EntityConnectionStringProvider<User>.DefaultSqlConnectionStringName;

        [TestMethod]
        public void ConnectionStringProvider_DefaultConnectionStringExists_Test()
        {
            // Arrange
            var connectionString1 = "some connections string 1";
            var connectionString_default = "some connections string default";
            var provider = new EntityConnectionStringProvider<User>
            {
                Connections = new ConnectionStringSettingsCollection
                {
                    new ConnectionStringSettings("AuditSqlRepository", connectionString1),
                    new ConnectionStringSettings(_DefaultConnectionString, connectionString_default)
                }
            };

            // Act
            var actual = provider.Provide();

            // Assert
            Assert.AreEqual(connectionString_default, actual);
        }

        [TestMethod]
        public void ConnectionStringProvider_EntityConnectionStringExists_Test()
        {
            // Arrange
            var connectionString1 = "some connections string 1";
            var connectionString2 = "some connections string 2";
            var connectionString_default = "some connections string default";
            var expectedCustomName = $"{nameof(User)}{_DefaultConnectionString}";
            var provider = new EntityConnectionStringProvider<User>
            {
                Connections = new ConnectionStringSettingsCollection
                {
                    new ConnectionStringSettings(expectedCustomName, connectionString1),
                    new ConnectionStringSettings("AuditSqlRepository", connectionString2),
                    new ConnectionStringSettings(_DefaultConnectionString, connectionString_default)
                }
            };

            // Act
            var actual = provider.Provide();

            // Assert
            Assert.AreEqual(connectionString1, actual);
        }

        [TestMethod]
        public void ConnectionStringProvider_OnlyCommonConnectionStringExists_Test()
        {
            // Arrange
            var connectionString1 = "some connections string 1";
            var provider = new EntityConnectionStringProvider<User>
            {
                Connections = new ConnectionStringSettingsCollection
                {
                    new ConnectionStringSettings(_DefaultConnectionString, connectionString1)
                }
            };

            // Act
            var actual = provider.Provide();

            // Assert
            Assert.AreEqual(connectionString1, actual);
        }

        [TestMethod]
        public void ConnectionStringProvider_NoConnectionStringExists_Test()
        {
            // Arrange
            var provider = new EntityConnectionStringProvider<User>
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
            var connectionString1 = "some connections string 1";
            var connectionString2 = "some connections string 2";
            var connectionString_default = "some connections string default";
            var provider = new EntityConnectionStringProvider<Log>
            {
                Connections = new ConnectionStringSettingsCollection
                {
                    new ConnectionStringSettings(expectedCustomName, connectionString1),
                    new ConnectionStringSettings("UserSqlRepository", connectionString2),
                    new ConnectionStringSettings(_DefaultConnectionString, connectionString_default)
                }
            };

            // Act
            var actual = provider.Provide();

            // Assert
            Assert.AreEqual(connectionString1, actual);
        }
    }
}