using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IGetPropertyValueHandler<TEntity, TInterface, TId>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        string GetSelectProperty(TId Id, string property);
        TInterface GetSelectProperties(TId id, params string[] properties);
    }
}