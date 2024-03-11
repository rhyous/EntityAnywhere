using Newtonsoft.Json;
using Rhyous.StringLibrary;
using Rhyous.StringLibrary.Pluralization;
using System.Diagnostics.CodeAnalysis;

namespace Rhyous.EntityAnywhere.Clients2
{
    [ExcludeFromCodeCoverage]
    public partial class EntityClientConnectionSettings : IEntityClientConnectionSettings
    {
        private readonly IEntityClientConfig Config;

        public EntityClientConnectionSettings(string entityName, IEntityClientConfig config)
        {
            EntityName = entityName;
            Config = config;
        }

        public virtual string EntityName { get; }
        public virtual string EntityNamePluralized => EntityName.Pluralize();
        public virtual string ServiceUrl => string.IsNullOrEmpty(Config.EntitySubpath) ? StringConcat.WithSeparator('/', Config.EntityWebHost, $"{EntityName}Service") : StringConcat.WithSeparator('/', Config.EntityWebHost, Config.EntitySubpath, $"{EntityName}Service");
        public virtual JsonSerializerSettings JsonSerializerSettings { get; }
    }

    [ExcludeFromCodeCoverage]
    public partial class EntityClientConnectionSettings<TEntity> : EntityClientConnectionSettings, IEntityClientConnectionSettings<TEntity>
    {
        public EntityClientConnectionSettings(IEntityClientConfig config) : base(typeof(TEntity).Name, config)
        {
        }
    }
}