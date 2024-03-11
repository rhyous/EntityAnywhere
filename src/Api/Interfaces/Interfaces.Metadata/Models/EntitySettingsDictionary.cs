using System.Collections.Concurrent;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class EntitySettingsDictionary : ConcurrentDictionary<string, EntitySettings>, IEntitySettingsDictionary
    {
    }
}
