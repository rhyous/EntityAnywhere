using Rhyous.Collections;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Repositories
{
    /// <summary>
    /// This class provides standard settings for the common Entity Framework-based common repository.
    /// However, it also allows for each entity to have its own custom settings.
    /// Just append to the defualt setting name which entity it is for.
    /// Example: AutomaticMigrationsEnabled becomes AutomaticMigrationsEnabledForEntity1
    /// </summary>
    public class Settings<TEntity> : ISettings<TEntity>
    {
        private const string Miscellaneous = nameof(Miscellaneous);

        private readonly IAppSettings _AppSettings;

        public Settings(IAppSettings appSettings)
        {
            _AppSettings = appSettings;
        }

        internal string Entity => typeof(TEntity).Name;
        internal string Group => typeof(TEntity).GetAttribute<EntitySettingsAttribute>()?.Group ?? Miscellaneous;

        public const string UseEntityFrameworkDatabaseManagementSetting = "UseEntityFrameworkDatabaseManagement";
        public const bool UseEntityFrameworkDatabaseManagementDefault = true;
        /// <summary>
        /// This setting turns on or off Entity Framework database management. Default is true.
        /// </summary>
        public bool UseEntityFrameworkDatabaseManagement
            => _AppSettings.Collection.Get($"{UseEntityFrameworkDatabaseManagementSetting}For{Entity}",
               _AppSettings.Collection.Get($"{UseEntityFrameworkDatabaseManagementSetting}For{Group}",
               _AppSettings.Collection.Get(UseEntityFrameworkDatabaseManagementSetting, UseEntityFrameworkDatabaseManagementDefault)));

        public const string AutomaticMigrationsEnabledSetting = "AutomaticMigrationsEnabled";
        public const bool AutomaticMigrationsEnabledDefault = true;
        /// <summary>
        /// This setting turns on or off Entity Framework's AutomaticMigrations. Default is true.
        /// </summary>
        public bool AutomaticMigrationsEnabled 
            => _AppSettings.Collection.Get($"{AutomaticMigrationsEnabledSetting}For{Entity}",
               _AppSettings.Collection.Get($"{AutomaticMigrationsEnabledSetting}For{Group}",
               _AppSettings.Collection.Get(AutomaticMigrationsEnabledSetting, AutomaticMigrationsEnabledDefault)));

        public const string AutomaticMigrationDataLossAllowedSetting = "AutomaticMigrationDataLossAllowed";
        public const bool AutomaticMigrationDataLossAllowedDefault = false;
        /// <summary>
        /// This setting turns on or off Entity Framework's AutomaticMigrationDataLoss. Default is false because if this is true it could result in data loss.
        /// </summary>
        public bool AutomaticMigrationDataLossAllowed
            => _AppSettings.Collection.Get($"{AutomaticMigrationDataLossAllowedSetting}For{Entity}",
               _AppSettings.Collection.Get($"{AutomaticMigrationDataLossAllowedSetting}For{Group}",
               _AppSettings.Collection.Get(AutomaticMigrationDataLossAllowedSetting, AutomaticMigrationDataLossAllowedDefault)));

        public const string ContextKeySetting = "ContextKey";
        public static string ContextKeyDefault = $"EAF.{typeof(TEntity).Name}";
        /// <summary>        
        /// This setting sets the ContextKey used in Entity Framework's __MigrationsHistory table.
        /// The default is EAF.{Entity} where {Entity} is replaced with the Entity name. 
        /// Example: The User entity would be EAF.User.
        /// </summary>
        public string ContextKey
            => _AppSettings.Collection.Get($"{ContextKeySetting}For{Entity}",
               _AppSettings.Collection.Get($"{ContextKeySetting}For{Group}",
               _AppSettings.Collection.Get(ContextKeySetting, ContextKeyDefault)));

        public const string LazyLoadingEnabledSetting = "UseLazyLoading";
        public const bool UseLazyLoadingDefault = true;
        /// <summary>
        /// Enables lazy loading in the dbcontext.
        /// </summary>
        public bool LazyLoadingEnabled
            => _AppSettings.Collection.Get($"{LazyLoadingEnabledSetting}For{Entity}",
               _AppSettings.Collection.Get($"{LazyLoadingEnabledSetting}For{Group}",
               _AppSettings.Collection.Get(LazyLoadingEnabledSetting, UseLazyLoadingDefault)));

        public const string ProxyCreationEnabledSetting = "UseLazyLoading";
        public const bool UseProxyCreationDefault = true;
        /// <summary>
        /// Enables proxy creation in the dbcontext.
        /// </summary>
        public bool ProxyCreationEnabled
            => _AppSettings.Collection.Get($"{ProxyCreationEnabledSetting}For{Entity}",
               _AppSettings.Collection.Get($"{ProxyCreationEnabledSetting}For{Group}",
               _AppSettings.Collection.Get(ProxyCreationEnabledSetting, UseProxyCreationDefault)));
    }
}