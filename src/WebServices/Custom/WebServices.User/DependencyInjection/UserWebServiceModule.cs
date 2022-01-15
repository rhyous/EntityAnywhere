using Autofac;
using Rhyous.WebFramework.Services;

namespace Rhyous.WebFramework.WebServices.DependencyInjection
{
    public class UserWebServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserRestHandlerProvider>().As<IUserRestHandlerProvider>();
            builder.RegisterType<FilterHandler>().As<IFilterHandler>();
            builder.RegisterType<UserServiceProxy>().As<IUserService>();
        }
    }
}