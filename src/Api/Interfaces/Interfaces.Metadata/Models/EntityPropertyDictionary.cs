using Rhyous.Collections;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class EntityPropertyDictionary : SortedConcurrentDictionary<string, EntityProperty>, IEntityPropertyDictionary
    {
    }
}