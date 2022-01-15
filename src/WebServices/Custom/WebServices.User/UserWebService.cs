using Rhyous.Odata;
using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.WebServices
{
    [CustomWebService("UserWebServices", typeof(IUserWebService), typeof(User), "UserService.svc")]
    public sealed class UserWebService : EntityWebServiceAlternateKey<User, IUser, long, string>,
                                         IUserWebService
    {
        private readonly IUserRestHandlerProvider _UserRestHandlerProvider;

        public UserWebService(IUserRestHandlerProvider userRestHandlerProvider)
            : base(userRestHandlerProvider)
        {
            _UserRestHandlerProvider = userRestHandlerProvider;
        }

        [ExcludeFromCodeCoverage] // Exclude because it simply forwards on.
        public async Task<OdataObjectCollection<User, long>> FilterAsync() => await _UserRestHandlerProvider.FilterHandler.FilterAsync();
    }
}