using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.PluginLoaders;
using Rhyous.WebFramework.Services;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.WebServices
{
    class UserServiceProxy : ServiceProxyAlternateKey<User, IUser, long, string>, IUserService
    {
        public UserServiceProxy(IEntityServiceLoader<IServiceCommonAlternateKey<User, IUser, long, string>, User, IUser, long> serviceLoader)
            : base(serviceLoader)
        {
        }

        public IUserService UserService => Service as IUserService;

        public Task<IQueryable<IUser>> FilterUsersAsync(string filters, NameValueCollection urlParameters)
            => UserService.FilterUsersAsync(filters, urlParameters);
    }
}