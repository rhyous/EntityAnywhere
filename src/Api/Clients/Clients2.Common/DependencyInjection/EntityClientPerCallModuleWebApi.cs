using Autofac;
using Autofac.Core;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Clients2.DependencyInjection
{
    /// <summary>
    /// This module should be registered in the WebApi scope and items should be resolved InstancePerLifetimeScope
    /// </summary>
    public class EntityClientPerCallModuleWebApi : Module
    {
        private readonly IEntityList _EntityList;
        private readonly IExtensionEntityList _ExtensionEntityList;
        private readonly IMappingEntityList _MappingEntityList;

        public EntityClientPerCallModuleWebApi(IEntityList entityList,
                                               IExtensionEntityList extensionEntityList,
                                               IMappingEntityList mappingEntityList)
        {
            _EntityList = entityList;
            _ExtensionEntityList = extensionEntityList;
            _MappingEntityList = mappingEntityList;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Entity Clients (non-generic)
            foreach (var entity in _EntityList.EntityNames)
            {
                builder.RegisterType<EntityClientAsync>()
                       .Named<IEntityClientAsync>(entity)
                       .WithParameter(new ResolvedParameter(
                           (pi, ctx) => pi.ParameterType == typeof(IEntityClientConnectionSettings) && pi.Name == "entityClientSettings",
                           (pi, ctx) => ctx.ResolveNamed<IEntityClientConnectionSettings>(entity)))
                       .InstancePerLifetimeScope();
            }
            foreach (var extensionEntity in _ExtensionEntityList.EntityNames)
            {
                builder.RegisterType<ExtensionEntityClientAsync>()
                       .Named<IExtensionEntityClientAsync>(extensionEntity)
                       .WithParameter(new ResolvedParameter(
                           (pi, ctx) => pi.ParameterType == typeof(IEntityClientConnectionSettings) && pi.Name == "entityClientSettings",
                           (pi, ctx) => ctx.ResolveNamed<IEntityClientConnectionSettings>(extensionEntity)))
                       .InstancePerLifetimeScope();
            }
            foreach (var mappingEntity in _MappingEntityList.EntityNames)
            {
                builder.RegisterType<MappingEntityClientAsync>()
                       .Named<IMappingEntityClientAsync>(mappingEntity)
                       .WithParameter(new ResolvedParameter(
                           (pi, ctx) => pi.ParameterType == typeof(IEntityClientConnectionSettings) && pi.Name == "entityClientSettings",
                           (pi, ctx) => ctx.ResolveNamed<IEntityClientConnectionSettings>(mappingEntity)))
                       .WithParameter(new ResolvedParameter(
                           (pi, ctx) => pi.ParameterType == typeof(IMappingEntitySettings) && pi.Name == "mappingEntitySettings",
                           (pi, ctx) => ctx.ResolveNamed<IMappingEntitySettings>(mappingEntity)))
                       .InstancePerLifetimeScope();
            }

            // Entity Clients (Generic)
            builder.RegisterGeneric(typeof(EntityClientAsync<,>))
                   .As(typeof(IEntityClientAsync<,>))
                   .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ExtensionEntityClientAsync<,>))
                   .As(typeof(IExtensionEntityClientAsync<,>))
                   .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(MappingEntityClientAsync<,,,>))
                   .As(typeof(IMappingEntityClientAsync<,,,>))
                   .InstancePerLifetimeScope();

            // EntityProvider
            builder.RegisterGeneric(typeof(EntityProvider<,>))
                   .As(typeof(IEntityProvider<,>))
                   .InstancePerLifetimeScope();

            // EntityClient's dependencies
            builder.RegisterType<HttpClientRunner>()
                   .As<IHttpClientRunner>()
                   .InstancePerLifetimeScope();
        }
    }
}