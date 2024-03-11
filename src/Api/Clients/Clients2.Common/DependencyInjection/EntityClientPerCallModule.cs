using Autofac;
using Autofac.Core;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Clients2.DependencyInjection
{
    /// <summary>
    /// This module should be registered per web call
    /// </summary>
    public class EntityClientPerCallModule : Module
    {
        private readonly IEntityList _EntityList;
        private readonly IExtensionEntityList _ExtensionEntityList;
        private readonly IMappingEntityList _MappingEntityList;

        public EntityClientPerCallModule(IEntityList entityList,
                                         IExtensionEntityList extensionEntityList,
                                         IMappingEntityList mappingEntityList)
        {
            _EntityList = entityList;
            _ExtensionEntityList = extensionEntityList;
            _MappingEntityList = mappingEntityList;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Headers>()
                   .As<IHeaders>()
                   .IfNotRegistered(typeof(IHeaders))
                   .SingleInstance();

            // Entity Clients (non-generic)
            foreach (var entity in _EntityList.EntityNames)
            {
                builder.RegisterType<EntityClientAsync>()
                       .Named<IEntityClientAsync>(entity)
                       .WithParameter(new ResolvedParameter(
                           (pi, ctx) => pi.ParameterType == typeof(IEntityClientConnectionSettings) && pi.Name == "entityClientSettings",
                           (pi, ctx) => ctx.ResolveNamed<IEntityClientConnectionSettings>(entity)))
                       .SingleInstance();
            }
            foreach (var extensionEntity in _ExtensionEntityList.EntityNames)
            {
                builder.RegisterType<ExtensionEntityClientAsync>()
                       .Named<IExtensionEntityClientAsync>(extensionEntity)
                       .WithParameter(new ResolvedParameter(
                           (pi, ctx) => pi.ParameterType == typeof(IEntityClientConnectionSettings) && pi.Name == "entityClientSettings",
                           (pi, ctx) => ctx.ResolveNamed<IEntityClientConnectionSettings>(extensionEntity)))
                       .SingleInstance();
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
                       .SingleInstance();
            }

            // Entity Clients (Generic)
            builder.RegisterGeneric(typeof(EntityClientAsync<,>))
                   .As(typeof(IEntityClientAsync<,>))
                   .SingleInstance();

            builder.RegisterGeneric(typeof(ExtensionEntityClientAsync<,>))
                   .As(typeof(IExtensionEntityClientAsync<,>))
                   .SingleInstance();

            builder.RegisterGeneric(typeof(MappingEntityClientAsync<,,,>))
                   .As(typeof(IMappingEntityClientAsync<,,,>))
                   .SingleInstance();

            // EntityProvider
            builder.RegisterGeneric(typeof(EntityProvider<,>))
                   .As(typeof(IEntityProvider<,>))
                   .SingleInstance();

            // EntityClient's dependencies
            builder.RegisterType<HttpClientRunner>()
                   .As<IHttpClientRunner>()
                   .SingleInstance();
        }
    }
}