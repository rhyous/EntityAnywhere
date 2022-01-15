using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.WebServices
{
    [CustomWebService("Entity1WebServices", typeof(IEntity1WebService), typeof(Entity1), "Entity1Service.svc")]
    public sealed class Entity1WebService : EntityWebService<Entity1, IEntity1, long>,
                                            IEntity1WebService
    {
        private readonly IRelatedEntityProvider<Entity1, IEntity1, long> _RelatedEntityProvider;
        private readonly IUrlParameters _UrlParameters;
        private readonly IRequestUri _RequestUri;

        public Entity1WebService(IRestHandlerProviderAlternateKey<Entity1, IEntity1, long, string> restHandlerProvider,
                                 IServiceCommon<Entity1, IEntity1, long> service,
                                 IRelatedEntityProvider<Entity1, IEntity1, long> relatedEntityProvider,
                                 IUrlParameters urlParameters,
                                 IRequestUri requestUri) 
            : base(restHandlerProvider, service)
        {
            _RelatedEntityProvider = relatedEntityProvider ?? throw new ArgumentNullException(nameof(relatedEntityProvider));
            _UrlParameters = urlParameters ?? throw new ArgumentNullException(nameof(urlParameters));
            _RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
        }

        [ExcludeFromCodeCoverage] // Exclude because it simply forwards on.
        public async Task<OdataObjectCollection<Entity1, long>> NewEndpoint()
        {
            throw new NotImplementedException();
        }
    }
}