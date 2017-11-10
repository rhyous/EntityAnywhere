using Newtonsoft.Json;
using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    /// <summary>
    /// A common class that any client can implement to talk to any entity's web services.
    /// </summary>
    /// <remarks>This inherits EntityClientAsync<![CDATA[<TEntity, TId>]]> and adds method to run the asnyc method synchronously.</remarks>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TId">The entity id type</typeparam>
    public class EntityClient<TEntity, TId> : EntityClientAsync<TEntity, TId>, IEntityClient<TEntity, TId>
        where TEntity : class, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        #region Constructors
        public EntityClient()
        {
        }

        public EntityClient(HttpClient httpClient, bool useMicrosoftDateFormat = false) 
            : this(useMicrosoftDateFormat ? new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat } : null)
        {
            _HttpClient = httpClient;
        }

        public EntityClient(bool useMicrosoftDateFormat) 
            : this(useMicrosoftDateFormat ? new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat } : null)
        {
        }

        public EntityClient(JsonSerializerSettings jsonSerializerSettings)
        {
            JsonSerializerSettings = jsonSerializerSettings;
        }
        #endregion

        public Dictionary<TId, TEntity> Cache { get; set; }

        /// <inheritdoc />
        public bool Delete(string id)
        {
            return TaskRunner.RunSynchonously(DeleteAsync, id);
        }
        
        /// <inheritdoc />
        public OdataObject<TEntity, TId> Get(string idOrName)
        {
            return TaskRunner.RunSynchonously(GetAsync, idOrName);
        }

        /// <summary>
        /// Gets an entity by the AlternateKey. This is only a valid call for Entities
        /// with the AlternateKey attribute. Call is asynchonous.
        /// </summary>
        /// <param name="altKey"></param>
        /// <returns>The entity with the specified alternate key.</returns>
        public OdataObject<TEntity, TId> GetByAlternateKey(string altKey)
        {
            return TaskRunner.RunSynchonously(GetByAlternateKeyAsync, altKey);
        }

        /// <inheritdoc />
        public OdataObjectCollection<TEntity, TId> GetAll()
        {
            return TaskRunner.RunSynchonously(GetAllAsync);
        }

        /// <inheritdoc />
        public OdataObjectCollection<TEntity, TId> GetByCustomUrl(string urlPart)
        {
            return TaskRunner.RunSynchonously(GetByCustomUrlAsync, urlPart);
        }
        
        /// <inheritdoc />
        public OdataObjectCollection<TEntity, TId> GetByCustomUrl(string urlPart, Func<string, HttpContent, Task<HttpResponseMessage>> httpMethod, HttpContent content)
        {
            return TaskRunner.RunSynchonously(GetByCustomUrlAsync, urlPart, httpMethod, content);
        }
        
        /// <inheritdoc />
        public OdataObjectCollection<TEntity, TId> GetByCustomUrl(string urlPart, Func<string, HttpContent, Task<HttpResponseMessage>> httpMethod, object content)
        {
            return TaskRunner.RunSynchonously(GetByCustomUrlAsync, urlPart, httpMethod, content);
        }
        
        /// <inheritdoc />
        public OdataObjectCollection<TEntity, TId> GetByQueryParameters(string queryParameters)
        {
            return TaskRunner.RunSynchonously(GetByQueryParametersAsync, queryParameters);
        }

        /// <inheritdoc />
        public OdataObjectCollection<TEntity, TId> GetByIds(IEnumerable<TId> ids)
        {
            return TaskRunner.RunSynchonously(GetByIdsAsync, ids);
        }
        
        /// <inheritdoc />
        public OdataObjectCollection<TEntity, TId> GetByIds(List<TId> ids)
        {
            return GetByIds((IEnumerable<TId>)ids);
        }
        
        /// <inheritdoc />
        public string GetProperty(string id, string property)
        {
            return TaskRunner.RunSynchonously(GetPropertyAsync, id, property);
        }
        
        /// <inheritdoc />
        public CsdlEntity<TEntity> GetMetadata()
        {
            return TaskRunner.RunSynchonously(GetMetadataAsync);
        }

        /// <inheritdoc />
        public OdataObject<TEntity, TId> Patch(string id, PatchedEntity<TEntity> patchedEntity)
        {
            return TaskRunner.RunSynchonously(PatchAsync, id, patchedEntity);
        }

        /// <inheritdoc />
        public OdataObjectCollection<TEntity, TId> Post(List<TEntity> entities)
        {
            return TaskRunner.RunSynchonously(PostAsync, entities);
        }

        /// <inheritdoc />
        public OdataObject<TEntity, TId> Put(string id, TEntity entity)
        {
            return TaskRunner.RunSynchonously(PutAsync, id, entity);
        }

        /// <inheritdoc />
        public string UpdateProperty(string id, string property, string value)
        {
            return TaskRunner.RunSynchonously(UpdatePropertyAsync, id, property, value);
        }
                
        /// <inheritdoc />
        public OdataObjectCollection<Addendum, long> GetAddenda(string id)
        {
            return TaskRunner.RunSynchonously(GetAddendaAsync, id);
        }

        /// <inheritdoc />
        public Addendum GetAddendaByName(string id, string name)
        {
            return TaskRunner.RunSynchonously(GetAddendaByNameAsync, id, name);
        }

        /// <inheritdoc />
        public OdataObjectCollection<Addendum, long> GetAddendaByEntityIds(List<string> ids)
        {
            return TaskRunner.RunSynchonously(GetAddendaByEntityIdsAsync, ids);
        }

        public int GetCount()
        {
            return TaskRunner.RunSynchonously(GetCountAsync);
        }        
    }
}