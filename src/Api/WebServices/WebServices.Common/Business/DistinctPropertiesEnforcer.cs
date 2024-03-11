using Rhyous.BusinessRules;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class DistinctPropertiesEnforcer<TEntity, TInterface, TId> : IDistinctPropertiesEnforcer<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public DistinctPropertiesEnforcer(IServiceCommon<TEntity, TInterface, TId> service)
        {
            Service = service;
        }

        // POSTS
        public Task Enforce(IEnumerable<TEntity> entities, ChangeType changeType)
        {
            var properties = typeof(TEntity).GetProperties<DistinctPropertyAttribute>();

            if (properties != null)
            {
                var addRulesCollection = RulesBuilder.BuildRules(entities, properties, changeType);
                if (!addRulesCollection.IsMet().Result)
                    throw new BusinessRulesNotMetException(addRulesCollection);
            }
            return Task.CompletedTask;
        }

        public IDistinctPropertiesRulesBuilder<TEntity, TInterface, TId> RulesBuilder
        {
            get { return _RulesBuilder ?? (_RulesBuilder = new DistinctPropertiesRulesBuilder<TEntity, TInterface, TId>(Service)); }
            set { _RulesBuilder = value; }
        }

        public IServiceCommon<TEntity, TInterface, TId> Service { get; }

        private IDistinctPropertiesRulesBuilder<TEntity, TInterface, TId> _RulesBuilder;

    }
}
