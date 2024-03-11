using Rhyous.Odata;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using Rhyous.Wrappers;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class GetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey> : IGetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
    {
        private readonly IServiceCommonAlternateKey<TEntity, TInterface, TId, TAltKey> _Service;
        private readonly IRelatedEntityProvider<TEntity, TInterface, TId> _RelatedEntityProvider;
        private readonly IIdDisambiguator<TEntity, TId, TAltKey> _IdDisambiguator;
        private readonly IUrlParameters _UrlParameters;
        private readonly IRequestUri _RequestUri;
        private readonly IGetByIdHandler<TEntity, TInterface, TId> _GetByIdHandler;
        private readonly IHttpStatusCodeSetter _HttpStatusCodeSetter;

        public GetByAlternateKeyHandler(IServiceCommonAlternateKey<TEntity, TInterface, TId, TAltKey> service,
                                        IRelatedEntityProvider<TEntity, TInterface, TId> relatedEntityProvider,
                                        IIdDisambiguator<TEntity, TId, TAltKey> idDisambiguator,
                                        IUrlParameters urlParameters,
                                        IRequestUri requestUri,
                                        IGetByIdHandler<TEntity, TInterface, TId> getByIdHandler,
                                        IHttpStatusCodeSetter httpStatusCodeSetter)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
            _RelatedEntityProvider = relatedEntityProvider ?? throw new ArgumentNullException(nameof(relatedEntityProvider));
            _IdDisambiguator = idDisambiguator ?? throw new ArgumentNullException(nameof(idDisambiguator));
            _UrlParameters = urlParameters ?? throw new ArgumentNullException(nameof(urlParameters));
            _RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
            _GetByIdHandler = getByIdHandler;
            _HttpStatusCodeSetter = httpStatusCodeSetter;
        }

        public async Task<OdataObject<TEntity, TId>> HandleAsync(string idOrAltId)
        {
            if (string.IsNullOrWhiteSpace(idOrAltId) || string.IsNullOrWhiteSpace(idOrAltId = idOrAltId.Unquote().UnescapeQuotes()))
                throw new RestException(HttpStatusCode.BadRequest);
            var disambiguated = _IdDisambiguator.Disambiguate(idOrAltId);
            if (disambiguated.IdType != IdType.Alt || string.IsNullOrWhiteSpace(disambiguated.AlternateIdProperty) 
            || !disambiguated.AlternateIdProperty.Equals(IdDisambiguator.Key, StringComparison.OrdinalIgnoreCase))
                return await _GetByIdHandler.HandleAsync(idOrAltId);
            var entity = _Service.GetByAlternateKey(disambiguated.AltId);
            if (entity == null)
            {
                _HttpStatusCodeSetter.StatusCode = HttpStatusCode.NotFound;
                return null;
            }
            var concrete = entity.ToConcrete<TEntity, TInterface>().AsOdata<TEntity, TId>(_RequestUri.Uri);
            await _RelatedEntityProvider.ProvideAsync(new[] { concrete }, _UrlParameters.Collection);
            return concrete;
        }
    }
}