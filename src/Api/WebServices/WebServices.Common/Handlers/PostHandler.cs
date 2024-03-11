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
    public class PostHandler<TEntity, TInterface, TId> : IPostHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IRelatedEntityEnforcer<TEntity> _RelatedEntityEnforcer;
        private readonly IDistinctPropertiesEnforcer<TEntity, TInterface, TId> _DistinctPropertiesEnforcer;
        private readonly IEntityEventAll<TEntity, TId> _EntityEvent;
        private readonly IServiceCommon<TEntity, TInterface, TId> _Service;
        private readonly IRelatedEntityProvider<TEntity, TInterface, TId> _RelatedEntityProvider;
        private readonly IUrlParameters _UrlParameters;
        private readonly IRequestUri _RequestUri;

        public PostHandler(IRelatedEntityEnforcer<TEntity> relatedEntityEnforcer, 
                           IDistinctPropertiesEnforcer<TEntity, TInterface, TId> distinctPropertiesEnforcer,
                           IEntityEventAll<TEntity, TId> entityEvent,
                           IServiceCommon<TEntity, TInterface, TId> service,
                           IRelatedEntityProvider<TEntity, TInterface, TId> relatedEntityProvider,
                           IUrlParameters urlParameters,
                           IRequestUri requestUri)
        {
            _RelatedEntityEnforcer = relatedEntityEnforcer ?? throw new ArgumentNullException(nameof(relatedEntityEnforcer));
            _DistinctPropertiesEnforcer = distinctPropertiesEnforcer ?? throw new ArgumentNullException(nameof(distinctPropertiesEnforcer));
            _EntityEvent = entityEvent ?? throw new ArgumentNullException(nameof(entityEvent));
            _Service = service ?? throw new ArgumentNullException(nameof(service));
            _RelatedEntityProvider = relatedEntityProvider ?? throw new ArgumentNullException(nameof(relatedEntityProvider));
            _UrlParameters = urlParameters ?? throw new ArgumentNullException(nameof(urlParameters));
            _RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
        }

        public async Task<OdataObjectCollection<TEntity, TId>> HandleAsync(List<TEntity> entities)
        {
            if (entities == null || !entities.Any())
                throw new RestException(HttpStatusCode.BadRequest);
            entities.ToList().TrimList();
            await _RelatedEntityEnforcer.Enforce(entities);    // Check to be sure RelatedEntities actually exist
            await _DistinctPropertiesEnforcer.Enforce(entities, ChangeType.Create);
            _EntityEvent?.BeforePost(entities);
            var queryable = await _Service.AddAsync(entities.ToList<TInterface>());
            var createdEntities = queryable.ToConcrete<TEntity, TInterface>()
                                           .AsOdata<TEntity, TId>(_RequestUri.Uri);
            _EntityEvent?.AfterPost(createdEntities.Select(o => o.Object));
            await _RelatedEntityProvider.ProvideAsync(createdEntities, _UrlParameters.Collection);
            return createdEntities;
        }
    }
}