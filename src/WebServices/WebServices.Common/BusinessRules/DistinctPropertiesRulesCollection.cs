using Rhyous.BusinessRules;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class DistinctPropertiesRulesCollection : BusinessRuleCollectionBase
    {
        public override string Name => "Distinct Properties Business Rules";

        public override string Description => "Business rules to enforce distinct properties";
    }
}
