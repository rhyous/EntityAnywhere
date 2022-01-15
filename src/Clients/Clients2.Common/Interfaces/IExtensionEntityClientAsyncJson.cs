using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{
    public interface IExtensionEntityClientAsync : IEntityClientAsync
    {
        Task<string> GetByEntityIdentifiersAsync(IEnumerable<EntityIdentifier> entityIdentifiers, bool forwardExceptions = true);
        Task<string> GetByEntityPropertyValueAsync(string entity, string property, string value, bool forwardExceptions = true);
    }

    public interface IAdminExtensionEntityClientAsync 
        : IExtensionEntityClientAsync { }

    public interface IExtensionEntityClientAsync<TEntity, TId> 
        : IEntityClientAsync<TEntity, TId>
        where TEntity : class, IId<TId>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        Task<OdataObjectCollection<TEntity, TId>> GetByEntityIdentifiersAsync(IEnumerable<EntityIdentifier> entityIdentifiers, bool forwardExceptions = true);
        Task<OdataObjectCollection<TEntity, TId>> GetByEntityPropertyValueAsync(string entity, string property, string value, bool forwardExceptions = true);
    }
    public interface IAdminExtensionEntityClientAsync<TEntity, TId> 
        : IExtensionEntityClientAsync<TEntity, TId>
        where TEntity : class, IId<TId>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    { }
}
