using Autofac;
using Rhyous.Odata;
using Rhyous.Odata.Expand;
using Rhyous.Odata.Filter;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    /// <summary>A module to be registered per call by a child per call scope.</summary>
    /// <remarks>Because this is per call, SingleInstance means one per call.</remarks>
    public class RelatedEntityRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AttributeEvaluator>()
                   .SingleInstance();
            builder.RegisterType<ExpandParser>()
                   .SingleInstance();
            builder.RegisterGeneric(typeof(RelatedEntitySorterWrapper<,>))
                   .As(typeof(IRelatedEntitySorterWrapper<,>))
                   .As(typeof(IRelatedEntityCollater<,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(RelatedEntitySorterHelper<,>))
                   .As(typeof(IRelatedEntitySorterHelper<,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(RelatedEntityExtensions<,,>))
                   .As(typeof(IRelatedEntityExtensions<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(RelatedEntityManyToOne<,,>))
                   .As(typeof(IRelatedEntityManyToOne<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(RelatedEntityOneToMany<,,>))
                   .As(typeof(IRelatedEntityOneToMany<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(RelatedEntityManyToMany<,,>))
                   .As(typeof(IRelatedEntityManyToMany<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(RelatedEntityAccessors<,,>))
                   .As(typeof(IRelatedEntityAccessors<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(RelatedEntityManager<,,>))
                   .As(typeof(IRelatedEntityManager<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(RelatedEntityProvider<,,>))
                   .As(typeof(IRelatedEntityProvider<,,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(CustomFilterConvertersRunnerProxy<>))
                   .As(typeof(ICustomFilterConvertersRunner<>))
                   .SingleInstance();
        }
    }
}