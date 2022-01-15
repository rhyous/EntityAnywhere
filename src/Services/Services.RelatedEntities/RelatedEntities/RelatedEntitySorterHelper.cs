using Rhyous.Odata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    /// <summary>
    /// This class just makes sure that there is something to sort and something to add 
    /// before trying to sort and add.
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    public class RelatedEntitySorterHelper<TInterface, TId>
        : IRelatedEntitySorterHelper<TInterface, TId>
    {
        private readonly IRelatedEntitySorterWrapper<TInterface, TId> _Sorter;

        public RelatedEntitySorterHelper(IRelatedEntitySorterWrapper<TInterface, TId> sorter)
        {
            _Sorter = sorter;
        }

        public void Sort(IEnumerable<TInterface> entities, RelatedEntityCollection relatedEntities, SortDetails sortDetails, List<RelatedEntityCollection> list)
        {
            if (relatedEntities == null || !relatedEntities.Any() || entities == null || !entities.Any())
                return;
            var collections = _Sorter.Sort(entities, relatedEntities, sortDetails);
            if (collections != null && collections.Any())
            {
                list.AddRange(collections);
            }
        }
    }
}