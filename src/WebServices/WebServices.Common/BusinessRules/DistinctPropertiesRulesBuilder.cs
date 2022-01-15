using Rhyous.BusinessRules;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class DistinctPropertiesRulesBuilder<TEntity, TInterface, TId> : IDistinctPropertiesRulesBuilder<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public DistinctPropertiesRulesBuilder(IServiceCommon<TEntity, TInterface, TId> service)
        {
            Service = service;
        }

        public IBusinessRuleCollection BuildRules(IEnumerable<TEntity> entities, IEnumerable<PropertyInfo> propertyInfos, ChangeType changeType)
        {
            var collection = new DistinctPropertiesRulesCollection();

            collection.Rules.Add(new DistinctPropertiesMustBeUniqueRule<TEntity, TInterface, TId>(entities, propertyInfos, Service, changeType));

            return collection;
        }

        public IServiceCommon<TEntity, TInterface, TId> Service { get; }
    }
}
