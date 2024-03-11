using Rhyous.EntityAnywhere.Entities;
using System.Collections.Concurrent;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class EntityGroupDictionary : ConcurrentDictionary<string, EntityGroup>, IEntityGroupDictionary
    {
    }
}