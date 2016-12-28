using System.Collections.Generic;

namespace Rhyous.BusinessRules
{
    public interface IBusinessRuleCollection : IBusinessRule
    {
        List<IBusinessRule> Rules { get; set; }
        Dictionary<IBusinessRule, BusinessRuleResult> Results { get; set; }
    }
}
