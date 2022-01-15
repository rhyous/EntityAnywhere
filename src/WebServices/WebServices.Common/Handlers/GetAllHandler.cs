using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class GetAllHandler<TEntity, TInterface, TId> : IGetAllHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IServiceCommon<TEntity, TInterface, TId> _Service;
        private readonly IRelatedEntityProvider<TEntity, TInterface, TId> _RelatedEntityProvider;
        private readonly IUrlParameters _urlParametersContainer;
        private readonly IRequestUri _RequestUri;

        public GetAllHandler(IServiceCommon<TEntity, TInterface, TId> service,
                             IRelatedEntityProvider<TEntity, TInterface, TId> relatedEntityProvider,
                             IUrlParameters urlParameters,
                             IRequestUri requestUri)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
            _RelatedEntityProvider = relatedEntityProvider ?? throw new ArgumentNullException(nameof(relatedEntityProvider));
            _urlParametersContainer = urlParameters ?? throw new ArgumentNullException(nameof(urlParameters));
            _RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
        }

        public async Task<OdataObjectCollection<TEntity, TId>> HandleAsync()
        {
            var urlParams = _urlParametersContainer.Collection;
            var queryable = await _Service.GetAsync(urlParams);
            var entityCollection = queryable.ToConcrete<TEntity, TInterface>().ToList().AsOdata<TEntity, TId>(_RequestUri.Uri);
            var countQueryable = await _Service.GetAsync(urlParams.Clone(keysToExclude: new[] { "$top", "$skip" }));
            entityCollection.TotalCount = countQueryable.Count();
            if (urlParams.Count > 0)
                await _RelatedEntityProvider.ProvideAsync(entityCollection, urlParams);
            return entityCollection;
        }

        public Task<List<TInterface>> GetByQueryableAsync(Expression<Func<TEntity, bool>> expression)
        {
            var queryable = _Service.Get(expression);
            return Task.FromResult(queryable.ToList());
        }
    }
}