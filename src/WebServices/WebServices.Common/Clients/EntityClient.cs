using Newtonsoft.Json;
using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    /// <summary>
    /// A common class that any client can implement to talk to any entity's web services.
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TId">The entity id type</typeparam>
    public class EntityClient<TEntity, TId> : IEntityClient<TEntity, TId>, IEntityClientAsync<TEntity, TId>
        where TEntity : class, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public const string EntitySuffix = "EntityUrl";
        public const string ServiceSuffix = "Service.svc";

        public EntityClient()
        {
        }

        public EntityClient(bool useMicrosoftDateFormat) : this(new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat })
        {
        }

        public EntityClient(JsonSerializerSettings jsonSerializerSettings)
        {
            JsonSerializerSettings = jsonSerializerSettings;
        }

        /// <summary>
        /// A JsonSerializerSettings object, that allows you to customize serialization settings.
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings { get; set; }

        internal HttpClient HttpClient
        {
            get { return _HttpClient ?? (_HttpClient = new HttpClient()); }
            set { _HttpClient = value; }
        } private HttpClient _HttpClient;

        /// <inheritdoc />
        public IHttpContextProvider HttpContextProvider
        {
            get { return _HttpContextProvider ?? (_HttpContextProvider = new HttpContextProvider()); }
            set { _HttpContextProvider = value; }
        } private IHttpContextProvider _HttpContextProvider;

        /// <inheritdoc />
        public string ServiceUrl
        {
            get { return _ServiceUrl ?? (_ServiceUrl = ConfigurationManager.AppSettings.Get($"{Entity}{EntitySuffix}", $"{HttpContextProvider.WebHost}/{typeof(TEntity).Name}{ServiceSuffix}")); }
            set { _ServiceUrl = value; }
        } internal string _ServiceUrl;

        /// <inheritdoc />
        public string Entity => typeof(TEntity).Name;

        /// <inheritdoc />
        public string EntityPluralized => PluralizationDictionary.Instance.GetValueOrDefault(Entity);

        /// <inheritdoc />
        public bool Delete(string id)
        {
            return TaskRunner.RunSynchonously(DeleteAsync, id);
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(string id)
        {
            return await HttpClientRunner.RunAndDeserialize<bool>(HttpClient.DeleteAsync, $"{ServiceUrl}/{EntityPluralized}({id})");
        }

        /// <inheritdoc />
        public OdataObject<TEntity> Get(string idOrName)
        {
            return TaskRunner.RunSynchonously(GetAsync, idOrName);
        }

        /// <inheritdoc />
        public async Task<OdataObject<TEntity>> GetAsync(string idOrName)
        {
            return await HttpClientRunner.RunAndDeserialize<OdataObject<TEntity>>(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}({idOrName})");
        }

        /// <inheritdoc />
        public List<OdataObject<TEntity>> GetAll()
        {
            return TaskRunner.RunSynchonously(GetAllAsync);
        }

        /// <inheritdoc />
        public async Task<List<OdataObject<TEntity>>> GetAllAsync()
        {
            return await HttpClientRunner.RunAndDeserialize<List<OdataObject<TEntity>>>(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}");
        }


        /// <inheritdoc />
        public List<OdataObject<TEntity>> GetByCustomUrl(string urlPart)
        {
            return TaskRunner.RunSynchonously(GetByCustomUrlAsync, urlPart);
        }

        /// <inheritdoc />
        public async Task<List<OdataObject<TEntity>>> GetByCustomUrlAsync(string urlPart)
        {
            return await HttpClientRunner.RunAndDeserialize<List<OdataObject<TEntity>>>(HttpClient.GetAsync, $"{ServiceUrl}/{urlPart}");
        }

        /// <inheritdoc />
        public List<OdataObject<TEntity>> GetByQueryParameters(string queryParameters)
        {
            return TaskRunner.RunSynchonously(GetByCustomUrlAsync, queryParameters);
        }

        /// <inheritdoc />
        public async Task<List<OdataObject<TEntity>>> GetByQueryParametersAsync(string queryParameters)
        {
            return await HttpClientRunner.RunAndDeserialize<List<OdataObject<TEntity>>>(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}?{queryParameters}");
        }

        /// <inheritdoc />
        public List<OdataObject<TEntity>> GetByIds(IEnumerable<TId> ids)
        {
            return TaskRunner.RunSynchonously(GetByIdsAsync, ids);
        }

        /// <inheritdoc />
        public async Task<List<OdataObject<TEntity>>> GetByIdsAsync(IEnumerable<TId> ids)
        {
            return await HttpClientRunner.RunAndDeserialize<IEnumerable<TId>, List<OdataObject<TEntity>>>(HttpClient.PostAsync, $"{ServiceUrl}/{EntityPluralized}/Ids", ids);
        }

        /// <inheritdoc />
        public List<OdataObject<TEntity>> GetByIds(List<TId> ids)
        {
            return GetByIds((IEnumerable<TId>)ids);
        }

        /// <inheritdoc />
        public async Task<List<OdataObject<TEntity>>> GetByIdsAsync(List<TId> ids)
        {
            return await GetByIdsAsync((IEnumerable<TId>)ids);
        }

        /// <inheritdoc />
        public string GetProperty(string id, string property)
        {
            return TaskRunner.RunSynchonously(GetPropertyAsync, id, property);
        }

        /// <inheritdoc />
        public async Task<string> GetPropertyAsync(string id, string property)
        {
            return await HttpClientRunner.Run(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}({id})/{property}");
        }

        /// <inheritdoc />
        public CsdlEntity<TEntity> GetMetadata()
        {
            return TaskRunner.RunSynchonously(GetMetadataAsync);
        }

        /// <inheritdoc />
        public async Task<CsdlEntity<TEntity>> GetMetadataAsync()
        {
            return await HttpClientRunner.RunAndDeserialize<CsdlEntity<TEntity>>(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}/$Metadata");
        }

        /// <inheritdoc />
        public OdataObject<TEntity> Patch(string id, PatchedEntity<TEntity> patchedEntity)
        {
            HttpContent postContent = new StringContent(JsonConvert.SerializeObject(patchedEntity, JsonSerializerSettings), Encoding.UTF8, "application/json");
            Task<HttpResponseMessage> response = HttpClient.PatchAsync($"{ServiceUrl}/{EntityPluralized}({id})", postContent);
            try
            {
                response.Wait();
                var readAsStringTask = response.Result.Content.ReadAsStringAsync();
                readAsStringTask.Wait();
                var result = readAsStringTask.Result;
                return JsonConvert.DeserializeObject<OdataObject<TEntity>>(result);
            }
            catch { return null; }
        }

        /// <inheritdoc />
        public async Task<OdataObject<TEntity>> PatchAsync(string id, PatchedEntity<TEntity> patchedEntity)
        {
            return await HttpClientRunner.RunAndDeserialize<PatchedEntity<TEntity>, OdataObject<TEntity>>(HttpClient.PatchAsync, $"{ServiceUrl}/{EntityPluralized}({id})", patchedEntity);
        }

        /// <inheritdoc />
        public List<OdataObject<TEntity>> Post(List<TEntity> entities)
        {
            return TaskRunner.RunSynchonously(PostAsync, entities);
        }

        /// <inheritdoc />
        public async Task<List<OdataObject<TEntity>>> PostAsync(List<TEntity> entities)
        {
            return await HttpClientRunner.RunAndDeserialize<List<TEntity>, List<OdataObject<TEntity>>>(HttpClient.PostAsync, $"{ServiceUrl}/{EntityPluralized}", entities);
        }

        /// <inheritdoc />
        public OdataObject<TEntity> Put(string id, TEntity entity)
        {
            return TaskRunner.RunSynchonously(PutAsync, id, entity);
        }

        /// <inheritdoc />
        public async Task<OdataObject<TEntity>> PutAsync(string id, TEntity entity)
        {
            return await HttpClientRunner.RunAndDeserialize<TEntity, OdataObject<TEntity>>(HttpClient.PutAsync, $"{ServiceUrl}/{EntityPluralized}({id})", entity);
        }

        /// <inheritdoc />
        public string UpdateProperty(string id, string property, string value)
        {
            return TaskRunner.RunSynchonously(UpdatePropertyAsync, id, property, value);
        }

        public async Task<string> UpdatePropertyAsync(string id, string property, string value)
        {
            return await HttpClientRunner.RunAndDeserialize<string, string>(HttpClient.PostAsync, $"{ServiceUrl}/{EntityPluralized}({id})/{property}", value);
        }

        /// <inheritdoc />
        public List<Addendum> GetAddenda(string id)
        {
            return TaskRunner.RunSynchonously(GetAddendaAsync, id);
        }

        /// <inheritdoc />
        public async Task<List<Addendum>> GetAddendaAsync(string id)
        {
            return await HttpClientRunner.RunAndDeserialize<List<Addendum>>(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}({id})/Addenda");
        }

        /// <inheritdoc />
        public Addendum GetAddendaByName(string id, string name)
        {
            return TaskRunner.RunSynchonously(GetAddendaByNameAsync, id, name);
        }

        /// <inheritdoc />
        public async Task<Addendum> GetAddendaByNameAsync(string id, string name)
        {
            return await HttpClientRunner.RunAndDeserialize<Addendum>(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}({id})/Addenda({name})");
        }

        /// <inheritdoc />
        public List<Addendum> GetAddendaByEntityIds(List<string> ids)
        {
            return TaskRunner.RunSynchonously(GetAddendaByEntityIdsAsync, ids);
        }

        /// <inheritdoc />
        public async Task<List<Addendum>> GetAddendaByEntityIdsAsync(List<string> ids)
        {
            return await HttpClientRunner.RunAndDeserialize<List<string>, List<Addendum>>(HttpClient.PostAsync, $"{ServiceUrl}/{EntityPluralized}/Ids/Addenda", ids);
        }
    }
}