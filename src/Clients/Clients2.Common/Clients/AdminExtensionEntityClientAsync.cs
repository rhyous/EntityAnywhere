using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class AdminExtensionEntityClientAsync<TEntity, TId>
        : ExtensionEntityClientAsync<TEntity, TId>,
          IAdminExtensionEntityClientAsync<TEntity, TId>
        where TEntity : class, IId<TId>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public AdminExtensionEntityClientAsync(IEntityClientConnectionSettings<TEntity> entityClientSettings,
                                               IAdminHttpClientRunner httpClientRunner)
            : base(entityClientSettings, httpClientRunner)
        {
        }
    }
}