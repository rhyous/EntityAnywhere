using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IGetByEntityIdentifiers<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        OdataObjectCollection<TEntity, TId> Handle(IEnumerable<EntityIdentifier> entityIdentifiers);
        OdataObjectCollection<TEntity, TId> Handle(string entity, List<string> entityIds);
    }
}
