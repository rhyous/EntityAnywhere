using Autofac;
using Autofac.Core;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Clients2.DependencyInjection
{
    public class AdminEntityClientModule : Module
    {
        private readonly IEntityList _EntityList;
        private readonly IExtensionEntityList _ExtensionEntityList;
        private readonly IMappingEntityList _MappingEntityList;

        public AdminEntityClientModule(IEntityList entityList,
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
                builder.RegisterType<AdminEntityClientAsync>()
                       .Named<IAdminEntityClientAsync>(entity)
                       .WithParameter(new ResolvedParameter(
                           (pi, ctx) => pi.ParameterType == typeof(IEntityClientConnectionSettings) && pi.Name == "entityClientSettings",
                           (pi, ctx) => ctx.ResolveNamed<IEntityClientConnectionSettings>(entity)))
                       .SingleInstance();
            }
            foreach (var extensionEntity in _ExtensionEntityList.EntityNames)
            {
                builder.RegisterType<AdminExtensionEntityClientAsync>()
                       .Named<IAdminExtensionEntityClientAsync>(extensionEntity)
                       .WithParameter(new ResolvedParameter(
                           (pi, ctx) => pi.ParameterType == typeof(IEntityClientConnectionSettings) && pi.Name == "entityClientSettings",
                           (pi, ctx) => ctx.ResolveNamed<IEntityClientConnectionSettings>(extensionEntity)))
                       .SingleInstance();
            }
            foreach (var mappingEntity in _MappingEntityList.EntityNames)
            {
                builder.RegisterType<AdminMappingEntityClientAsync>()
                       .Named<IAdminMappingEntityClientAsync>(mappingEntity)
                       .WithParameter(new ResolvedParameter(
                           (pi, ctx) => pi.ParameterType == typeof(IEntityClientConnectionSettings) && pi.Name == "entityClientSettings",
                           (pi, ctx) => ctx.ResolveNamed<IEntityClientConnectionSettings>(mappingEntity)))
                       .WithParameter(new ResolvedParameter(
                           (pi, ctx) => pi.ParameterType == typeof(IMappingEntitySettings) && pi.Name == "mappingEntitySettings",
                           (pi, ctx) => ctx.ResolveNamed<IMappingEntitySettings>(mappingEntity)))
                       .SingleInstance();
            }

            // Entity Clients (Generic)
            builder.RegisterGeneric(typeof(AdminEntityClientAsync<,>))
                   .As(typeof(IAdminEntityClientAsync<,>))
                   .SingleInstance();

            builder.RegisterGeneric(typeof(AdminExtensionEntityClientAsync<,>))
                   .As(typeof(IAdminExtensionEntityClientAsync<,>))
                   .SingleInstance();

            builder.RegisterGeneric(typeof(AdminMappingEntityClientAsync<,,,>))
                   .As(typeof(IAdminMappingEntityClientAsync<,,,>))
                   .SingleInstance();


            builder.RegisterType<AdminHeaders>()
                   .As<IAdminHeaders>()
                   .SingleInstance();

            builder.RegisterType<AdminHttpClientRunner>()
                   .As<IAdminHttpClientRunner>();
        }
    }
}