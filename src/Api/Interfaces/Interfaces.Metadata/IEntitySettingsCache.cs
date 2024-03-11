using Rhyous.Collections;
using System;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntitySettingsCache : ICacheBase<IEntitySettingsDictionary>
    {
        EntitySettings Provide(Type entityType);
    }
}