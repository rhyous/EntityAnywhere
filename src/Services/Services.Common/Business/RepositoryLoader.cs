using Rhyous.WebFramework.Interfaces;
using System.Linq;

namespace Rhyous.WebFramework.Services
{
    /// <summary>
    /// A conditional repository loader that implements the custom or common pattern.
    /// It loads a custom repository if it exists, otherwise it loads the common repository.
    /// </summary>
    public class RepositoryLoader
    {
        /// <summary>
        /// Loads a custom repository if it exists, otherwise it loads the common repository.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TInterface">The entity interface type.</typeparam>
        /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string, etc...</typeparam>
        /// <returns>A custom repository if it exists, otherwise it loads the common repository</returns>
        public static IRepository<TEntity, TInterface, TId> Load<TEntity, TInterface, TId>()
            where TEntity : class, TInterface
            where TInterface : IId<TId>
        {
            var repo =  new EntityRepositoryLoader<TEntity, TInterface, TId>().Plugins?.FirstOrDefault()
                ?? new EntityRepositoryLoaderCommon<TEntity, TInterface, TId>().Plugins?.FirstOrDefault();
            return repo;
        }
    }
}
