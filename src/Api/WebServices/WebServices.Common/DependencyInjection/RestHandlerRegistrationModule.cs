using Autofac;
using Rhyous.SimplePluginLoader;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.EntityAnywhere.PluginLoaders;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;

namespace Rhyous.EntityAnywhere.WebServices.DependencyInjection
{
    /// <summary>
    /// This module class contains the Rest Handlers for the common web services.
    /// However, they should NOT be resolved directly from the WCFScope.
    /// By registering them with the WCFScope, we get Big O(1) registrations, as
    /// opposed to Big O(n) registrations, where N is the number of web service calls.
    /// However, resolving them directly from the WCFScope will result in their dependecies
    /// failing. You must resolve these using <see cref="IRestHandlerProvider"/>'s Provide<T>()
    /// method which is registered in the PerCall scope.
    /// </summary>
    public class RestHandlerRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Endpoint Handlers
            builder.RegisterType<GetMetadataHandler>().As<IGetMetadataHandler>();
            builder.RegisterGeneric(typeof(DeleteHandler<,,>)).As(typeof(IDeleteHandler<,,>));
            builder.RegisterGeneric(typeof(DeleteManyHandler<,,>)).As(typeof(IDeleteManyHandler<,,>));
            builder.RegisterGeneric(typeof(DeleteExtensionHandler<,,>)).As(typeof(IDeleteExtensionHandler<,,>));
            builder.RegisterGeneric(typeof(GenerateRepositoryHandler<,,>)).As(typeof(IGenerateRepositoryHandler<,,>));
            builder.RegisterGeneric(typeof(GetAllHandler<,,>)).As(typeof(IGetAllHandler<,,>));
            builder.RegisterGeneric(typeof(GetByAlternateKeyHandler<,,,>)).As(typeof(IGetByAlternateKeyHandler<,,,>));
            builder.RegisterGeneric(typeof(GetByEntityIdentifiersHandler<,,>)).As(typeof(IGetByEntityIdentifiers<,,>));
            builder.RegisterGeneric(typeof(GetByIdHandler<,,>)).As(typeof(IGetByIdHandler<,,>));
            builder.RegisterGeneric(typeof(GetByIdsHandler<,,>)).As(typeof(IGetByIdsHandler<,,>));
            builder.RegisterGeneric(typeof(GetByMappedIdsHandler<,,,>)).As(typeof(IGetByMappedIdsHandler<,,,>));
            builder.RegisterGeneric(typeof(GetByMappedIdsHandler<,,,>)).As(typeof(IGetByPropertyValuesHandler<,,,>));
            builder.RegisterGeneric(typeof(GetByMappingsHandler<,,,,>)).As(typeof(IGetByMappingsHandler<,,,,>));
            builder.RegisterGeneric(typeof(GetByPropertyValuesHandler<,,>)).As(typeof(IGetByPropertyValuesHandler<,,>));
            builder.RegisterGeneric(typeof(GetByPropertyValuePairsHandler<,,>)).As(typeof(IGetByPropertyValuePairsHandler<,,>));
            builder.RegisterGeneric(typeof(GetCountHandler<,,>)).As(typeof(IGetCountHandler<,,>));
            builder.RegisterGeneric(typeof(GetPropertyHandler<,,>)).As(typeof(IGetPropertyHandler<,,>));
            builder.RegisterGeneric(typeof(InsertSeedDataHandler<,,>)).As(typeof(IInsertSeedDataHandler<,,>));
            builder.RegisterGeneric(typeof(PatchHandler<,,>)).As(typeof(IPatchHandler<,,>));
            builder.RegisterGeneric(typeof(PostHandler<,,>)).As(typeof(IPostHandler<,,>));
            builder.RegisterGeneric(typeof(PostExtensionHandler<,,>)).As(typeof(IPostExtensionHandler<,,>));
            builder.RegisterGeneric(typeof(PutHandler<,,>)).As(typeof(IPutHandler<,,>));
            builder.RegisterGeneric(typeof(UpdateExtensionValueHandler<,,>)).As(typeof(IUpdateExtensionValueHandler<,,>));
            builder.RegisterGeneric(typeof(UpdatePropertyHandler<,,>)).As(typeof(IUpdatePropertyHandler<,,>));
            builder.RegisterGeneric(typeof(GetDistinctPropertyValuesHandler<,,>)).As(typeof(IGetDistinctPropertyValuesHandler<,,>));

            // Handler dependencies
            builder.RegisterGeneric(typeof(AutofacObjectCreator<>)).As(typeof(IObjectCreator<>));
            builder.RegisterGeneric(typeof(EntityRepositoryLoader<,,,>)).As(typeof(IRepositoryLoader<,,,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EntityRepositoryLoaderCommon<,,,>)).As(typeof(IRepositoryLoaderCommon<,,,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RepositoryProxy<,,>)).As(typeof(IRepository<,,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EntityServiceLoader<,,,>)).As(typeof(IEntityServiceLoader<,,,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EntityServiceLoaderCommon<,,,>)).As(typeof(IEntityServiceLoaderCommon<,,,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(ServiceProxy<,,>)).As(typeof(IServiceCommon<,,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(ServiceProxyAlternateKey<,,,>)).As(typeof(IServiceCommonAlternateKey<,,,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EntityEventLoader<,>));
            builder.RegisterGeneric(typeof(EntityEventLoaderCommon<,>)).As(typeof(IEntityEventLoaderCommon<,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EntityEventProxy<,>)).As(typeof(IEntityEventAll<,>));
            builder.RegisterGeneric(typeof(DistinctPropertiesEnforcer<,,>)).As(typeof(IDistinctPropertiesEnforcer<,,>));
            builder.RegisterGeneric(typeof(RelatedEntityEnforcer<>)).As(typeof(IRelatedEntityEnforcer<>));
            builder.RegisterGeneric(typeof(RelatedEntityRulesBuilder<>)).As(typeof(IRelatedEntityRulesBuilder<>));
        }
    }
}
