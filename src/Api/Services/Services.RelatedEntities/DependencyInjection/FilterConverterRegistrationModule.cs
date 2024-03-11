using Autofac;
using Autofac.Core;
using Rhyous.Odata.Csdl;
using Rhyous.Odata.Filter;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    /// <summary>A module to be registered just in time to use it.</summary>
    /// <remarks>
    /// This can't be registered per call as that would create an infinite loop. It must only be created by calls that need it,
    /// which excludes the $Metadata service call, as it needs that call to succeed in order register.
    /// </remarks>
    public class FilterConverterRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Related Entity
            builder.RegisterGeneric(typeof(CustomFilterConvertersRunner<>))
                   .As(typeof(ICustomFilterConvertersRunner<>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(CustomFilterConverterCollection<>))
                   .As(typeof(ICustomFilterConverterCollection<>))
                   .SingleInstance();
            builder.RegisterType<RelatedEntityFilterDataProvider>()
                   .As<IRelatedEntityFilterDataProvider>()
                   .SingleInstance();
            var resolvedparam = new ResolvedParameter((pi, ctx) => pi.ParameterType == typeof(CsdlSchema),
                                                      (pi, ctx) => ctx.Resolve<IMetadataCache>().GetCsdlSchema());
            builder.RegisterGeneric(typeof(RelatedEntityFilterConverter<>))
                   .As(typeof(IRelatedEntityFilterConverter<>))
                   .WithParameter(resolvedparam)
                   .SingleInstance();
            builder.RegisterGeneric(typeof(RelatedEntityExtensionFilterConverter<>))
                   .As(typeof(IRelatedEntityExtensionFilterConverter<>))
                   .WithParameter(resolvedparam)
                   .SingleInstance();
        }
    }
}