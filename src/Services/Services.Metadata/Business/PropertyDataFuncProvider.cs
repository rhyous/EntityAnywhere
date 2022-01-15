using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Services
{
    internal class PropertyDataFuncProvider : IPropertyDataFuncProvider
    {
        private readonly IDictionary<string, EntitySetting> _EntitySettings;

        public PropertyDataFuncProvider(IDictionary<string, EntitySetting> entitySettings)
        {
            _EntitySettings = entitySettings;
        }

        public IFuncList<string, string> Provide()
        {
            if (_EntitySettings == null)
                return null;
            return new FuncList<string, string> { GetPropertyData };
        }

        public IEnumerable<KeyValuePair<string, object>> GetPropertyData(string entity, string property)
        {
            if (!_EntitySettings.TryGetValue(entity, out EntitySetting entitySetting)
             || !entitySetting.EntityProperties.Any())
                return null;
            if (!entitySetting.EntityProperties.TryGetValue(property, out IEntityProperty entityProperty))
                return null;
            var order = new KeyValuePair<string, object>("@UI.DisplayOrder", entityProperty.Order);
            var searchable = new KeyValuePair<string, object>("@UI.Searchable", entityProperty.Searchable);
            return new KeyValuePair<string, object>[] { order, searchable };
        }
    }
}
