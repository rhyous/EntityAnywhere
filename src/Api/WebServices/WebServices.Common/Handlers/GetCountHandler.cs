using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class GetCountHandler<TEntity, TInterface, TId> : IGetCountHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IServiceCommon<TEntity, TInterface, TId> _Service;
        private readonly IUrlParameters _UrlParameters;

        public GetCountHandler(IServiceCommon<TEntity, TInterface, TId> service,
                               IUrlParameters urlParameters)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
            _UrlParameters = urlParameters ?? throw new ArgumentNullException(nameof(urlParameters));
        }

        public async Task<int> HandleAsync()
        {
            return await _Service.GetCountAsync(_UrlParameters.Collection.Clone(keysToExclude: new[] { "$top", "$skip" }));
        }
    }
}