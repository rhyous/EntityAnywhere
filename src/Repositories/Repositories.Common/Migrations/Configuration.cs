using System.Data.Entity.Migrations;

namespace Rhyous.WebFramework.Repositories
{
    /// <summary>
    /// This class configures the DbContext. It sets the AutomaticMigrationsEnabled, AutomaticMigrationDataLossAllowed, and ContextKey settings.
    /// Other settings could be added in the future.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal sealed class Configuration<TEntity> : DbMigrationsConfiguration<BaseDbContext<TEntity>>
        where TEntity : class
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = ConfigSettings.AutomaticMigrationsEnabled;
            AutomaticMigrationDataLossAllowed = ConfigSettings.AutomaticMigrationDataLossAllowed;
            ContextKey = ConfigSettings.ContextKey.Replace("{Entity}", typeof(TEntity).Name);
        }

        internal ISettings ConfigSettings
        {
            get { return _Settings ?? (_Settings = Settings.Instance); }
            set { _Settings = value; }
        } private ISettings _Settings;
    }
}
