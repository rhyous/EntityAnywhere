using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IGetByMappingsHandler<TEntity, TInterface, TId, TE1Id, TE2Id>
        where TEntity : class, TInterface, new()
        where TInterface : IMappingEntity<TE1Id, TE2Id>, IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
        where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
    {
        Task<OdataObjectCollection<TEntity, TId>> HandleAsync(IEnumerable<TEntity> mappings);
    }
}
