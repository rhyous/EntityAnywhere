using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// The REST handler for get count. Which means using the Odata parameter $Count.
    /// </summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    /// <typeparam name="TInterface">The entity interface.</typeparam>
    /// <typeparam name="TId">The entity Id type.</typeparam>
    /// <remarks>This is only generic because Autofac registration needs it to be.</remarks>
    public interface IGetCountHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        Task<int> HandleAsync();
    }
}