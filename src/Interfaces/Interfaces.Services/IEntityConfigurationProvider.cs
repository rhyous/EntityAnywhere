using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// Responsible for Creating an <see cref="IEntityConfiguration"/> from the Entity Type
    /// </summary>
    public interface IEntityConfigurationProvider
    {
        /// <summary>
        /// Asynchronously retrieve the entity configuration
        /// </summary>
        /// <param name="entityType">The entity type to use</param>
        /// <returns>A <see cref="Task{TResult}"/> of <see cref="IEntityConfiguration"/></returns>
        Task<IEntityConfiguration> ProvideAsync(Type entityType);

        /// <summary>
        /// Synchronously retrieve the entity configuration
        /// </summary>
        /// <param name="entityType">The entity type to use</param>
        /// <returns>The <see cref="IEntityConfiguration"/></returns>
        IEntityConfiguration Provide(Type entityType);
    }
}