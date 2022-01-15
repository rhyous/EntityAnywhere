using Autofac;
using Rhyous.Odata;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using Rhyous.WebFramework.Services.RelatedEntities;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.WebServices
{
    class FilterHandler : IFilterHandler
    {
        private readonly IRelatedEntityProvider<User, IUser, long> _RelatedEntityProvider;
        private readonly IUrlParameters _UrlParameters;
        private readonly IUserService _UserService;
        private readonly IRequestUri _RequestUri;

        public FilterHandler(ILifetimeScope container,
                             IRelatedEntityProvider<User, IUser, long> relatedEntityProvider,
                             IUrlParameters urlParameters,
                             IUserService UserService,
                             IRequestUri requestUri)
        {
            _RelatedEntityProvider = relatedEntityProvider;
            _UrlParameters = urlParameters;
            _UserService = UserService;
            _RequestUri = requestUri;
        }

        public async Task<OdataObjectCollection<User, long>> FilterAsync()
        {
            var urlParameters = _UrlParameters.Collection;
            if (urlParameters == null || urlParameters.Count == 0)
                return null;
            var queryable = await _UserService.FilterUsersAsync(urlParameters.Get("$filter"), urlParameters);
            if (queryable == null)
                return new User[] { }.AsOdata<User, long>(_RequestUri.Uri); //Empty Odata object
            var entityCollection = queryable.ToConcrete<User, IUser>().ToList().AsOdata<User, long>(_RequestUri.Uri);
            if (_UrlParameters.Collection.Count > 0)
                await _RelatedEntityProvider.ProvideAsync(entityCollection, _UrlParameters.Collection);
            return entityCollection;
        }
    }
}
