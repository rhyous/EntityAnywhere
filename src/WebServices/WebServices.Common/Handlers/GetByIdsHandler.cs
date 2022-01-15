using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class GetByIdsHandler<TEntity, TInterface, TId> : IGetByIdsHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IServiceCommon<TEntity, TInterface, TId> _Service;
        private readonly IRelatedEntityProvider<TEntity, TInterface, TId> _RelatedEntityProvider;
        private readonly IUrlParameters _UrlParameters;
        private readonly IRequestUri _RequestUri;

        public GetByIdsHandler(IServiceCommon<TEntity, TInterface, TId> service,
                               IRelatedEntityProvider<TEntity, TInterface, TId> relatedEntityProvider,
                               IUrlParameters urlParameters,
                               IRequestUri requestUri)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
            _RelatedEntityProvider = relatedEntityProvider ?? throw new ArgumentNullException(nameof(relatedEntityProvider));
            _UrlParameters = urlParameters ?? throw new ArgumentNullException(nameof(urlParameters));
            _RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
        }

        public async Task<OdataObjectCollection<TEntity, TId>> HandleAsync(List<TId> ids)
        {
            if (ids == null || !ids.Any())
                throw new RestException(HttpStatusCode.BadRequest);
            var countQueryable = await _Service.GetAsync(ids, _UrlParameters.Collection.Clone(keysToExclude: new[] { "$top", "$skip" }));
            var queryable = await _Service.GetAsync(ids, _UrlParameters.Collection);
            var entities = queryable?.ToConcrete<TEntity, TInterface>().AsOdata<TEntity, TId>(_RequestUri.Uri);
            await _RelatedEntityProvider.ProvideAsync(entities, _UrlParameters.Collection);
            entities.TotalCount = countQueryable.Count();
            return entities;
        }
    }
}