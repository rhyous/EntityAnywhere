using Rhyous.BusinessRules;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IDistinctPropertiesRulesBuilder<TEntity, TInterface, TId>
    {
        IBusinessRuleCollection BuildRules(IEnumerable<TEntity> entities, IEnumerable<PropertyInfo> propertyInfos, ChangeType changeType);
    }
}