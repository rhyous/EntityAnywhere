using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IGetByPropertyValuesHandler<TEntity, TInterface, TId, TProp>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        Task<OdataObjectCollection<TEntity, TId>> HandleAsync(string property, List<TProp> values);
    }

    public interface IGetByPropertyValuesHandler<TEntity, TInterface, TId> : IGetByPropertyValuesHandler<TEntity, TInterface, TId, string>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
    }
}