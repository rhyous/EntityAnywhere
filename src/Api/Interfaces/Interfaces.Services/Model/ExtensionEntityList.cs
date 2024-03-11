using Rhyous.Collections;
using Rhyous.EntityAnywhere.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>Caches ExtensionEntity AutoExpand settings as well as the Entities, however, Entities does not clear. Only AutoExpand settings clears.</summary>
    public class ExtensionEntityList : CacheBase<ConcurrentDictionaryWrapper<string, bool>>, IExtensionEntityList
    {
        const bool DefaultAutoExpand = false;

        public ExtensionEntityList() : base(new ConcurrentDictionaryWrapper<string, bool>())
        {
        }

        public List<Type> Entities
        {
            get { return _ExtensionEntities ?? (_ExtensionEntities = new List<Type>()); }
        } private List<Type> _ExtensionEntities;

        public IEnumerable<string> EntityNames => Entities?.Select(t => t.Name);

        public bool ShouldAutoExpand(string name)
        {
            if (!_Cache.TryGetValue(name, out bool value))
            {
                CreateCacheAsync(); // Doesn't need to be awaited
                _Cache.TryGetValue(name, out value);
            }        
            return value;
        }

        protected override Task CreateCacheAsync()
        {
            foreach (var entityType in Entities)
            {
                var att = entityType.GetAttribute<ExtensionEntityAttribute>();
                var value = att == null ? DefaultAutoExpand : att.AutoExpand;
                _Cache.TryAdd(entityType.Name, value);
            }
            return Task.CompletedTask;
        }
    }
}
