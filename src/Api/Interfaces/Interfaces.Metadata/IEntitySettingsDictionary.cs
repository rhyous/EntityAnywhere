using Rhyous.Collections;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntitySettingsDictionary : IConcurrentDictionary<string, EntitySettings>, IClearable, ICountable { }
}
