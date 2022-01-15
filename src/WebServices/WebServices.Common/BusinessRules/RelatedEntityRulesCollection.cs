using Rhyous.BusinessRules;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class RelatedEntityRulesCollection : BusinessRuleCollectionBase
    {
        public override string Name => "Related Entity Business Rules";

        public override string Description => "Business rules to enforce related entity states";
    }
}
