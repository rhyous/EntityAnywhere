using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhyous.Odata;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using Rhyous.Wrappers;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Rhyous.EntityAnywhere.Exceptions;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class GetByIdHandler<TEntity, TInterface, TId> : IGetByIdHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IServiceCommon<TEntity, TInterface, TId> _Service;
        private readonly IRelatedEntityProvider<TEntity, TInterface, TId> _RelatedEntityProvider;
        private readonly IIdDisambiguator<TEntity, TId> _IdDisambiguator;
        private readonly INamedFactory<IAdminExtensionEntityClientAsync> _ExtensionEntityClientFactory;
        private readonly IAdminExtensionEntityClientAsync _AlternateIdClient;
        private readonly IUrlParameters _UrlParameters;
        private readonly IHttpStatusCodeSetter _HttpStatusCodeSetter;
        private readonly IRequestUri _RequestUri;
        private const string AlternateIdEntityName = "AlternateId";

        public GetByIdHandler(IServiceCommon<TEntity, TInterface, TId> service,
                              IRelatedEntityProvider<TEntity, TInterface, TId> relatedEntityProvider,
                              IIdDisambiguator<TEntity, TId> idDisambiguator,
                              INamedFactory<IAdminExtensionEntityClientAsync> extensionEntityClientFactory,
                              IUrlParameters urlParameters,
                              IHttpStatusCodeSetter httpStatusCodeSetter,
                              IRequestUri requestUri)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
            _RelatedEntityProvider = relatedEntityProvider ?? throw new ArgumentNullException(nameof(relatedEntityProvider));
            _IdDisambiguator = idDisambiguator ?? throw new ArgumentNullException(nameof(idDisambiguator));
            _ExtensionEntityClientFactory = extensionEntityClientFactory ?? throw new ArgumentNullException(nameof(extensionEntityClientFactory));
            _AlternateIdClient = _ExtensionEntityClientFactory.Create(AlternateIdEntityName);
            _UrlParameters = urlParameters ?? throw new ArgumentNullException(nameof(urlParameters));
            _HttpStatusCodeSetter = httpStatusCodeSetter;
            _RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
        }

        public async Task<OdataObject<TEntity, TId>> HandleAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(id = id.Unquote().UnescapeQuotes()))
            {
                throw new RestException(HttpStatusCode.BadRequest);
            }
            var disambiguatedId = _IdDisambiguator.Disambiguate(id);
            if (disambiguatedId.IdType == IdType.Alt)
            {
                await GetEntityIdFromAlternateIds(disambiguatedId);
                if (disambiguatedId.IdType != IdType.Id)
                {
                    _HttpStatusCodeSetter.StatusCode = HttpStatusCode.NotFound;
                    return null;
                }
            }
            var entity = await GetByTIdAsync(disambiguatedId.Id);
            if (entity == null)
                _HttpStatusCodeSetter.StatusCode = HttpStatusCode.NotFound;
            return entity;
        }

        internal async Task GetEntityIdFromAlternateIds(DisambiguatedId<TId> disambiguated)
        {
            var json = await _AlternateIdClient.GetByEntityPropertyValueAsync(typeof(TEntity).Name, disambiguated.AlternateIdProperty, disambiguated.AltId, true);
            if (string.IsNullOrWhiteSpace(json))
                return;
            var collection = JsonConvert.DeserializeObject<OdataObjectCollection>(json);
            var entityJson = collection?.FirstOrDefault()?.Object.ToString();
            if (string.IsNullOrWhiteSpace(entityJson))
                return;
            var jObj = JObject.Parse(entityJson);

            disambiguated.AltId = jObj?["EntityId"]?.ToString();
            if (!string.IsNullOrWhiteSpace(disambiguated.AltId))
                disambiguated.Id = (TId)disambiguated.AltId.ToType(typeof(TId));
            disambiguated.IdType = IdType.Id;
        }

        public async Task<OdataObject<TEntity, TId>> GetByTIdAsync(TId id)
        {
            if (id.Equals(default(TId)))
                return null;
            var entity = _Service.Get(id)?.ToConcrete<TEntity, TInterface>().AsOdata<TEntity, TId>(_RequestUri.Uri);
            await _RelatedEntityProvider.ProvideAsync(new[] { entity }, _UrlParameters.Collection);
            return entity;
        }
    }
}