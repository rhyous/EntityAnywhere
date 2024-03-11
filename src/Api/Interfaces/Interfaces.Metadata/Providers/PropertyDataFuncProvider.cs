using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Entities;
using System.Collections.Generic;
using Rhyous.EntityAnywhere.Tools;

namespace Rhyous.EntityAnywhere.Interfaces
{
    internal class PropertyDataFuncProvider : IPropertyDataFuncProvider
    {
        internal const string UIDisplayOrder = "@UI.DisplayOrder";
        internal const string UISearchable = "@UI.Searchable";
        private readonly IEntitySettingsCache _EntitySettingsCache;

        public PropertyDataFuncProvider(IEntitySettingsCache entitySettingsCache)
        {
            _EntitySettingsCache = entitySettingsCache;
        }

        public ICustomPropertyDataFuncs Provide()
        {
            if (_EntitySettingsCache == null)
                return null;
            return new CustomPropertyDataFuncs { GetPropertyData };
        }

        /// <summary>Lazy instantiation. Create the cache once, the first ime you need it, but not in the constructor.</summary>
        internal IEntitySettingsDictionary EntitySettingsDictionary => _EntitySettingsDictionary ?? (_EntitySettingsDictionary = TaskRunner.RunSynchonously(_EntitySettingsCache.ProvideAsync, false));
        private IEntitySettingsDictionary _EntitySettingsDictionary;

        public IEnumerable<KeyValuePair<string, object>> GetPropertyData(string entity, string property)
        {
            if (!EntitySettingsDictionary.TryGetValue(entity, out EntitySettings entitySetting))
                return null;
            if (!entitySetting.EntityProperties.TryGetValue(property, out EntityProperty entityProperty))
                return null;
            var order = new KeyValuePair<string, object>(UIDisplayOrder, entityProperty.Order);
            var searchable = new KeyValuePair<string, object>(UISearchable, entityProperty.Searchable);
            return new KeyValuePair<string, object>[] { order, searchable };
        }
    }
}
