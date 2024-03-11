using Rhyous.BusinessRules;
using Rhyous.Odata;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IRelatedEntityRulesBuilder<TEntity>
    {
        IBusinessRuleCollection BuildRules(IEnumerable<TEntity> entities, IEnumerable<RelatedEntityAttribute> relatedEntityAttributes, IEnumerable<string> changedProperties = null);
    }
}