using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IGetByIdsHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        Task<OdataObjectCollection<TEntity, TId>> HandleAsync(List<TId> ids);
    }
}