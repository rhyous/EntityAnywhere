using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// The REST handler for getting a property value.    
    /// </summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    /// <typeparam name="TInterface">The entity interface.</typeparam>
    /// <typeparam name="TId">The entity Id type.</typeparam>
    /// <remarks>This is only generic because Autofac registration needs it to be.</remarks>
    public interface IGetPropertyHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        string Handle(string id, string property);
    }
}
