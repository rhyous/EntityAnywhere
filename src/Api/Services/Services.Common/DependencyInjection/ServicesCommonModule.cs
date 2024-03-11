using Autofac;

namespace Rhyous.EntityAnywhere.Services.DependencyInjection
{
    public class ServicesCommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Handlers
            builder.RegisterType<ServiceHandlerProvider>().As<IServiceHandlerProvider>()
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
            builder.RegisterGeneric(typeof(GetDistinctPropertyValueHandler<,,>)).As(typeof(IGetDistinctPropertyValuesHandler<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(GetSelectPropertiesHandler<,,>)).As(typeof(IGetPropertyValueHandler<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(InsertSeedDataHandler<,,>)).As(typeof(IInsertSeedDataHandler<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(QueryableHandler<,,>)).As(typeof(IQueryableHandler<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(UpdateHandler<,,>)).As(typeof(IUpdateHandler<,,>))
                   .SingleInstance();

            // AltKey Handlers
            builder.RegisterGeneric(typeof(DuplicateEntityPreventer<,,,>)).As(typeof(IDuplicateEntityPreventer<,,,>))
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