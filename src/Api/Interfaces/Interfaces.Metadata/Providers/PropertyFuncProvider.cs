using Rhyous.Odata.Csdl;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    internal class PropertyFuncProvider : IPropertyFuncProvider
    {
        internal const string EntityGroupPropertyName = "@EAF.EntityGroup";
        private const string Miscellaneous = nameof(Miscellaneous);
        private readonly IEntitySettingsCache _EntitySettingsCache;

        public PropertyFuncProvider(IEntitySettingsCache entitySettingsCache)
        {
            _EntitySettingsCache = entitySettingsCache;
        }

        public ICustomPropertyFuncs Provide()
        {
            return new CustomPropertyFuncs { GetPropertyData };
        }

        public IEnumerable<KeyValuePair<string, object>> GetPropertyData(string entity)
        {
            var entitySettingsDictionary = _EntitySettingsCache.ProvideAsync().Result;
            if (!entitySettingsDictionary.TryGetValue(entity, out EntitySettings entitySetting))
                return null;
            var group = new KeyValuePair<string, object>(EntityGroupPropertyName, entitySetting.EntityGroup?.Name ?? Miscellaneous);
            return new KeyValuePair<string, object>[] { group };
        }
    }
}