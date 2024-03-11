using Newtonsoft.Json;

namespace Rhyous.EntityAnywhere.Clients2
{
    public partial interface IEntityClientConnectionSettings
    {
        string EntityName { get; }
        string EntityNamePluralized { get; }
        string ServiceUrl { get; }
        JsonSerializerSettings JsonSerializerSettings { get; }
    }

    public partial interface IEntityClientConnectionSettings<TEntity> : IEntityClientConnectionSettings
    {
    }
}