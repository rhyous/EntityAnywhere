using Autofac;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.DependencyInjection
{
    public class ServicesCommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EntityConfigurationProvider>().As<IEntityConfigurationProvider>()
                   .InstancePerLifetimeScope();

            // Handlers
            builder.RegisterGeneric(typeof(ServiceHandlerProvider<,,>)).As(typeof(IServiceHandlerProvider<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(AddHandler<,,>)).As(typeof(IAddHandler<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(DeleteHandler<,,>)).As(typeof(IDeleteHandler<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(GenerateRepositoryHandler<,,>)).As(typeof(IGenerateRepositoryHandler<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(GetByIdHandler<,,>)).As(typeof(IGetByIdHandler<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(GetByIdsHandler<,,>)).As(typeof(IGetByIdsHandler<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(GetByPropertyValuesHandler<,,>)).As(typeof(IGetByPropertyValuesHandler<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(GetPropertyValueHandler<,,>)).As(typeof(IGetPropertyValueHandler<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(InsertSeedDataHandler<,,>)).As(typeof(IInsertSeedDataHandler<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(QueryableHandler<,,>)).As(typeof(IQueryableHandler<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(UpdateHandler<,,>)).As(typeof(IUpdateHandler<,,>))
                   .SingleInstance();

            // AltKey Handlers
            builder.RegisterGeneric(typeof(ServiceHandlerProviderAltKey<,,,>)).As(typeof(IServiceHandlerProviderAltKey<,,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(AddAltKeyHandler<,,,>)).As(typeof(IAddAltKeyHandler<,,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(GetByAlternateKeyHandler<,,,>)).As(typeof(IGetByAlternateKeyHandler<,,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(SearchByAlternateKeyHandler<,,,>)).As(typeof(ISearchByAlternateKeyHandler<,,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(UpdateAltKeyHandler<,,,>)).As(typeof(IUpdateAltKeyHandler<,,,>))
                   .SingleInstance();

        }
    }
}