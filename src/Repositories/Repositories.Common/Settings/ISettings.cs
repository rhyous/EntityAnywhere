namespace Rhyous.WebFramework.Repositories
{
    /// <summary>
    /// An interface for common Entity Framework configuration settings.
    /// </summary>
    public interface ISettings
    {
        bool AutomaticMigrationDataLossAllowed { get; }
        bool AutomaticMigrationsEnabled { get; }
        string ContextKey { get; }
        bool UseEntityFrameworkDatabaseManagement { get; }
    }
}