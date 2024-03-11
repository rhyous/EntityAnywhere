using System.Data.Entity.Migrations;

namespace Rhyous.EntityAnywhere.Repositories
{
    /// <summary>
    /// This class configures the DbContext. It sets the AutomaticMigrationsEnabled, AutomaticMigrationDataLossAllowed, and ContextKey settings.
    /// Other settings could be added in the future.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal sealed class MigrationsConfigurationContainer<TEntity> : IMigrationsConfigurationContainer<TEntity> where TEntity : class
    {
        private readonly ISettings<TEntity> _Settings;

        public DbMigrationsConfiguration<BaseDbContext<TEntity>> Config { get; }

        public MigrationsConfigurationContainer(ISettings<TEntity> settings)
        {
            _Settings = settings;
            Config = new DbMigrationsConfiguration<BaseDbContext<TEntity>>();
            Config.AutomaticMigrationsEnabled = _Settings.AutomaticMigrationsEnabled;
            Config.AutomaticMigrationDataLossAllowed = _Settings.AutomaticMigrationDataLossAllowed;
            Config.ContextKey = _Settings.ContextKey;
        }
    }
}