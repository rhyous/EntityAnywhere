using LinqKit;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Rhyous.Collections;
using Rhyous.EntityAnywhere.Services.RelatedEntities;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class GetByMappingsHandler<TEntity, TInterface, TId, TE1Id, TE2Id>
        : IGetByMappingsHandler<TEntity, TInterface, TId, TE1Id, TE2Id>
        where TEntity : class, TInterface, new()
        where TInterface : IMappingEntity<TE1Id, TE2Id>, IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
        where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
    {
        private readonly IServiceCommon<TEntity, TInterface, TId> _Service;
        private readonly IEntityInfoMapping<TEntity, TE1Id, TE2Id> _EntityInfoMapping;
        private readonly IRelatedEntityProvider<TEntity, TInterface, TId> _RelatedEntityProvider;
        private readonly IUrlParameters _UrlParameters;
        private readonly IRequestUri _RequestUri;

        public GetByMappingsHandler(IServiceCommon<TEntity, TInterface, TId> service,
                                    IEntityInfoMapping<TEntity, TE1Id, TE2Id> entityInfoMapping,
                                    IRelatedEntityProvider<TEntity, TInterface, TId> relatedEntityProvider,
                                    IUrlParameters urlParameters,
                                    IRequestUri requestUri)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
            _EntityInfoMapping = entityInfoMapping;
            _RelatedEntityProvider = relatedEntityProvider;
            _UrlParameters = urlParameters;
            _RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
        }

        public async Task<OdataObjectCollection<TEntity, TId>> HandleAsync(IEnumerable<TEntity> mappings)
        {
            if (mappings == null || !mappings.Any())
                throw new RestException(HttpStatusCode.BadRequest);
            var urlParams = _UrlParameters.Collection;
            var expression = mappings.ToExpression<TEntity, TInterface, TId, TE1Id, TE2Id>(_EntityInfoMapping);
            var queryable = _Service.Get(expression, urlParams.Get("$top", -1), urlParams.Get("$skip", -1));
            var countQueryable = _Service.Get(expression);
            var entityCollection = queryable.ToConcrete<TEntity, TInterface>().ToList().AsOdata<TEntity, TId>(_RequestUri.Uri);
            entityCollection.TotalCount = countQueryable.Count();

            if (urlParams.Count > 0)
                await _RelatedEntityProvider.ProvideAsync(entityCollection, urlParams);
            return entityCollection;
        }
    }
}