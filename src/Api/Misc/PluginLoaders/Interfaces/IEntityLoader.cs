using Rhyous.SimplePluginLoader;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces

{
    public interface IEntityLoader : IRuntimePluginLoader<IBaseEntity>
    {
        void Load(IEnumerable<Type> entityTypes);
    }
}