using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    public interface IEntityClientAsync<T, Tid> : IEntityClient<T, Tid>
        where T : class, new()
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
    {
        #region Async
        Task<EntityMetadata<T>> GetMetadataAsync();
        
        Task<List<OdataObject<T>>> GetAllAsync();

        Task<List<OdataObject<T>>> GetAllAsync(string queryParameters);

        Task<List<OdataObject<T>>> GetByIdsAsync(List<Tid> ids);
        
        Task<OdataObject<T>> GetAsync(string id);
        
        Task<string> GetPropertyAsync(string id, string property);
        
        Task<string> UpdatePropertyAsync(string id, string property, string value);
        
        Task<List<OdataObject<T>>> PostAsync(List<T> entity);
        
        Task<OdataObject<T>> PutAsync(string id, T entity);
        
        Task<OdataObject<T>> PatchAsync(string id, PatchedEntity<T> patchedEntity);
        
       Task<bool> DeleteAsync(string id);
        
        Task<List<Addendum>> GetAddendaAsync(string id);
        
        Task<List<Addendum>> GetAddendaByEntityIdsAsync(List<string> ids);
        
        Task<Addendum> GetAddendaByNameAsync(string id, string name);

        Task<List<OdataObject<T>>> GetByIdsAsync(IEnumerable<Tid> ids);

        Task<List<OdataObject<T>>> GetByCustomUrlAsync(string url);
        #endregion
    }
}
