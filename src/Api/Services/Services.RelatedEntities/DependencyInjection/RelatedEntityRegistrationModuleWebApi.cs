using Autofac;
using Rhyous.Odata;
using Rhyous.Odata.Expand;
using Rhyous.Odata.Filter;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    /// <summary>A module to be registered per call by a child per call scope.</summary>
    /// <remarks>Because this is per call, InstancePerLifetimeScope means one per call.</remarks>
    public class RelatedEntityRegistrationModuleWebApi : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AttributeEvaluator>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<ExpandParser>()
                   .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RelatedEntitySorterWrapper<,>))
                   .As(typeof(IRelatedEntitySorterWrapper<,>))
                   .As(typeof(IRelatedEntityCollater<,>))
                   .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RelatedEntitySorterHelper<,>))
                   .As(typeof(IRelatedEntitySorterHelper<,>))
                   .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RelatedEntityExtensions<,,>))
                   .As(typeof(IRelatedEntityExtensions<,,>))
                   .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RelatedEntityManyToOne<,,>))
                   .As(typeof(IRelatedEntityManyToOne<,,>))
                   .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RelatedEntityOneToMany<,,>))
                   .As(typeof(IRelatedEntityOneToMany<,,>))
                   .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RelatedEntityManyToMany<,,>))
                   .As(typeof(IRelatedEntityManyToMany<,,>))
                   .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RelatedEntityAccessors<,,>))
                   .As(typeof(IRelatedEntityAccessors<,,>))
                   .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RelatedEntityManager<,,>))
                   .As(typeof(IRelatedEntityManager<,,>))
                   .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RelatedEntityProvider<,,>))
                   .As(typeof(IRelatedEntityProvider<,,>))
                   .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(CustomFilterConvertersRunnerProxy<>))
                   .As(typeof(ICustomFilterConvertersRunner<>))
                   .InstancePerLifetimeScope();
        }
    }
}