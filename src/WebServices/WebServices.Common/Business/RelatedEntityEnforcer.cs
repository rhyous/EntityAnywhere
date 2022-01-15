using Rhyous.BusinessRules;
using Rhyous.Odata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    // Ignoring warning until BusiessRule IsMet is Async
    public class RelatedEntityEnforcer<TEntity> : IRelatedEntityEnforcer<TEntity>
        where TEntity : new()
    {
        private readonly IRelatedEntityRulesBuilder<TEntity> RulesBuilder;

        public RelatedEntityEnforcer(IRelatedEntityRulesBuilder<TEntity> rulesBuilder)
        {
            RulesBuilder = rulesBuilder;
        }

        public Task Enforce(IEnumerable<TEntity> entities, IEnumerable<string> changedProperties = null)
        {
            var relatedEntityAttributes = GetRelatedEntityAttributes(typeof(TEntity));
            var addRulesCollection = RulesBuilder.BuildRules(entities, relatedEntityAttributes, changedProperties);
            if (!addRulesCollection.IsMet().Result)
                throw new BusinessRulesNotMetException(addRulesCollection);
            return Task.CompletedTask;
        }

        private IEnumerable<RelatedEntityAttribute> GetRelatedEntityAttributes(Type entityType)
        {
            return entityType.GetProperties().Select(p => p.GetCustomAttribute<RelatedEntityAttribute>(true)).Where(a => a != null);
        }
    }
}