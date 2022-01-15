namespace Rhyous.EntityAnywhere.Repositories
{
    /// <summary>
    /// An interface for common Entity Framework configuration settings.
    /// </summary>
    public interface ISettings<TEntity>
    {
        bool AutomaticMigrationDataLossAllowed { get; }
        bool AutomaticMigrationsEnabled { get; }
        string ContextKey { get; }
        bool UseEntityFrameworkDatabaseManagement { get; }
        bool LazyLoadingEnabled { get; }
        bool ProxyCreationEnabled { get; } 
    }
}