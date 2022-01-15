using Rhyous.EntityAnywhere.Attributes;
using System;
using System.Configuration;

namespace Rhyous.EntityAnywhere.Repositories
{
    /// <summary>
    /// Provides the name of the connection string for the entity.
    /// This uses the custom or common pattern. If a custom entity connection string exists,
    /// use it, otherwise use the common entity connection string.
    /// Example: For a User entity, if a connection string named UserSqlRepository exists,
    /// us it, otherwise use the common entity connection string call SqlRepository.
    /// </summary>
    public class EntityConnectionStringNameProvider<TEntity> : IEntityConnectionStringNameProvider<TEntity>
    {
        internal static string DefaultSqlConnectionStringName = "SqlRepository";

        public string Provide()
        {
            // Get by entity name
            var entityName = typeof(TEntity).Name;
            var entityConnectionStringName = entityName + DefaultSqlConnectionStringName;
            if (Connections[entityConnectionStringName] != null)
                return entityConnectionStringName;

            // Get by entity group name
            var entityGroup = typeof(TEntity).GetAttribute<EntitySettingsAttribute>()?.Group;
            if (!string.IsNullOrWhiteSpace(entityGroup))
            {
                entityConnectionStringName = entityGroup + DefaultSqlConnectionStringName;
                if (Connections[entityConnectionStringName] != null)
                    return entityConnectionStringName;
            }

            // Get default
            if (Connections[DefaultSqlConnectionStringName] != null)
                return DefaultSqlConnectionStringName;
            throw new InvalidOperationException($"No connection string was found in the web.config. A connection string name either {entityConnectionStringName} or {DefaultSqlConnectionStringName} is required.");
        }

        internal ConnectionStringSettingsCollection Connections
        {
            get { return _Connections ?? (_Connections = ConfigurationManager.ConnectionStrings); }
            set { _Connections = value; }
        } private ConnectionStringSettingsCollection _Connections;
    }
}