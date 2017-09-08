using Rhyous.WebFramework.Interfaces;
using System;
using System.Configuration;

namespace Rhyous.WebFramework.Repositories
{
    /// <summary>
    /// This class provides standard settings for the common Entity Framework-based common repository.
    /// </summary>
    public class Settings : ISettings
    {
        #region Singleton

        private static readonly Lazy<Settings> Lazy = new Lazy<Settings>(() => new Settings());

        public static Settings Instance { get { return Lazy.Value; } }

        private Settings()
        {
        }

        #endregion

        public static string UseEntityFrameworkDatabaseManagementSetting = "UseEntityFrameworkDatabaseManagement";
        public static bool UseEntityFrameworkDatabaseManagementDefault = true;
        /// <summary>
        /// This setting turns on or off Entity Framework database management. Default is true.
        /// </summary>
        public bool UseEntityFrameworkDatabaseManagement { get { return ConfigurationManager.AppSettings.Get(UseEntityFrameworkDatabaseManagementSetting, UseEntityFrameworkDatabaseManagementDefault); } }

        /// <summary>
        /// This setting turns on or off Entity Framework's AutomaticMigrations. Default is true.
        /// </summary>
        public static string AutomaticMigrationsEnabledSetting = "AutomaticMigrationsEnabled";
        public static bool AutomaticMigrationsEnabledDefault = true;
        public bool AutomaticMigrationsEnabled { get { return ConfigurationManager.AppSettings.Get(AutomaticMigrationsEnabledSetting, AutomaticMigrationsEnabledDefault); } }

        /// <summary>
        /// This setting turns on or off Entity Framework's AutomaticMigrationDataLoss. Default is false because if this is true it could result in data loss.
        /// </summary>
        public static string AutomaticMigrationDataLossAllowedSetting = "AutomaticMigrationDataLossAllowed";
        public static bool AutomaticMigrationDataLossAllowedDefault = false;
        public bool AutomaticMigrationDataLossAllowed { get { return ConfigurationManager.AppSettings.Get(AutomaticMigrationDataLossAllowedSetting, AutomaticMigrationDataLossAllowedDefault); } }

        public static string ContextKeySetting = "ContextKey";

        public static string ContextKeyDefault = "EAF.{Entity}";
        /// <summary>        
        /// This setting sets the ContextKey used in Entity Framework's __MigrationsHistory table. Default is EAF.{Entity} where {Entity} is replaced with the Entity name. So the User entity would be EAF.User.
        /// </summary>
        public string ContextKey { get { return ConfigurationManager.AppSettings.Get(ContextKeySetting, ContextKeyDefault); } }        
    }
}
