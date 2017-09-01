using Rhyous.WebFramework.Interfaces;
using System.IO;

namespace Rhyous.WebFramework.Services
{
    /// <summary>
    /// This loads a custom Repository plugin for a given entity, if a custom plugin exists.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string, etc...</typeparam>
    public class EntityRepositoryLoader<TEntity, TInterface, TId> : PluginLoaderBase<IRepository<TEntity, TInterface, TId>>
        where TEntity : TInterface
    {
        public override bool ThrowExceptionIfNoPluginFound => false;
        public override string PluginSubFolder => Path.Combine("Repositories", typeof(TEntity).Name);
    }

}