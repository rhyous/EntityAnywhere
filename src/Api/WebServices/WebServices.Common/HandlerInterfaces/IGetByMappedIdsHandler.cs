using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IGetByMappedIdsHandler<TEntity, TInterface, TId, TProp> : IGetByPropertyValuesHandler<TEntity, TInterface, TId, TProp>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
    }
}