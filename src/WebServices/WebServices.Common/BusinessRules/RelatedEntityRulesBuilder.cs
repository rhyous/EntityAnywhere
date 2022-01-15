using Rhyous.BusinessRules;
using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class RelatedEntityRulesBuilder<TEntity> : IRelatedEntityRulesBuilder<TEntity>
    {
        private readonly INamedFactory<IEntityClientAsync> _EntityClientFactory;

        public RelatedEntityRulesBuilder(INamedFactory<IEntityClientAsync> entityClientFactory)
        {
            _EntityClientFactory = entityClientFactory;
        }

        public IBusinessRuleCollection BuildRules(IEnumerable<TEntity> entities, IEnumerable<RelatedEntityAttribute> relatedEntityAttributes, IEnumerable<string> changedProperties = null)
        {
            var collection = new RelatedEntityRulesCollection();

            if (changedProperties != null)
            {
                relatedEntityAttributes = relatedEntityAttributes.Where(x => changedProperties.Contains(x.Property));
            }

            foreach (var attribute in relatedEntityAttributes)
            {
                if (attribute.RelatedEntityMustExist)
                {
                    var rule = new RelatedEntityMustExistRule(_EntityClientFactory.Create(attribute.RelatedEntity), 
                                                              entities.Select(e => e.GetPropertyValue(attribute.Property)?.ToString()),
                                                              attribute.AllowedNonExistentValue,
                                                              attribute.Nullable);
                    collection.Rules.Add(rule);
                }
            }
            return collection;
        }
    }
}