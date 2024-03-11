using Rhyous.Collections;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityPropertyDictionary : IConcurrentDictionary<string, EntityProperty>
    { }
}