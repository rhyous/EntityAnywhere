using Autofac;

namespace Rhyous.EntityAnywhere.Repositories.DependencyInjection
{
    public class DbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuditablesHandler>()
                   .As<IAuditablesHandler>()
                   .SingleInstance();
            builder.RegisterGeneric(typeof(EntityConnectionStringNameProvider<>))
                   .As(typeof(IEntityConnectionStringNameProvider<>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(Settings<>))
                   .As(typeof(ISettings<>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(MigrationsConfigurationContainer<>))
                   .As(typeof(IMigrationsConfigurationContainer<>));
            builder.RegisterGeneric(typeof(BaseDbContext<>))
                   .As(typeof(IBaseDbContext<>));
            builder.RegisterGeneric(typeof(UpdateDbContext<>))
                   .As(typeof(IUpdateDbContext<>));
        }
    }
}