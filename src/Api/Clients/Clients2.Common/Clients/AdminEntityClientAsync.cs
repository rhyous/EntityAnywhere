using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class AdminEntityClientAsync<TEntity, TId> : EntityClientAsync<TEntity, TId>, IAdminEntityClientAsync<TEntity, TId>
        where TEntity : class, IId<TId>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public AdminEntityClientAsync(IEntityClientConnectionSettings<TEntity> entityClientSettings, 
                                      IAdminHttpClientRunner httpClientRunner) 
            : base(entityClientSettings, httpClientRunner)
        {
        }
    }
}