using Rhyous.Odata.Csdl;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services
{
    internal class PropertyFuncProvider : IPropertyFuncProvider
    {
        private readonly IDictionary<string, EntitySetting> _EntitySettings;

        public PropertyFuncProvider(IDictionary<string, EntitySetting> entitySettings)
        {
            _EntitySettings = entitySettings;
        }

        public IFuncList<string> Provide()
        {
            if (_EntitySettings == null)
                return null;
            return new FuncList<string> { GetPropertyData };
        }

        public IEnumerable<KeyValuePair<string, object>> GetPropertyData(string entity)
        {
            if (!_EntitySettings.TryGetValue(entity, out EntitySetting entitySetting))
                return null;
            var group = new KeyValuePair<string, object>("@EAF.EntityGroup", entitySetting.EntityGroup);
            return new KeyValuePair<string, object>[] { group };
        }
    }
}