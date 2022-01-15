using Rhyous.Odata;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    public interface IRelatedEntitySorterHelper<TInterface, TId>
    {
        void Sort(IEnumerable<TInterface> entities, RelatedEntityCollection relatedEntities, SortDetails sortDetails, List<RelatedEntityCollection> list);
    }
}