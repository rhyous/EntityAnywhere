using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class AdminMappingEntityClientAsync<TEntity, TId, TE1Id, TE2Id>
        : MappingEntityClientAsync<TEntity, TId, TE1Id, TE2Id>,
          IAdminMappingEntityClientAsync<TEntity, TId, TE1Id, TE2Id>
        where TEntity : class, IId<TId>, IMappingEntity<TE1Id, TE2Id>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
        where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
    {
        public AdminMappingEntityClientAsync(IEntityClientConnectionSettings<TEntity> entityClientConnectionSettings,
                                             IMappingEntitySettings<TEntity> mappingEntitySettings,
                                             IAdminHttpClientRunner httpClientRunner)
       : base(entityClientConnectionSettings, mappingEntitySettings, httpClientRunner)
        {
        }
    }
}