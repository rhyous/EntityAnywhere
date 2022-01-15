using Autofac;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.DependencyInjection
{
    public class UserServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DuplicateUsernameDetector>()
                   .As<IDuplicateUsernameDetector>();

            builder.RegisterType<PasswordManager>()
                   .As<IPasswordManager>();

            builder.RegisterType<UserService>()
                   .As<IServiceCommonAlternateKey<User, IUser, long, string>>()
                   .As<IServiceCommon<User, IUser, long>>()
                   .As<IUserService>();
        }
    }
}