using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Repositories
{
    /// <summary></summary>
    public class Settings<TEntity> : ISettings<TEntity>
    {
        public bool AutomaticMigrationDataLossAllowed { get; }
        public bool AutomaticMigrationsEnabled { get; }
        public string ContextKey { get; } = $"EAF.{typeof(TEntity).Name}";
        public bool UseEntityFrameworkDatabaseManagement { get; }
        public bool LazyLoadingEnabled { get; }
        public bool ProxyCreationEnabled { get; }
    }
}