using Rhyous.Odata;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    /// <summary>
    /// This only exists because IRelatedEntitySorter<T> in Rhyous.Odata
    /// only has one generic argument, but its implementations have two,
    /// and Autofac doesn't have a way to know how to implement that.
    /// </summary>
    /// <typeparam name="TInterface">The interface</typeparam>
    /// <typeparam name="TId">The type of the Id property.</typeparam>
    public class RelatedEntitySorterWrapper<TInterface, TId> 
        : RelatedEntitySorter<TInterface, TId>,
          IRelatedEntitySorterWrapper<TInterface, TId>        
    {
    }
}