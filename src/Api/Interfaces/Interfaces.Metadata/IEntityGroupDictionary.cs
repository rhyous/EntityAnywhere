using Rhyous.Collections;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityGroupDictionary : IConcurrentDictionary<string, EntityGroup>, IClearable, ICountable { }
}