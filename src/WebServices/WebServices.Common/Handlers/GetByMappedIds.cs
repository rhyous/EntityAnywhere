using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class GetByMappedIdsHandler<TEntity, TInterface, TId, TProp> : IGetByPropertyValuesHandler<TEntity, TInterface, TId, TProp>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IServiceCommon<TEntity, TInterface, TId> _Service;
        private readonly IRelatedEntityProvider<TEntity, TInterface, TId> _RelatedEntityProvider;
        private readonly IUrlParameters _UrlParameters;
        private readonly IRequestUri _RequestUri;

        public GetByMappedIdsHandler(IServiceCommon<TEntity, TInterface, TId> service,
                                     IRelatedEntityProvider<TEntity, TInterface, TId> relatedEntityProvider,
                                     IUrlParameters urlParameters,
                                     IRequestUri requestUri)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
            _RelatedEntityProvider = relatedEntityProvider ?? throw new ArgumentNullException(nameof(relatedEntityProvider));
            _UrlParameters = urlParameters ?? throw new ArgumentNullException(nameof(urlParameters));
            _RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
        }

        public async Task<OdataObjectCollection<TEntity, TId>> HandleAsync(string propertyName, List<TProp> ids)
        {
            if (string.IsNullOrWhiteSpace(propertyName) || ids == null || !ids.Any())
                throw new RestException(HttpStatusCode.BadRequest);
            var lambda = propertyName.ToLambda<TEntity, TProp>(ids);
            var countQueryable = _Service.Get(lambda);
            var entities = _Service.Get(lambda, _UrlParameters.Collection.Get("$top", -1), _UrlParameters.Collection.Get("$skip", -1))?
                                   .ToConcrete<TEntity, TInterface>().ToList().AsOdata<TEntity, TId>(_RequestUri.Uri);
            await _RelatedEntityProvider.ProvideAsync(entities, _UrlParameters.Collection);
            entities.TotalCount = countQueryable.Count();
            return entities;
        }
    }
}