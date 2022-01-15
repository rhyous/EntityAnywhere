using Autofac;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.WebServices
{
    public class UserRestHandlerProvider : RestHandlerProviderAlternateKey<User, IUser, long, string>,
                                           IUserRestHandlerProvider
    {

        public UserRestHandlerProvider(ILifetimeScope container)
            : base(container)
        {
        }

        public IFilterHandler FilterHandler => _Container.Resolve<IFilterHandler>();
    }
}